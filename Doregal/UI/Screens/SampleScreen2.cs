﻿using System;
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
    public class SampleScreen2 : UIScreen
    {
        public SampleScreen2(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/SampleScreen2", "SampleScreen2", globalContent)
        {
            Contract.Require(uiScreenService, "uiScreenService");

            IsOpaque = true;

            this.font = LocalContent.Load<SpriteFont>("Garamond");
            this.blankTexture = Texture2D.CreateRenderBuffer(40, 40);
            this.textRenderer = new TextRenderer();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (IsReadyForInput)
            {
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
            spriteBatch.Draw(blankTexture, new RectangleF(offset, 0, Width, Height), new Color(0, 0, 180));

#if ANDROID || IOS 
            var text = "This is SampleScreen2\nTap to open SampleScreen1";
#else
            var text = "This is SampleScreen2\nPress left arrow key to open SampleScreen1";
#endif

            var settings = new TextLayoutSettings(font, Width, Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            textRenderer.Draw(spriteBatch, text, new Vector2(offset, 0), Color.White, settings);

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

        private readonly SpriteFont font;
        private readonly Texture2D blankTexture;
        private readonly TextRenderer textRenderer;
    }
}