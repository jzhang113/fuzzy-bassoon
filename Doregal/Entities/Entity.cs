using System;
using Ultraviolet;
using Ultraviolet.Graphics.Graphics2D;

namespace Doregal.Entities
{
    public class Entity
    {
        public Vector2 Position { get; set; }
        public SpriteAnimation SpriteAnimation { get; }

        public Entity(SpriteAnimation sprite)
        {
            Position = new Vector2();
            SpriteAnimation = sprite;
        }

        internal void Move(Vector2 direction)
        {
            Vector2 newPos = Position + direction;

            Position = Vector2.Max(Vector2.Zero, Vector2.Min(newPos, new Vector2(80, 60)));
        }
    }
}
