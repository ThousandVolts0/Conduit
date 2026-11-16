using Conduit.Engine.Graphics;
using Conduit.Engine.Graphics.Textures;
using Microsoft.Extensions.Logging;
using StbImageSharp;
namespace Conduit.Engine.Content.Loaders
{
    public class TextureResourceLoader : IResourceLoader<ITexture>
    {
        private readonly ILogger<TextureResourceLoader> _logger;
        private readonly IGraphicsDevice _graphicsDevice;

        public TextureResourceLoader(ILogger<TextureResourceLoader> logger, IGraphicsDevice graphicsDevice)
        {
            _logger = logger;
            _graphicsDevice = graphicsDevice;
        }

        public ITexture Load(string path, IResourceProvider provider)
        {
            using (var stream = File.OpenRead(path))
            {
                ImageResult result = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                return _graphicsDevice.CreateTexture(result);
            }
        }
    }
}
