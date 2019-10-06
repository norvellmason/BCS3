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
        public List<Vector2> Points;

        public bool Closed { get; set; } = true;

        public Polygon(params Vector2[] points)
        {
            Points = new List<Vector2>(points);
        }

        public Vector2 PointAt(int index, float angle)
        {
            float x = (float)(Math.Cos(angle) * Points[index].X + Math.Sin(angle) * Points[index].Y);
            float y = (float)(Math.Cos(angle) * Points[index].Y - Math.Sin(angle) * Points[index].X);

            return new Vector2(x, y);
        }
        
        /// <summary>
        /// Determines if the given point is inside the polygon
        /// </summary>
        /// <param name="points">the vertices of polygon</param>
        /// <param name="testPoint">the given point</param>
        /// <returns>true if the point is inside the polygon; otherwise, false</returns>
        public bool ContainsPoint(Vector2 position, float angle, Vector2 testPoint)
        {
            bool result = false;
            int j = Points.Count - 1;
            for (int i = 0; i < Points.Count; i++)
            {
                if((position + PointAt(i, angle)).Y < testPoint.Y && (position + PointAt(j, angle)).Y >= testPoint.Y || (position + PointAt(j, angle)).Y < testPoint.Y && (position + PointAt(i, angle)).Y >= testPoint.Y)
                {
                    if ((position + PointAt(i, angle)).X + (testPoint.Y - (position + PointAt(i, angle)).Y) / ((position + PointAt(j, angle)).Y - (position + PointAt(i, angle)).Y) * ((position + PointAt(j, angle)).X - (position + PointAt(i, angle)).X) < testPoint.X)
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
