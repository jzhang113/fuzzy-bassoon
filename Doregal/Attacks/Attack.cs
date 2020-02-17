using Doregal.World;
using System;
using System.Collections.Generic;
using Ultraviolet;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Doregal.Attacks
{
    public class Attack
    {
        public IList<AttackFrame> Sequence { get; }
        public bool Done => Frame == -1 || Frame >= Sequence.Count;
        public AttackFrame CurrFrame => Sequence[Frame];

        private TimeSpan CurrTime { get; set; }
        private int Frame { get; set; }
        private readonly Texture2D _blankTexture;

        public Attack(IList<AttackFrame> sequence)
        {
            Sequence = sequence;
            CurrTime = TimeSpan.Zero;
            Frame = 0;
            _blankTexture = Texture2D.CreateRenderBuffer(1, 1);
            _blankTexture.SetData(new Color[] { Color.White });
        }

        public void Update(TimeSpan dt)
        {
            if (Done) return;

            CurrTime += dt;

            while (!Done && CurrTime >= CurrFrame.Duration)
            {
                CurrTime -= CurrFrame.Duration;
                Frame++;
            }
        }

        internal void DrawHitbox(Camera camera, UltravioletTime time, SpriteBatch spriteBatch)
        {
            if (Done) return;

            foreach (CircleF circ in CurrFrame.Hitbox)
            {
                spriteBatch.Draw(_blankTexture, new RectangleF(circ.Position, new Size2F(circ.Radius, circ.Radius)), Color.Blue);
            }
        }
    }
}
