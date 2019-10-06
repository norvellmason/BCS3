using System;

using Microsoft.Xna.Framework;

namespace BCS_3.Zak
{
    public class Planet
    {
        public Polygon Polygon { get; private set; }

        public Vector2 Position { get; set; }
        public float Angle { get; set; }

        public Planet(Vector2 position, Polygon polygon)
        {
            Position = position;
            Polygon = polygon;
        }
    }
}
