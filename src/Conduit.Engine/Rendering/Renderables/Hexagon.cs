using Conduit.Engine.Content;
using Conduit.Engine.Graphics.Materials;
using Conduit.Engine.Rendering.Renderables;

namespace Conduit.Engine.Rendering.Shapes
{
    public class Hexagon : IRenderable
    {
        public Mesh Mesh => Mesh.Hexagon;
        public Material? Material { get; }

        public Hexagon(Material? material = null)
        {
            Material = material;
        }
    }
}
