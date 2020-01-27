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

        private bool _attacking = false;
        private TimeSpan _attackDuration = TimeSpan.FromMilliseconds(500);
        private TimeSpan _attackRemaining;
        private RectangleF _hitbox;
        private Texture2D _blankTexture;

        public Entity(SpriteAnimation sprite, float size, Color color)
        {
            SpriteAnimation = sprite;
            Size = size;
            Color = color;

            _blankTexture = Texture2D.CreateRenderBuffer(1, 1);
            _blankTexture.SetData(new Color[] { Color.White });
        }

        internal void Move(Vector2 accel, TimeSpan dt)
        {
            Accel += accel;
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

            // attack
            if (_attacking)
            {
                _attackRemaining -= dt;
                Color = Color.Blue;

                if (_attackRemaining <= TimeSpan.Zero)
                {
                    _attacking = false;
                    Color = Color.Red;
                }
            }
        }

        internal void ResetAccel()
        {
            Accel = Vector2.Zero;
        }

        internal void Attack(Vector2 position)
        {
            if (!_attacking)
            {
                _attacking = true;
                _attackRemaining = _attackDuration;
                _hitbox = new RectangleF(position, new Size2F(50, 10));
            }
        }

        public void Draw(Camera camera, UltravioletTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawSprite(
                SpriteAnimation.Controller,
                camera.ToScreenPos(Position),
                Size * camera.Zoom, Size * camera.Zoom, Color, 0);

            if (_attacking)
            {
                spriteBatch.Draw(_blankTexture, _hitbox, Color.Blue);
            }
        }
    }
}
