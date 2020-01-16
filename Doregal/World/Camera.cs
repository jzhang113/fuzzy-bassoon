using System;
using System.Collections.Generic;
using Ultraviolet;

namespace Doregal.World
{
    // current position of the camera, in pixels
    public class Camera
    {
        // positions in pixels
        public float MapWidth => MapWidthTiles * Zoom;
        public float MapHeight => MapHeightTiles * Zoom;
        public float ScreenWidth { get; }
        public float ScreenHeight { get; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public int Zoom { get; set; }

        // positions in tiles
        public int MapWidthTiles { get; }
        public int MapHeightTiles { get; }
        public int TileX => (int)(X / Zoom);
        public int TileY => (int)(Y / Zoom);

        // partial camera position
        public float OffsetX => X - TileX * Zoom;
        public float OffsetY => Y - TileY * Zoom;

        public Camera(int mapWidth, int mapHeight, float screenWidth, float screenHeight, int zoom)
        {
            MapWidthTiles = mapWidth;
            MapHeightTiles = mapHeight;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            Zoom = zoom;
        }

        internal void Update(in Vector2 center)
        {
            // set left and top limits for the camera
            float startX = Math.Max(center.X - ScreenWidth / 2, 0);
            float startY = Math.Max(center.Y - ScreenHeight / 2, 0);

            // set right and bottom limits for the camera
            float xDiff = MapWidth - ScreenWidth;
            float yDiff = MapHeight - ScreenHeight;
            X = xDiff < 0 ? 0 : Math.Min(xDiff, startX);
            Y = yDiff < 0 ? 0 : Math.Min(yDiff, startY);
        }

        internal bool OnScreen(in Vector2 pos)
        {
            float x = pos.X;
            float y = pos.Y;

            return x >= X && x < X + ScreenWidth && y >= Y && y < Y + ScreenHeight;
        }
    }
}
