using Doregal.Assets;
using Doregal.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
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
        private ContentManager _content;
        private TextRenderer _textRenderer;
        private TextLayoutCommandStream _textLayoutCommands;
        private FreeTypeFont _firaFont;
        private Sprite _asciiSprite;
        private SpriteBatch _spriteBatch;
        private int _x;
        private int _y;

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

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var config = new OpenGLUltravioletConfiguration();
            config.Plugins.Add(new FreeTypeFontPlugin());

            return new OpenGLUltravioletContext(this, config);
        }

        protected override void OnLoadingContent()
        {
            LoadInputBindings();

            _content = ContentManager.Create("Content");
            LoadContentManifests();
            LoadLocalizationDatabases();

            _textRenderer = new TextRenderer();
            _textLayoutCommands = new TextLayoutCommandStream();
            _firaFont = _content.Load<FreeTypeFont>(GlobalFontID.FiraSans);

            _textRenderer.RegisterFont("fira", _firaFont);
            _textRenderer.RegisterStyle("preset1", new TextStyle(_firaFont, true, null, Color.Lime));
            _textRenderer.RegisterStyle("preset2", new TextStyle(_firaFont, null, true, Color.Red));

            _spriteBatch = SpriteBatch.Create();
            _asciiSprite = _content.Load<Sprite>(GlobalSpriteID.Ascii);

            _x = 0;
            _y = 0;
            
            base.OnLoadingContent();
        }

        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            // _asciiSprite.Update(time);

            SampleInput.Actions actions = Ultraviolet.GetInput().GetActions();

            if (actions.MoveLeft.IsDown()) _x -= 10;
            else if (actions.MoveRight.IsDown()) _x += 10;

            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var width = window.DrawableSize.Width;
            var height = window.DrawableSize.Height;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            _spriteBatch.DrawSprite(_asciiSprite["Player"].Controller, new Vector2(_x, _y), null, null, Color.Red, 0);


            var settingsTopLeft = new TextLayoutSettings(_firaFont, width, height, TextFlags.AlignTop | TextFlags.AlignLeft);
            _textRenderer.Draw(_spriteBatch, "Aligned top left", Vector2.Zero, Color.White, settingsTopLeft);

            const string text =
    "Ultraviolet Formatting Commands\n" +
    "\n" +
    "||c:AARRGGBB| - Changes the color of text.\n" +
    "|c:FFFF0000|red|c| |c:FFFF8000|orange|c| |c:FFFFFF00|yellow|c| |c:FF00FF00|green|c| |c:FF0000FF|blue|c| |c:FF6F00FF|indigo|c| |c:FFFF00FF|magenta|c|\n" +
    "\n" +
    "||font:name| - Changes the current font.\n" +
    "We can |font:fira|transition to a completely different font|font| within a single line\n" +
    "\n" +
    "||b| and ||i| - Changes the current font style.\n" +
    "|b|bold|b| |i|italic|i| |b||i|bold italic|i||b|\n" +
    "\n" +
    "||style:name| - Changes to a preset style.\n" +
    "|style:preset1|this is preset1|style| |style:preset2|this is preset2|style|\n" +
    "\n" +
    "||icon:name| - Draws an icon in the text.\n" +
    "[|icon:ok| OK] [|icon:cancel| Cancel]";

            var settings = new TextLayoutSettings(_firaFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
            _textRenderer.CalculateLayout(text, _textLayoutCommands, settings);
            _textRenderer.Draw(_spriteBatch, _textLayoutCommands, Vector2.Zero, Color.White);

            _spriteBatch.End();

            base.OnDrawing(time);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(_spriteBatch);
                SafeDispose.Dispose(_content);
            }
            base.Dispose(disposing);
        }

        protected void LoadLocalizationDatabases()
        {
            var fss = FileSystemService.Create();
            IEnumerable<string> databases = _content.GetAssetFilePathsInDirectory("Localization", "*.xml");
            foreach (string database in databases)
            {
                using (Stream stream = fss.OpenRead(database))
                {
                    Localization.Strings.LoadFromStream(stream);
                }
            }
        }

        protected void LoadContentManifests()
        {
            IUltravioletContent uvContent = Ultraviolet.GetContent();

            IEnumerable<string> contentManifestFiles = _content.GetAssetFilePathsInDirectory("Manifests");
            uvContent.Manifests.Load(contentManifestFiles);

            uvContent.Manifests["Global"]["FreeTypeFonts"].PopulateAssetLibrary(typeof(GlobalFontID));
            uvContent.Manifests["Global"]["Sprites"].PopulateAssetLibrary(typeof(GlobalSpriteID));
        }

        private string GetInputBindingsPath() => Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");

        private void LoadInputBindings() => Ultraviolet.GetInput().GetActions().Load(GetInputBindingsPath(), throwIfNotFound: false);

        private void SaveInputBindings() => Ultraviolet.GetInput().GetActions().Save(GetInputBindingsPath());
    }
}
