using System;

using Microsoft.Xna.Framework;

namespace BCS_3.Zak
{
    public class Planet
    {
        public Polygon Polygon { get; private set; }

        public Vector2 Position { get; set; }
        public float Angle { get; set; }

        public Planet(Vector2 position)
        {
            Position = position;

            int pointCount = 30;
            Vector2[] points = new Vector2[pointCount];
            Random random = new Random();

            for (int index = 0; index < pointCount; index++)
            {
                float angle = index * (float)Math.PI * 2 / pointCount;
                float distance = 200 + random.Next(100);

                points[index] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
            }

            Polygon = new Polygon(points);
        }
    }
}
