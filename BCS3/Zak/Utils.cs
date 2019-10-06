using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace BCS_3.Zak
{
    public class Utils
    {
        public static Random random = new Random();

        public static Vector2 VectorFrom(float rotation, float length = 1.0f)
        {
            return new Vector2((float)Math.Cos(rotation), -(float)Math.Sin(rotation)) * length;
        }

        public static float AngleOf(Vector2 vector)
        {
            return (float)Math.Atan2(-vector.Y, vector.X);
        }

        public static int DigitsToRepresent(int number)
        {
            return (int)Math.Floor(Math.Log10(number + 1));
        }
    }
}
