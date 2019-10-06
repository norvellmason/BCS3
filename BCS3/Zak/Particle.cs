using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace BCS_3.Zak
{
    public class Particle
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public float Angle { get; set; }
        public float AngularVelocity { get; set; }

        public float Scale { get; set; }

        public Color Color { get; set; }

        public float Life { get; set; }

        public bool Remove
        {
            get
            {
                return Life <= 0;
            }
        }

        public Particle(Vector2 position, Vector2 velocity, Color color)
        {
            Position = position;
            Velocity = velocity;

            Angle = (float)Utils.random.NextDouble() * 2 * (float)Math.PI;
            AngularVelocity = 1.0f + (float)Utils.random.NextDouble();

            Scale = 0.9f + 0.2f * (float)Utils.random.NextDouble();

            Color = color;
        }

        public void Update(float timePassed)
        {
            Position += Velocity * timePassed;
            Angle += AngularVelocity * timePassed;

            Life -= timePassed;
        }
    }
}
