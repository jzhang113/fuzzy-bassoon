using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;

namespace Doregal.World
{
    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        public bool[,] Field { get; }
        public Camera Camera { get; }

        public float playerX = 0;
        public float playerY = 0;

        private NoiseMap _noise;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Field = new bool[Width, Height];
            Camera = new Camera(width, height, 640, 480, 16);

            Generate();
        }

        public void Generate()
        {
            _noise = new NoiseMap();
            var rnd = new System.Random();
            var noiseSource = new Perlin() { Seed = rnd.Next() };
            var noiseMapBuilder = new PlaneNoiseMapBuilder
            {
                DestNoiseMap = _noise,
                SourceModule = noiseSource,
            };

            noiseMapBuilder.SetDestSize(Width, Height);
            noiseMapBuilder.SetBounds(0, 10, 0, 10);
            noiseMapBuilder.Build();

            // TODO: placeholder generation rn
            int x = 0;
            int y = 0;
            foreach (var line in _noise.IterateAllLines())
            {
                foreach (float val in line)
                {
                    if (val > 0.3) Field[x, y] = true;

                    x++;
                }

                x = 0;
                y++;
            }
        }
    }
}
