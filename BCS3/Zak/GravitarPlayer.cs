using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BCS_3.Zak
{
    public class GravitarPlayer
    {
        public Vector2 Position { get; set; }

        public Vector2 Velocity = new Vector2(0, 0);

        public float Angle { get; set; } = 0;

        public bool Thrusting { get; private set; } = false;
        public bool TurningLeft { get; private set; } = false;
        public bool TurningRight { get; private set; } = false;
        public bool Firing { get; private set; } = false;
        public bool Tractoring { get; private set; } = false;

        public bool Alive { get; set; } = true;
        public float DeadTime { get; set; } = 0.0f;

        public float firingTimer = 0;
        public float firingDelay = 0.3f;

        public GravitarPlayer(Vector2 position)
        {
            Position = position;
        }

        public void Control(KeyboardState keyboardState)
        {
            Thrusting = keyboardState.IsKeyDown(Keys.Up);
            TurningLeft = keyboardState.IsKeyDown(Keys.Left);
            TurningRight = keyboardState.IsKeyDown(Keys.Right);
            Firing = keyboardState.IsKeyDown(Keys.Space);
            Tractoring = keyboardState.IsKeyDown(Keys.Down);
        }

        public void Update(float timePassed)
        {
            if (Thrusting)
                Velocity += Utils.VectorFrom(Angle, 200) * timePassed;

            if (TurningLeft)
                Angle += 3 * timePassed;

            if (TurningRight)
                Angle -= 3 * timePassed;

            if (firingTimer > 0)
                firingTimer -= timePassed;

            if (Alive)
                DeadTime = 0;
            else
                DeadTime += timePassed;
        }
    }
}
