using System;
using System.Collections.Generic;
using Ultraviolet;

namespace Doregal.Attacks
{
    class AttackFactory
    {
        private IDictionary<string, Func<Point2F, IList<AttackFrame>>> AttackData { get; }

        public AttackFactory()
        {
            AttackData = new Dictionary<string, Func<Point2F, IList<AttackFrame>>>();
        }

        public void Register(string name, Func<Point2F, IList<AttackFrame>> data) => AttackData.Add(name, data);

        public Attack Construct(string name, Point2F position) => new Attack(AttackData[name](position));

        public Attack Construct(string name, Point2F position, double offset, double rotation)
        {
            float dx = (float)(offset * Math.Cos(rotation));
            float dy = (float)(offset * Math.Sin(rotation));

            return new Attack(AttackData[name](new Point2F(position.X + dx, position.Y + dy)));
        }
    }
}
