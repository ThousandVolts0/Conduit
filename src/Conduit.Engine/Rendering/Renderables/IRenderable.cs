using Conduit.Engine.Content;
using Conduit.Engine.Graphics.Materials;

namespace Conduit.Engine.Rendering.Renderables
{
    public interface IRenderable
    {
        public Material? Material { get; }
        public Mesh Mesh { get; }
    }
}
