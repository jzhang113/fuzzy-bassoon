using Doregal.Assets;
using Doregal.Input;
using System;
using System.IO;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.OpenGL;
using Ultraviolet.Platform;

namespace Doregal
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 2 - Handling Input", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public partial class Game : UltravioletActivity
#else
    public partial class Game : UltravioletApplication
#endif
    {
        private ContentManager content;
        private Sprite sprite;
        private SpriteAnimationController controller1;
        private SpriteAnimationController controller2;
        private SpriteAnimationController controller3;
        private SpriteAnimationController controller4;
        private SpriteBatch spriteBatch;

        public Game()
            : base("Ultraviolet", "Sample 2 - Handling Input")
        {
            PlatformSpecificInitialization();
        }

        partial void PlatformSpecificInitialization();

        public static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }

        protected override UltravioletContext OnCreatingUltravioletContext() => new OpenGLUltravioletContext(this);

        protected override void OnLoadingContent()
        {
            LoadInputBindings();

            this.content = ContentManager.Create("Content");
            LoadContentManifests(this.content);
            LoadLocalizationDatabases();

            this.sprite = this.content.Load<Sprite>(GlobalSpriteID.Explosion);
            this.spriteBatch = SpriteBatch.Create();

            this.controller1 = new SpriteAnimationController();
            this.controller2 = new SpriteAnimationController();
            this.controller3 = new SpriteAnimationController();
            this.controller4 = new SpriteAnimationController();

            base.OnLoadingContent();
        }

        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            this.sprite.Update(time);

            if (time.TotalTime.TotalMilliseconds > 250 && !controller1.IsPlaying)
            {
                controller1.PlayAnimation(sprite["Explosion"]);
            }
            if (time.TotalTime.TotalMilliseconds > 500 && !controller2.IsPlaying)
            {
                controller2.PlayAnimation(sprite["Explosion"]);
            }
            if (time.TotalTime.TotalMilliseconds > 750 && !controller3.IsPlaying)
            {
                controller3.PlayAnimation(sprite["Explosion"]);
            }
            if (time.TotalTime.TotalMilliseconds > 1000 && !controller4.IsPlaying)
            {
                controller4.PlayAnimation(sprite["Explosion"]);
            }

            controller1.Update(time);
            controller2.Update(time);
            controller3.Update(time);
            controller4.Update(time);

            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            spriteBatch.DrawSprite(this.sprite["Explosion"].Controller, new Vector2(32, 32));
            spriteBatch.DrawSprite(controller1, new Vector2(132, 32));
            spriteBatch.DrawSprite(controller2, new Vector2(232, 32));
            spriteBatch.DrawSprite(controller3, new Vector2(332, 32));
            spriteBatch.DrawSprite(controller4, new Vector2(432, 32));

            spriteBatch.End();

            base.OnDrawing(time);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(spriteBatch);
                SafeDispose.Dispose(content);
            }
            base.Dispose(disposing);
        }

        protected void LoadLocalizationDatabases()
        {
            var fss = FileSystemService.Create();
            var databases = content.GetAssetFilePathsInDirectory("Localization", "*.xml");
            foreach (var database in databases)
            {
                using (var stream = fss.OpenRead(database))
                {
                    Localization.Strings.LoadFromStream(stream);
                }
            }
        }

        protected void LoadContentManifests(ContentManager content)
        {
            var uvContent = Ultraviolet.GetContent();

            var contentManifestFiles = this.content.GetAssetFilePathsInDirectory("Manifests");
            uvContent.Manifests.Load(contentManifestFiles);

            // uvContent.Manifests["Global"]["Fonts"].PopulateAssetLibrary(typeof(GlobalFontID));
            uvContent.Manifests["Global"]["Sprites"].PopulateAssetLibrary(typeof(GlobalSpriteID));
        }

        private string GetInputBindingsPath() => Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");

        private void LoadInputBindings() => Ultraviolet.GetInput().GetActions().Load(GetInputBindingsPath(), throwIfNotFound: false);

        private void SaveInputBindings() => Ultraviolet.GetInput().GetActions().Save(GetInputBindingsPath());
    }
}
