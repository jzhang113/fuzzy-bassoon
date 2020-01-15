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
            _blankTexture = Texture2D.CreateRenderBuffer(1, 1);
            _blankTexture.SetData(new Color[] { Color.White });
            _textRenderer = new TextRenderer();
            _asciiSprite = GlobalContent.Load<Sprite>(GlobalSpriteID.Ascii);
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (IsReadyForInput)
            {
                MainInput.Actions actions = Ultraviolet.GetInput().GetActions();

                if (actions.MoveLeft.IsDown()) _x = _x <= 0 ? 0 : _x - 10;
                else if (actions.MoveRight.IsDown()) _x = _x >= Width - 16 ? Width - 16 : _x + 10;

                if (actions.MoveUp.IsDown()) _y = _y <= 0 ? 0 : _y - 10;
                else if (actions.MoveDown.IsDown()) _y = _y >= Height - 16 ? Height - 16 : _y + 10;

                if (actions.ExitApplication.IsDown())
                {
                    Screens.Close(this);
                }
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_blankTexture, new RectangleF(0, 0, Width, Height), Color.SlateGray);

            spriteBatch.DrawSprite(_asciiSprite["Player"].Controller, new Vector2(_x, _y), null, null, Color.Red, 0);

            base.OnDrawingForeground(time, spriteBatch);
        }
    }
}