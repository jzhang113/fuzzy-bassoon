using System.Text;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Input;
using Ultraviolet.UI;

namespace Doregal.UI.Screens
{
    public class TitleScreen : UIScreen
    {
        private readonly UIScreenService _uiScreenService;
        private readonly SpriteFont _font;
        private readonly Texture2D _blankTexture;
        private readonly TextRenderer _textRenderer;
        private readonly TextLayoutCommandStream _textLayoutCommands;
        private readonly StringBuilder _titleText;

        public TitleScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/SampleScreen1", "SampleScreen1", globalContent)
        {
            Contract.Require(uiScreenService, "uiScreenService");

            IsOpaque = true;

            _uiScreenService = uiScreenService;
            _font = LocalContent.Load<SpriteFont>("SegoeUI");
            _textRenderer = new TextRenderer();
            _textLayoutCommands = new TextLayoutCommandStream();
            _titleText = new StringBuilder();

            var textFormatter = new StringFormatter();
            textFormatter.AddArgument("FFFFFF00");
            textFormatter.Format(
                "Doregal\n" +
                "|c:{0}|(p)|c|lay\n" +
                "|c:{0}|(q)|c|uit", _titleText);

            _blankTexture = Texture2D.CreateTexture(1,1);
            _blankTexture.SetData(new Color[] { Color.White });
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (IsReadyForInput)
            {
                var input = Ultraviolet.GetInput();
                var keyboard = input.GetKeyboard();
                var touch = input.GetPrimaryTouchDevice();

                if (keyboard.IsKeyPressed(Key.P) || (touch != null && touch.WasTapped()))
                {
                    var screen = _uiScreenService.Get<MainScreen>();
                    Screens.Open(screen);
                }
                else if (keyboard.IsKeyPressed(Key.Q))
                {
                    Ultraviolet.Host.Exit();
                }
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_blankTexture, new RectangleF(0, 0, Width, Height), Color.SlateGray);

            var settings = new TextLayoutSettings(_font, Width, Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            _textRenderer.CalculateLayout(_titleText, _textLayoutCommands, settings);
            _textRenderer.Draw(spriteBatch, _textLayoutCommands, Vector2.Zero, Color.White);

            base.OnDrawingForeground(time, spriteBatch);
        }
    }
}