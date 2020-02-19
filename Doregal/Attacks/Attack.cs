using Doregal.World;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Ultraviolet;
using Ultraviolet.Graphics;

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

        internal void DrawHitbox(Camera camera, UltravioletTime time, SKCanvas canvas)
        {
            if (Done) return;

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(0, 0, 255);

                foreach (CircleF circ in CurrFrame.Hitbox)
                {
                    canvas.DrawCircle(circ.X, circ.Y, circ.Radius, paint);
                }
            }
        }
    }
}
