using Doregal.Assets;
using Doregal.Input;
using Doregal.World;
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
        private readonly int _tileWidth;
        private readonly int _tileHeight;
        private Map _map;
        private bool _firstLoad = true;

        private const int BASE_MAP_WIDTH = 80;
        private const int BASE_MAP_HEIGHT = 60;

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
            _tileWidth = _asciiSprite["Player"].Controller.Width;
            _tileHeight = _asciiSprite["Player"].Controller.Height;

            Opening += (_) =>
            {
                if (_firstLoad)
                {
                    _map = new Map(BASE_MAP_WIDTH, BASE_MAP_HEIGHT);
                    _firstLoad = false;
                }
            };
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (IsReadyForInput)
            {
                MainInput.Actions actions = Ultraviolet.GetInput().GetActions();

                if (actions.MoveLeft.IsDown()) _map.playerX = _map.playerX <= 0 ? 0 : _map.playerX - 5;
                else if (actions.MoveRight.IsDown()) _map.playerX = _map.playerX >= Width - _map.Camera.Zoom ? Width - 16 : _map.playerX + 5;

                if (actions.MoveUp.IsDown()) _map.playerY = _map.playerY <= 0 ? 0 : _map.playerY - 5;
                else if (actions.MoveDown.IsDown()) _map.playerY = _map.playerY >= Height - _map.Camera.Zoom ? Height - 16 : _map.playerY + 5;

                _map.Camera.Update(new Vector2(_map.playerX, _map.playerY));

                if (actions.ExitApplication.IsDown())
                {
                    Screens.Close(this);
                }

                var keyboard = Ultraviolet.GetInput().GetKeyboard();
                if (keyboard.IsKeyPressed(Key.R))
                {
                    _map.Generate();
                }
                if (keyboard.IsKeyPressed(Key.Z)) _map.Camera.Zoom++;
                else if (keyboard.IsKeyPressed(Key.X)) _map.Camera.Zoom--;
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            //Ultraviolet.GetGraphics().Clear(Color.Black);

            // background color
            spriteBatch.Draw(_blankTexture, new RectangleF(0, 0, Width, Height), Color.Black);

            // walls
            for (int x = 0; x <= _map.Camera.MapWidthTiles;  x++)
            {
                for (int y = 0; y <= _map.Camera.MapHeightTiles; y++)
                {
                    int tileX = _map.Camera.TileX + x;
                    int tileY = _map.Camera.TileY + y;

                    float pixelX = _map.Camera.Zoom * x - _map.Camera.OffsetX;
                    float pixelY = _map.Camera.Zoom * y - _map.Camera.OffsetY;

                    if (tileX < 0 || tileY < 0 || tileX >= BASE_MAP_WIDTH || tileY >= BASE_MAP_HEIGHT)
                        continue;

                    bool walk = _map.Field[tileX, tileY];
                    string tile = walk ? "Floor" : "Wall";

                    spriteBatch.DrawSprite(_asciiSprite[tile].Controller, new Vector2(pixelX, pixelY));
                }
            }

            spriteBatch.DrawSprite(_asciiSprite["Player"].Controller, new Vector2(_map.playerX, _map.playerY), null, null, Color.Red, 0);

            base.OnDrawingForeground(time, spriteBatch);
        }
    }
}