using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace BCS_3.Zak
{
    public enum EnemyType{
        Standard,
        SungBrute,
        Queen
    }

    public class GravitarEnemy
    {
        public Vector2 Position { get; set; }
        public float Angle { get; set; }

        public bool Remove { get; set; } = false;

        public EnemyType Type { get; private set; }

        public float firingTimer = 5.0f;
        public float firingDelay = 4.0f;

        private Planet parent;
        private int segmentIndex;
        private float segmentPosition;

        public GravitarEnemy(EnemyType type, Planet parent, int segmentIndex, float segmentPosition)
        {
            Type = type;

            this.parent = parent;
            this.segmentIndex = segmentIndex;
            this.segmentPosition = segmentPosition;
        }

        public void MoveToParent()
        {
            Vector2 firstPoint = parent.Polygon.PointAt(segmentIndex, parent.Angle);
            Vector2 secondPoint = parent.Polygon.PointAt((segmentIndex + 1) % parent.Polygon.Points.Count, parent.Angle);
            Vector2 offset = secondPoint - firstPoint;
            float angle = Utils.AngleOf(offset);

            Position = firstPoint + segmentPosition * offset + Utils.VectorFrom(Utils.AngleOf(offset) + (float)Math.PI / 2, 20);
            Angle = angle;
        }
    }
}
