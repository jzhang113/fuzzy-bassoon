﻿using Ultraviolet.Core;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Input;
using Ultraviolet.UI;
using Doregal.Assets;

namespace Doregal.UI.Screens
{
    public class SampleScreen1 : UIScreen
    {
        public SampleScreen1(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/SampleScreen1", "SampleScreen1", globalContent)
        {
            Contract.Require(uiScreenService, "uiScreenService");

            IsOpaque = true;

            this.uiScreenService = uiScreenService;

            this.font = LocalContent.Load<SpriteFont>("SegoeUI");
            //this.blankTexture = GlobalContent.Load<Texture2D>(GlobalTextureID.Blank);
            this.blankTexture = Texture2D.CreateRenderBuffer(1, 1);
            this.textRenderer = new TextRenderer();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (IsReadyForInput)
            {
                var input = Ultraviolet.GetInput();
                var keyboard = input.GetKeyboard();
                var touch = input.GetPrimaryTouchDevice();

                if (keyboard.IsKeyPressed(Key.Right) || (touch != null && touch.WasTapped()))
                {
                    var screen = uiScreenService.Get<SampleScreen2>();
                    Screens.Open(screen);
                }
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blankTexture, new RectangleF(0, 0, Width, Height), new Color(180, 0, 0));

#if ANDROID || IOS
            var text = "This is SampleScreen1\nTap to open SampleScreen2";
#else
            var text = "This is SampleScreen1\nPress right arrow key to open SampleScreen2";
#endif

            var settings = new TextLayoutSettings(font, Width, Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            textRenderer.Draw(spriteBatch, text, Vector2.Zero, Color.White * TransitionPosition, settings);

            base.OnDrawingForeground(time, spriteBatch);
        }

        private readonly UIScreenService uiScreenService;

        private readonly SpriteFont font;
        private readonly Texture2D blankTexture;
        private readonly TextRenderer textRenderer;
    }
}