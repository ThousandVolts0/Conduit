using Conduit.Engine.Content;
using Conduit.Engine.Graphics.Shaders;
using Conduit.Engine.Graphics.Textures;
using System.Numerics;

namespace Conduit.Engine.Graphics.Materials
{
    public class UnlitMaterial : Material
    {
        public UnlitMaterial(IShader shader) : base(shader) { }
        public UnlitMaterial(IShader shader, Dictionary<string, object> properties) : base(shader, properties) { }

        public override UnlitMaterial Clone() => new UnlitMaterial(Shader, _properties);

        public Vector4 Color
        {
            get => TryGetProperty("u_Color", out Vector4 color) ? color : default;
            set => SetProperty("u_Color", value);
        }

        public ITexture Texture
        {
            get => TryGetProperty("u_Texture", out ITexture texture) ? texture : default;
            set => SetProperty("u_Texture", value);
        }
    }
}
