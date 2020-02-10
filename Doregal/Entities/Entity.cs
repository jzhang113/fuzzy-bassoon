using Doregal.Attacks;
using Doregal.World;
using System;
using Ultraviolet;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Doregal.Entities
{
    public class Entity
    {
        public SpriteAnimation SpriteAnimation { get; }
        public float Size { get; }
        public Color Color { get; private set; }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Accel { get; set; }

        // exposing constants for ImGui
        internal float Friction = 0.2f;
        internal Vector2 MaxVelocity = new Vector2(1, 1);

        private Attack _attackFactory;
        private Attack _currentAttack;

        public Entity(SpriteAnimation sprite, float size, Color color)
        {
            SpriteAnimation = sprite;
            Size = size;
            Color = color;

            _attackFactory = new Attack(TimeSpan.FromMilliseconds(500), 30, (pos, dt) => new Point2F(pos.X + 1, pos.Y + 1));
        }

        internal void Move(Vector2 accel, TimeSpan dt)
        {
            if (_currentAttack == null || !_currentAttack.Attacking)
            {
                Accel += accel;
            }
        }

        internal Attack Attack(Vector2 position)
        {
            if (_currentAttack == null || !_currentAttack.Attacking)
            {
                // choose attack
                _currentAttack = _attackFactory.Begin(position);
                return _currentAttack;
            }
            else
            {
                if (!_currentAttack.Attacking)
                    _currentAttack = null;

                return null;
            }
        }

        internal void Update(TimeSpan dt)
        {
            // movement
            Velocity = Vector2.Clamp((1 - Friction) * Velocity + Accel, -MaxVelocity, MaxVelocity);
            Vector2 newPos = Position + Velocity;

            const float MAP_WIDTH = 80;
            const float MAP_HEIGHT = 60;
            var bounds = new Vector2(MAP_WIDTH - Size, MAP_HEIGHT - Size);

            Position = Vector2.Clamp(newPos, Vector2.Zero, bounds);
            Accel = Vector2.Zero;
        }

        public void Draw(Camera camera, UltravioletTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawSprite(
                SpriteAnimation.Controller,
                camera.ToScreenPos(Position),
                Size * camera.Zoom, Size * camera.Zoom, Color, 0);
        }
    }
}
