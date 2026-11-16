using Conduit.Engine.Components;
using Conduit.Engine.Core;
using Conduit.Engine.Core.Game;
using Conduit.Engine.Core.Game.Abstractions;
using Conduit.Engine.Graphics.Textures;
using Conduit.Engine.Rendering.Shapes;
using Microsoft.Extensions.Logging;
using System.Numerics;
using Vector2 = Conduit.Engine.Mathematics.Vector2;

namespace Conduit.Samples.BasicGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new GameBuilder()
                .Configure((settings) =>
                {
                    settings.Window.Width = 1000;
                    settings.Window.Height = 600;
                    settings.Window.Title = "Sample game";

                    settings.Logging.MinimumLevel = LogLevel.Debug;
                    settings.Logging.UseConsole = true;
                })
                .AddScene<MainScene>("Main")
                .SetActiveScene("Main")
                .Build();

            game.Run();
        }
    }

    public class MainScene : Scene
    {
        protected override void OnInitialize(GameContext ctx)
        {
            // todo:
            // add source generator that generates csharp material class from .mat files using their .shader reference
            // add more tests, and improve the ones that exists, use Theory more
            // also return that generated class from resource loader, as it self, not the broad Material instance, so u can access its individual properties
            // make unlit material accept plain color and not require texture, maybe just pass in flat white texture as default

            // add gl sampler objects to allow easier and more modular configuration of sampler settings for textures
            // simplify shapes that inherit from IRenderable (Quad, Circle, etc) to make it more centralized maybe Shape.Quad or smth
            // somehow enforce some shader contract that graphics device use to set model/view/proj matrices, cuz rn it just looks at predefined strings and hope its there

            // centralize game systems/services, maybe GameRuntime or smth
            // change game builder, rn Build() does wayy too much. maybe add seperate GameFactory
            // add behaviours that run methods marked with specific attributes for core lifecycle events such as OnUpdate and OnStart
            // add more xml documentation to classes

            var material = ctx.Resources.Defaults.Materials.Unlit;
            material.Color = new Vector4(1f, 1f, 1f, 1f);
            material.Texture = ctx.Resources.Load<ITexture>("test.jpg");
            var playerShape = new Quad(material);

            Vector2 pos = new(0f, 0f);
            Vector2 scale = new(5f, 5f);
            float rot = 0f;

            Entity mainCam = new Entity("MainCamera")
                .Attach(new Camera());

            Entity player = new Entity("Player")
                .Attach(new Transform(pos, scale, rot))
                .Attach(new Renderer(playerShape));

            AddEntity(player);
            AddEntity(mainCam);
        }
    }
}
