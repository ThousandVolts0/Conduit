using Conduit.Engine.Graphics.Materials;

namespace Conduit.Engine.Content.Defaults
{
    public class DefaultMaterials
    {
        public UnlitMaterial Unlit => _unlit.Clone();
        private readonly UnlitMaterial _unlit;
        public DefaultMaterials(UnlitMaterial unlit)
        {
            _unlit = unlit;
        }
    }
}
