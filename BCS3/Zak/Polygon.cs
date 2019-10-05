using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace BCS_3.Zak
{
    public class Polygon
    {
        public Vector2 Position;
        public float Rotation = 0;

        public List<Vector2> Points;
        
        public Polygon(Vector2 position, params Vector2[] points)
        {
            Position = position;
            Points = new List<Vector2>(points);
        }

        public Vector2 PointAt(int index)
        {
            float x = (float)(Math.Cos(Rotation) * Points[index].X + Math.Sin(Rotation) * Points[index].Y);
            float y = (float)(Math.Cos(Rotation) * Points[index].Y - Math.Sin(Rotation) * Points[index].X);

            return Position + new Vector2(x, y);
        }
        
        /// <summary>
        /// Determines if the given point is inside the polygon
        /// </summary>
        /// <param name="points">the vertices of polygon</param>
        /// <param name="testPoint">the given point</param>
        /// <returns>true if the point is inside the polygon; otherwise, false</returns>
        public bool ContainsPoint(Vector2 testPoint)
        {
            bool result = false;
            int j = Points.Count - 1;
            for (int i = 0; i < Points.Count; i++)
            {
                if (PointAt(i).Y < testPoint.Y && PointAt(j).Y >= testPoint.Y || PointAt(j).Y < testPoint.Y && PointAt(i).Y >= testPoint.Y)
                {
                    if (PointAt(i).X + (testPoint.Y - PointAt(i).Y) / (PointAt(j).Y - PointAt(i).Y) * (PointAt(j).X - PointAt(i).X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }
}
