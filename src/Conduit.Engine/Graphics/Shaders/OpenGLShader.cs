using Conduit.Engine.Content;
using Conduit.Engine.Graphics.Textures;
using Silk.NET.OpenGL;
using System.Numerics;
using TextureWrapMode = Conduit.Engine.Graphics.Textures.TextureWrapMode;
using TextureFilterMode = Conduit.Engine.Graphics.Textures.TextureFilterMode;
using System.Diagnostics;

namespace Conduit.Engine.Graphics.Shaders
{
    public class OpenGLShader : IShader
    {
        public uint Handle { get; }
        private Dictionary<string, int> _uniformLocations = new();
        private readonly GL _gl;
        internal OpenGLShader(GL gl, uint handle)
        {
            _gl = gl;
            Handle = handle;
        }

        private int GetUniformLocation(string name)
        {
            if (_uniformLocations.TryGetValue(name, out int value))
                return value;

            int loc = _gl.GetUniformLocation(Handle, name);
            _uniformLocations[name] = loc;

            return loc;
        }

        public void Bind()
        {
            _gl.UseProgram(Handle);
        }

        public void Dispose() => _gl.DeleteProgram(Handle);

        public void SetUniform<T>(string name, T value)
        {
            switch (value)
            {
                case int i:
                    _gl.Uniform1(GetUniformLocation(name), i);
                    break;
                case float f:
                    _gl.Uniform1(GetUniformLocation(name), f);
                    break;
                case bool b:
                    _gl.Uniform1(GetUniformLocation(name), b ? 1 : 0);
                    break;
                case Vector2 v2:
                    _gl.Uniform2(GetUniformLocation(name), v2);
                    break;
                case Vector3 v3:
                    _gl.Uniform3(GetUniformLocation(name), v3);
                    break;
                case Vector4 v4:
                    _gl.Uniform4(GetUniformLocation(name), v4);
                    break;
                case ITexture t:
                    _gl.ActiveTexture(TextureUnit.Texture0);
                    _gl.BindTexture(TextureTarget.Texture2D, t.Handle);

                    _gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureWrapS, (int)ToGLEnum(t.WrapU));
                    _gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureWrapT, (int)ToGLEnum(t.WrapV));

                    _gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureMinFilter, t.HasMipmaps 
                        ? (int)ToGLEnum(t.FilterMode, t.MipmapFilterMode) 
                        : (int)ToGLEnum(t.FilterMode));
                    _gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)ToGLEnum(t.FilterMode));

                    _gl.Uniform1(GetUniformLocation(name), 0);
                    break;
                case Matrix4x4 m4:
                    _gl.UniformMatrix4(GetUniformLocation(name), 1, false, ref m4.M11);
                    break;
                default:
                    throw new Exception($"Unsupported property type: {value?.GetType()}");
            }
        }

        private GLEnum ToGLEnum(TextureWrapMode wrapMode)
        {
            return wrapMode switch
            {
                TextureWrapMode.Repeat => GLEnum.Repeat,
                TextureWrapMode.ClampToEdge => GLEnum.ClampToEdge,
                TextureWrapMode.MirrorRepeat => GLEnum.MirroredRepeat,
                TextureWrapMode.MirrorClampToEdge => GLEnum.MirrorClampToEdge,
                _ => throw new UnreachableException()
            };
        }

        private GLEnum ToGLEnum(TextureFilterMode filterMode)
        {
            return filterMode switch
            {
                TextureFilterMode.Linear => GLEnum.Linear,
                TextureFilterMode.Nearest => GLEnum.Nearest,
                _ => throw new UnreachableException()
            };
        }

        private GLEnum ToGLEnum(TextureFilterMode filterMode, MipmapFilterMode mipmapFilterMode)
        {
            return (filterMode, mipmapFilterMode) switch
            {
                (TextureFilterMode.Linear, MipmapFilterMode.Linear) => GLEnum.LinearMipmapLinear,
                (TextureFilterMode.Linear, MipmapFilterMode.Nearest) => GLEnum.LinearMipmapNearest,
                (TextureFilterMode.Nearest, MipmapFilterMode.Nearest) => GLEnum.NearestMipmapNearest,
                (TextureFilterMode.Nearest, MipmapFilterMode.Linear) => GLEnum.NearestMipmapLinear,
                _ => throw new UnreachableException()
            };
        }

        public override string ToString()
        {
            return Handle.ToString();
        }
    }
}
