using Doregal.Assets;
using Doregal.Input;
using System;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Input;
using Ultraviolet.UI;

namespace Doregal.UI.Screens
{
    public class MainScreen : UIScreen
    {
        private readonly SpriteFont _font;
        private readonly Sprite _asciiSprite;
        private readonly Texture2D _blankTexture;
        private readonly TextRenderer _textRenderer;
        private int _x;
        private int _y;

        public MainScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/SampleScreen2", "SampleScreen2", globalContent)
        {
            Contract.Require(uiScreenService, "uiScreenService");

            IsOpaque = true;
            DefaultOpenTransitionDuration = TimeSpan.Zero;
            DefaultCloseTransitionDuration = TimeSpan.Zero;

            _font = LocalContent.Load<SpriteFont>("Garamond");
            _blankTexture = Texture2D.CreateRenderBuffer(40, 40);
            _textRenderer = new TextRenderer();
            _asciiSprite = GlobalContent.Load<Sprite>(GlobalSpriteID.Ascii);
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (IsReadyForInput)
            {
                MainInput.Actions actions = Ultraviolet.GetInput().GetActions();

                if (actions.MoveLeft.IsDown()) _x -= 10;
                else if (actions.MoveRight.IsDown()) _x += 10;

                if (actions.MoveUp.IsDown()) _y -= 10;
                else if (actions.MoveDown.IsDown()) _y += 10;

                var input = Ultraviolet.GetInput();
                var keyboard = input.GetKeyboard();
                var touch = input.GetPrimaryTouchDevice();

                if (keyboard.IsKeyPressed(Key.Left) || (touch != null && touch.WasTapped()))
                {
                    Screens.Close(this);
                }
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            var offset = GetScreenOffset();
            spriteBatch.Draw(_blankTexture, new RectangleF(offset, 0, Width, Height), new Color(0, 0, 180));

#if ANDROID || IOS 
            var text = "This is SampleScreen2\nTap to open SampleScreen1";
#else
            var text = "This is SampleScreen2\nPress left arrow key to open SampleScreen1";
#endif

            var settings = new TextLayoutSettings(_font, Width, Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            _textRenderer.Draw(spriteBatch, text, new Vector2(offset, 0), Color.White, settings);

            spriteBatch.DrawSprite(_asciiSprite["Player"].Controller, new Vector2(_x, _y), null, null, Color.Red, 0);

            base.OnDrawingForeground(time, spriteBatch);
        }

        private Int32 GetScreenOffset()
        {
            if (State == UIPanelState.Opening)
            {
                return Width - (Int32)(Width * Easings.EaseInOutSin(TransitionPosition));
            }
            if (State == UIPanelState.Closing)
            {
                return (Int32)(Width * Easings.EaseInSin(1.0f - TransitionPosition));
            }
            return 0;
        }
    }
}