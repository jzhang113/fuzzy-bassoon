using Doregal.Assets;
using Doregal.Input;
using Doregal.UI;
using Doregal.UI.Screens;
using System;
using System.Collections.Generic;
using System.IO;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.FreeType2;
using Ultraviolet.OpenGL;
using Ultraviolet.Platform;

namespace Doregal
{
#if ANDROID
    [Android.App.Activity(Label = "Doregal", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public partial class Game : UltravioletActivity
#else
    public partial class Game : UltravioletApplication
#endif
    {
        public Game() : base("Ultraviolet", "Doregal")
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

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var config = new OpenGLUltravioletConfiguration();
            config.Plugins.Add(new FreeTypeFontPlugin());

            return new OpenGLUltravioletContext(this, config);
        }

        protected override void OnLoadingContent()
        {
            LoadInputBindings();

            var content = ContentManager.Create("Content");
            LoadContentManifests(content);
            LoadLocalizationDatabases(content);

            var screenService = new UIScreenService(content);
            var screen = screenService.Get<TitleScreen>();
            Ultraviolet.GetUI().GetScreens().Open(screen, TimeSpan.Zero);

            base.OnLoadingContent();
        }

        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        protected void LoadLocalizationDatabases(ContentManager content)
        {
            Contract.Require(content, nameof(content));

            var fss = FileSystemService.Create();
            IEnumerable<string> databases = content.GetAssetFilePathsInDirectory("Localization", "*.xml");
            foreach (string database in databases)
            {
                using (Stream stream = fss.OpenRead(database))
                {
                    Localization.Strings.LoadFromStream(stream);
                }
            }
        }

        protected void LoadContentManifests(ContentManager content)
        {
            Contract.Require(content, nameof(content));

            IUltravioletContent uvContent = Ultraviolet.GetContent();
            IEnumerable<string> contentManifestFiles = content.GetAssetFilePathsInDirectory("Manifests");
            uvContent.Manifests.Load(contentManifestFiles);

            uvContent.Manifests["Global"]["FreeTypeFonts"].PopulateAssetLibrary(typeof(GlobalFontID));
            uvContent.Manifests["Global"]["Sprites"].PopulateAssetLibrary(typeof(GlobalSpriteID));
        }

        private string GetInputBindingsPath() => Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");

        private void LoadInputBindings() => Ultraviolet.GetInput().GetActions().Load(GetInputBindingsPath(), throwIfNotFound: false);

        private void SaveInputBindings() => Ultraviolet.GetInput().GetActions().Save(GetInputBindingsPath());
    }
}
