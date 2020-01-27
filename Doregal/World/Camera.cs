using System;
using Ultraviolet;

namespace Doregal.World
{
    // current position of the camera, in pixels
    public class Camera : UltravioletResource
    {
        // positions in pixels
        public float MapWidth => MapWidthTiles * Zoom;
        public float MapHeight => MapHeightTiles * Zoom;
        public float ScreenWidth { get; }
        public float ScreenHeight { get; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Zoom { get; set; }

        // positions in tiles
        public int MapWidthTiles { get; }
        public int MapHeightTiles { get; }
        public int ScreenWidthTiles => (int)(ScreenWidth / Zoom);
        public int ScreenHeightTiles => (int)(ScreenHeight / Zoom);
        public int TileX => (int)(X / Zoom);
        public int TileY => (int)(Y / Zoom);

        // partial camera position
        public float OffsetX => X - TileX * Zoom;
        public float OffsetY => Y - TileY * Zoom;

        public Camera(UltravioletContext uv, int mapWidth, int mapHeight, float zoom) : base(uv)
        {
            var view = Ultraviolet.GetGraphics().GetViewport();

            MapWidthTiles = mapWidth;
            MapHeightTiles = mapHeight;
            ScreenWidth = view.Width;
            ScreenHeight = view.Height;
            Zoom = zoom;
        }

        internal void Update(in Vector2 center)
        {
            // set left and top limits for the camera
            float startX = Math.Max(center.X * Zoom - ScreenWidth / 2, 0);
            float startY = Math.Max(center.Y * Zoom - ScreenHeight / 2, 0);

            // set right and bottom limits for the camera
            float xDiff = MapWidth - ScreenWidth;
            float yDiff = MapHeight - ScreenHeight;

            X = xDiff < 0 ? 0 : Math.Min(xDiff, startX);
            Y = yDiff < 0 ? 0 : Math.Min(yDiff, startY);
        }

        internal bool OnMap(in Vector2 mapPos)
        {
            bool xOnMap = mapPos.X >= TileX && mapPos.X < X + ScreenWidthTiles;
            bool yOnMap = mapPos.Y >= TileY && mapPos.Y < Y + ScreenHeightTiles;
            return xOnMap && yOnMap;
        }

        internal Vector2 ToScreenPos(in Vector2 mapPos) => (mapPos - new Vector2(TileX, TileY)) * Zoom - new Vector2(OffsetX, OffsetY);
    }
}
