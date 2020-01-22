using System;
using Ultraviolet;
using Ultraviolet.Graphics.Graphics2D;

namespace Doregal.Entities
{
    public class Entity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Accel { get; set; }
        public SpriteAnimation SpriteAnimation { get; }

        // exposing constants for ImGui
        internal float Friction = 0.2f;
        internal Vector2 MaxVelocity = new Vector2(1, 1);

        public Entity(SpriteAnimation sprite)
        {
            Position = new Vector2();
            SpriteAnimation = sprite;
        }

        internal void Move(Vector2 accel, TimeSpan dt)
        {
            Accel += accel;
        }

        internal void RealMove(TimeSpan dt)
        {
            Velocity = Vector2.Clamp((1 - Friction) * Velocity + Accel, -MaxVelocity, MaxVelocity);
            Vector2 newPos = Position + Velocity;

            Position = Vector2.Clamp(newPos, Vector2.Zero, new Vector2(79, 59));
        }

        internal void ResetAccel()
        {
            Accel = Vector2.Zero;
        }
    }
}
