using Conduit.Engine.Content;
using Conduit.Engine.Graphics.Materials;
using Conduit.Engine.Graphics.Shaders;
using Conduit.Engine.Graphics.Textures;
using Conduit.Engine.Rendering;
using Conduit.Engine.Rendering.Viewports;
using Silk.NET.Windowing;
using StbImageSharp;
using System.Drawing;

namespace Conduit.Engine.Graphics
{
    public interface IGraphicsDevice : IDisposable
    {
        void SetClearColor(Color color);
        void Clear();
        void Initialize(IWindow window);
        void SetViewport(Viewport view);
        IShader CreateShader(ShaderSource source);
        ITexture CreateTexture(ImageResult result);

        void Draw(Mesh mesh, Material material, RenderInfo info);
    }
}
