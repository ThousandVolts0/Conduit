using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit.Engine.Graphics.Textures
{
    public interface ITexture : IDisposable
    {
        uint Handle { get; }

        TextureFilterMode FilterMode { get; set; }
        MipmapFilterMode MipmapFilterMode { get; set; }
        TextureWrapMode WrapU { get; set; }
        TextureWrapMode WrapV { get; set; }
        bool HasMipmaps { get; }
        void GenerateMipmaps();
    }

    public enum TextureFilterMode
    {
        Nearest,
        Linear
    }

    public enum MipmapFilterMode
    {
        Nearest,
        Linear
    }

    public enum TextureWrapMode
    {
        Repeat,
        MirrorRepeat,
        ClampToEdge,
        MirrorClampToEdge
    }
}
