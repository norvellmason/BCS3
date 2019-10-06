using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace BCS_3.Zak
{
    public static class Sprites
    {
        public static Polygon PlayerShip = new Polygon(new Vector2(15, 10), new Vector2(-15, 15), new Vector2(-15, -15), new Vector2(15, -10));
        public static Polygon PlayerThrust = new Polygon(new Vector2(-15, 10), new Vector2(-50, 0), new Vector2(-15, -10));
        public static Polygon PlayerThrustLeft = new Polygon(new Vector2(-15, 12.5f), new Vector2(-25, 10), new Vector2(-15, 7.5f));
        public static Polygon PlayerThrustRight = new Polygon(new Vector2(-15, -12.5f), new Vector2(-25, -10), new Vector2(-15, -7.5f));
        public static Polygon TractorBeam = new Polygon(new Vector2(-15, 5), new Vector2(-100, 15), new Vector2(-100, -15), new Vector2(-15, -5));

        public static Polygon Bullet = new Polygon(new Vector2(10, 0), new Vector2(2, 2), new Vector2(0, 10), new Vector2(-2, 2), new Vector2(-10, 0), new Vector2(-2, -2), new Vector2(0, -10), new Vector2(2, -2));
        public static Polygon Broccoli = new Polygon(new Vector2(15, -5), new Vector2(20, -15), new Vector2(10, -20), new Vector2(0, -15), new Vector2(-10, -20), new Vector2(-20, -15), new Vector2(-15, -5), new Vector2(-20, 5), new Vector2(-15, 15), new Vector2(-5, 10), new Vector2(-5, 25), new Vector2(5, 25), new Vector2(5, 10), new Vector2(15, 15), new Vector2(20, 5));

        public static Polygon Sungdroid = new Polygon(new Vector2(-15, 20), new Vector2(15, 20), new Vector2(10, 0), new Vector2(-10, 0));
        public static Polygon QueenSungdroid = new Polygon(new Vector2(-15, 20), new Vector2(15, 20), new Vector2(10, 0), new Vector2(5, 0), new Vector2(0, -40), new Vector2(-5, 0), new Vector2(-10, 0));

        public static Polygon[] Digits =
        {
            new Polygon(new Vector2(-0.5f, 1), new Vector2(0.5f, -1), new Vector2(-0.5f, -1), new Vector2(-0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, -1)) { Closed = false  },
            new Polygon(new Vector2(0.5f, 1), new Vector2(0.5f, -1)) { Closed = false },
            new Polygon(new Vector2(-0.5f, -1), new Vector2(0.5f, -1), new Vector2(0.5f, 0), new Vector2(-0.5f, 0), new Vector2(-0.5f, 1), new Vector2(0.5f, 1)) { Closed = false },
            new Polygon(new Vector2(-0.5f, -1), new Vector2(0.5f, -1), new Vector2(0.5f, 0), new Vector2(-0.5f, 0), new Vector2(0.5f, 0), new Vector2(0.5f, 1), new Vector2(-0.5f, 1)) { Closed = false },
            new Polygon(new Vector2(-0.5f, -1), new Vector2(-0.5f, 0), new Vector2(0.5f, 0), new Vector2(0.5f, -1), new Vector2(0.5f, 1)) { Closed = false },
            new Polygon(new Vector2(0.5f, -1), new Vector2( -0.5f, -1), new Vector2(-0.5f, 0), new Vector2(0.5f, 0), new Vector2(0.5f, 1), new Vector2(-0.5f, 1)) { Closed = false },
            new Polygon(new Vector2(0.5f, -1), new Vector2(-0.5f, -1), new Vector2(-0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 0), new Vector2(-0.5f, 0)) { Closed = false },
            new Polygon(new Vector2(-0.5f, -1), new Vector2(0.5f, -1), new Vector2(0.5f, 1)) { Closed = false },
            new Polygon(new Vector2(0.5f, 0), new Vector2(0.5f, -1), new Vector2(-0.5f, -1), new Vector2(-0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 0), new Vector2(-0.5f, 0)) { Closed = false },
            new Polygon(new Vector2(0.5f, 1), new Vector2(0.5f, -1), new Vector2(-0.5f, -1), new Vector2(-0.5f, 0), new Vector2(0.5f, 0)) { Closed = false },
        };
        public static Polygon Minus = new Polygon(new Vector2(0.5f, 0), new Vector2(-0.5f, 0));

        public static Polygon Paticle = new Polygon(new Vector2(0, 0), new Vector2(0, -6), new Vector2(-3, 3), new Vector2(3, 6));

        public static Polygon circle = new Polygon(new Vector2(1, 0), new Vector2(0.707106f, 0.707106f), new Vector2(0, 1), new Vector2(-0.707106f, 0.707106f), new Vector2(-1, 0), new Vector2(-0.707106f, -0.707106f), new Vector2(0, -1), new Vector2(0.707106f, -0.707106f));
        public static Polygon Arrow = new Polygon(new Vector2(20, 0), new Vector2(0, -10), new Vector2(0, 10));
    }
}
