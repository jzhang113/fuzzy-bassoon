using Doregal.Attacks;
using Doregal.World;
using System;
using Ultraviolet;
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

        private readonly AttackFactory _attackFactory;
        private Attack _currentAttack;

        public Entity(SpriteAnimation sprite, float size, Color color)
        {
            SpriteAnimation = sprite;
            Size = size;
            Color = color;

            _attackFactory = new AttackFactory();
            _attackFactory.Register("basic", pos => new AttackFrame[]
            {
                new AttackFrame(null, TimeSpan.FromMilliseconds(100), new CircleF[] { new CircleF(pos, 10) }, null),
                new AttackFrame(null, TimeSpan.FromMilliseconds(100), new CircleF[] { new CircleF(pos.X, pos.Y + 10, 10) }, null),
                new AttackFrame(null, TimeSpan.FromMilliseconds(100), new CircleF[] { new CircleF(pos.X, pos.Y + 20, 10) }, null),
                new AttackFrame(null, TimeSpan.FromMilliseconds(100), new CircleF[] { new CircleF(pos.X, pos.Y + 30, 10) }, null),
            });
        }

        internal void Move(Vector2 accel, TimeSpan dt)
        {
            if (_currentAttack == null || _currentAttack.Done)
            {
                Accel += accel;
            }
        }

        internal Attack Attack(Point2F position)
        {
            if (_currentAttack == null || _currentAttack.Done)
            {
                _currentAttack = _attackFactory.Construct("basic", position);
                return _currentAttack;
            }
            else
            {
                if (_currentAttack.Done)
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
