using Microsoft.Xna.Framework;

namespace BCS_3.Zak
{
    public class GravitarBullet
    {
        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public float Angle { get; set; }

        public bool Friendly { get; private set; }

        public bool Remove { get; set; } = false;

        public GravitarBullet(Vector2 position, Vector2 velocity, bool friendly)
        {
            Position = position;
            Velocity = velocity;
            Friendly = friendly;
        }

        public void Update(float timePassed)
        {
            Angle += timePassed * 20;
        }
    }
}
