using Conduit.Engine.Content;
using Conduit.Engine.Graphics.Materials;
using Conduit.Engine.Rendering.Renderables;

namespace Conduit.Engine.Rendering.Shapes
{
    public class Circle : IRenderable
    {
        public Mesh Mesh => Mesh.Circle;
        public Material? Material { get; }

        public Circle(Material? material = null)
        {
            Material = material;
        }
    }
}
