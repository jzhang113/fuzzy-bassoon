using System;
using System.Collections.Generic;
using Ultraviolet;
using Ultraviolet.Graphics.Graphics2D;

namespace Doregal.Attacks
{
    public struct AttackFrame
    {
        public SpriteFrame Sprite { get; }
        public TimeSpan Duration { get; }
        public ICollection<CircleF> Hitbox { get; }
        public ICollection<CircleF> Hurtbox { get; }

        public AttackFrame(SpriteFrame sprite, TimeSpan duration, ICollection<CircleF> hitbox, ICollection<CircleF> hurtbox)
        {
            Sprite = sprite;
            Duration = duration;
            Hitbox = hitbox;
            Hurtbox = hurtbox;
        }
    }
}
