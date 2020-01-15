using System;
using System.Collections.Generic;
using System.Text;

namespace Doregal.World
{
    public class Map
    {
        public int Width { get; }
        public int Height { get; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Generate()
        {
            // TODO map gen
        }
    }
}
