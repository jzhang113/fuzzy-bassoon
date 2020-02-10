using Doregal.World;
using System;
using Ultraviolet;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Doregal.Attacks
{
    public class Attack
    {
        public bool Attacking { get; set; }
        public CircleF Hitbox => new CircleF(_attackPos, _size);

        private readonly TimeSpan _attackDuration;
        private readonly float _size;
        private readonly Func<Point2F, TimeSpan, Point2F> _updatePos;

        private TimeSpan _attackRemaining;
        private readonly Texture2D _blankTexture;
        private Point2F _attackPos;

        public Attack(TimeSpan attackDuration, float size, Func<Point2F, TimeSpan, Point2F> path)
        {
            _attackDuration = attackDuration;
            _size = size;
            _updatePos = path;

            _blankTexture = Texture2D.CreateRenderBuffer(1, 1);
            _blankTexture.SetData(new Color[] { Color.White });

            Attacking = false;
        }

        public Attack Begin(Vector2 position)
        {
            return new Attack(_attackDuration, _size, _updatePos)
            {
                Attacking = true,
                _attackRemaining = _attackDuration,
                _attackPos = (Point2F)position
            };
        }

        public void Update(TimeSpan dt)
        {
            if (Attacking)
            {
                _attackRemaining -= dt;
                _attackPos = _updatePos(_attackPos, dt);

                if (_attackRemaining <= TimeSpan.Zero)
                {
                    Attacking = false;
                }
            }
        }

        internal void DrawHitbox(Camera camera, UltravioletTime time, SpriteBatch spriteBatch)
        {
            if (Attacking)
            {
                spriteBatch.Draw(_blankTexture, new RectangleF(_attackPos, new Size2F(_size, _size)), Color.Blue);
            }
        }
    }
}
