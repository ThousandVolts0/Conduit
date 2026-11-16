using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit.Engine.Graphics.Textures
{
    public class OpenGLTexture : ITexture
    {
        public uint Handle { get; }

        public TextureFilterMode FilterMode { get; set; } = TextureFilterMode.Linear;
        public MipmapFilterMode MipmapFilterMode { get; set; } = MipmapFilterMode.Linear;
        public TextureWrapMode WrapU { get; set; } = TextureWrapMode.Repeat;
        public TextureWrapMode WrapV { get; set; } = TextureWrapMode.Repeat;
        public bool HasMipmaps { get; private set; }
        private readonly GL _gl;

        internal OpenGLTexture(GL gl, uint handle)
        {
            _gl = gl;
            Handle = handle;
        }

        public void GenerateMipmaps()
        {
            HasMipmaps = true;

            _gl.BindTexture(GLEnum.Texture2D, Handle);
            _gl.GenerateMipmap(GLEnum.Texture2D);
            _gl.BindTexture(GLEnum.Texture2D, 0);
        }

        public void Dispose()
        {
            _gl.DeleteTexture(Handle);
        }
    }
}
