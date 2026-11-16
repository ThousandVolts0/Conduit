using Conduit.Engine.Content;
using Conduit.Engine.Graphics.Data;
using Conduit.Engine.Graphics.Materials;
using Conduit.Engine.Graphics.Shaders;
using Conduit.Engine.Graphics.Textures;
using Conduit.Engine.Rendering;
using Conduit.Engine.Rendering.Viewports;
using Microsoft.Extensions.Logging;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using StbImageSharp;
using System.Drawing;
using System.Reflection.Metadata;

namespace Conduit.Engine.Graphics
{
    public class OpenGLGraphicsDevice : IGraphicsDevice 
    {
        private readonly ILogger<OpenGLGraphicsDevice> _logger;
        private readonly Dictionary<Mesh, GPUMesh> _loadedMeshes = new();
        private GL _gl = null!;

        public OpenGLGraphicsDevice(ILogger<OpenGLGraphicsDevice> logger)
        {
            _logger = logger;
        }

        private GPUMesh LoadMesh(Mesh data)
        {
            uint vao = CreateVAO();
            uint vbo = CreateVBO(data);
            uint ebo = CreateEBO(data);

            // 3 floats for position (x,y,z)
            // 2 for texture coordinates (u, v)
            const uint posLoc = 0;
            _gl.EnableVertexAttribArray(posLoc);
            _gl.VertexAttribPointer(posLoc, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            const uint texCoordLoc = 1;
            _gl.EnableVertexAttribArray(texCoordLoc);
            _gl.VertexAttribPointer(texCoordLoc, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (3 * sizeof(float)));

            _loadedMeshes[data] = new()
            {
                VAO = vao,
                VBO = vbo,
                EBO = ebo,
            };

            return _loadedMeshes[data];
        }

        public void Draw(Mesh mesh, Material material, RenderInfo info)
        {
            _logger.LogDebug("Drawing mesh: {Mesh} with material {Material}", mesh.GetType().Name, material.GetType().Name);

            GPUMesh gpuMesh = _loadedMeshes.ContainsKey(mesh) ? _loadedMeshes[mesh] : LoadMesh(mesh);
            IShader shader = material.Shader;

            shader.Bind();
            shader.SetUniform("u_Projection", info.Projection);
            shader.SetUniform("u_Model", info.Model);
            shader.SetUniform("u_View", info.View);

            material.ApplyProperties();

            _gl.BindVertexArray(gpuMesh.VAO);
            _gl.DrawElements(PrimitiveType.Triangles, (uint)mesh.Indices.Length, DrawElementsType.UnsignedInt, ReadOnlySpan<uint>.Empty);
        }

        public IShader CreateShader(ShaderSource source)
        {
            uint vert = CreateShader(ShaderType.VertexShader, source.Vertex);
            uint frag = CreateShader(ShaderType.FragmentShader, source.Fragment);

            uint handle = _gl.CreateProgram();

            _gl.AttachShader(handle, vert);
            _gl.AttachShader(handle, frag);

            _gl.LinkProgram(handle);
            _gl.GetProgram(handle, ProgramPropertyARB.LinkStatus, out int lStatus);

            if (lStatus != (int)GLEnum.True)
                throw new Exception($"Program failed to link: {_gl.GetProgramInfoLog(handle)}");

            _gl.DetachShader(handle, vert);
            _gl.DetachShader(handle, frag);
            _gl.DeleteShader(vert);
            _gl.DeleteShader(frag);

            return new OpenGLShader(_gl, handle);
        }

        public unsafe ITexture CreateTexture(ImageResult result)
        {
            uint handle = _gl.GenTexture();
            _gl.ActiveTexture(TextureUnit.Texture0);
            _gl.BindTexture(TextureTarget.Texture2D, handle);

            ImageResult flipped = FlipImageVertically(result);
            
            fixed (byte* ptr = flipped.Data)
                _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)result.Width,
                    (uint)result.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);

            _gl.BindTexture(TextureTarget.Texture2D, 0);

            return new OpenGLTexture(_gl, handle);
        }

        private ImageResult FlipImageVertically(ImageResult image)
        {
            byte[] flippedData = new byte[image.Data.Length];

            int bytesPerPixel = (int)image.Comp;
            int bytesPerRow = image.Width * bytesPerPixel;

            for (int row = 0; row < image.Height; row++)
            {
                int sourceRow = image.Height - 1 - row; 

                int flippedRowStart = row * bytesPerRow;
                int sourceRowStart = sourceRow * bytesPerRow;

                for (int i = 0; i < bytesPerRow; i++)
                    flippedData[flippedRowStart + i] = image.Data[sourceRowStart + i];
            }

            image.Data = flippedData;
            return image;
        }

        public void SetViewport(Viewport view)
        {
            _gl.Viewport((int)view.X, (int)view.Y, (uint)view.Width, (uint)view.Height);
        }

        public void Initialize(IWindow window) => _gl = window.CreateOpenGL();
        public void Clear() => _gl.Clear(ClearBufferMask.ColorBufferBit);
        public void SetClearColor(Color color) => _gl.ClearColor(color);

        public void Dispose()
        {
            foreach (GPUMesh mesh in _loadedMeshes.Values)
            {
                _gl.DeleteVertexArray(mesh.VAO);
                _gl.DeleteBuffer(mesh.VBO);
                _gl.DeleteBuffer(mesh.EBO);
            }

            _loadedMeshes.Clear();
        }

        private uint CreateVAO()
        {
            var vao = _gl.GenVertexArray();
            _gl.BindVertexArray(vao);
            return vao;
        }

        private unsafe uint CreateVBO(Mesh mesh)
        {
            var vbo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

            fixed (float* buf = mesh.Vertices)
                _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(mesh.Vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);

            return vbo;
        }

        private unsafe uint CreateEBO(Mesh mesh)
        {
            var ebo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);

            fixed (uint* buf = mesh.Indices)
                _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(mesh.Indices.Length * sizeof(uint)), buf, BufferUsageARB.StaticDraw);

            return ebo; 
        }

        private uint CreateShader(ShaderType type, string source)
        {
            uint shader = _gl.CreateShader(type);
            _gl.ShaderSource(shader, source);
            _gl.CompileShader(shader);
            _gl.GetShader(shader, ShaderParameterName.CompileStatus, out int status);
            if (status != (int)GLEnum.True)
                throw new Exception($"{type} shader failed to compile: " + _gl.GetShaderInfoLog(shader));
            return shader;
        }
    }
}
