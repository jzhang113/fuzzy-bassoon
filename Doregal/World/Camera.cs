using System;
using System.Collections.Generic;
using Ultraviolet;

namespace Doregal.World
{
    // current position of the camera, in pixels
    public class Camera
    {
        public float MapWidth { get; }
        public float MapHeight { get; }
        public float ScreenWidth { get; }
        public float ScreenHeight { get; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public int Zoom { get; set; }

        public Camera(float mapWidth, float mapHeight, float screenWidth, float screenHeight, int zoom)
        {
            MapWidth = mapWidth * zoom;
            MapHeight = mapHeight * zoom;
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
