using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3.Mason
{
    class RaceCar
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float Rotation;
        public float AngularVelocity;
        public float AngularAcceleration;
        public float Radius;
        Texture2D Texture;

        public bool isAlive = true;

        public RaceCar(Texture2D carTexture, float radius)
        {
            this.Texture = carTexture;
            this.Radius = radius;
            this.Rotation = 0f;
        }

        public void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState kState = Keyboard.GetState();

            Vector2 directionVector = Zak.Utils.VectorFrom(this.Rotation);
            directionVector.Y = directionVector.Y * -1;

            if (kState.IsKeyDown(Keys.Up))
            {
                this.Acceleration = 1.5f * directionVector;
            }

            if (kState.IsKeyDown(Keys.Down))
            {
                this.Acceleration = -1.5f * directionVector;
            }

            //if (this.Acceleration.Length() > 2f)
            //{
            //    this.Acceleration.Normalize();
            //    this.Acceleration = this.Acceleration * 2f;
            //}

            if (!(kState.IsKeyDown(Keys.Down) || kState.IsKeyDown(Keys.Up)))
            {
                this.Acceleration = Vector2.Zero;
            }

            if (kState.IsKeyDown(Keys.Left))
            {
                this.AngularAcceleration = (float)(-Math.PI * 4f);
            }

            if (kState.IsKeyDown(Keys.Right))
            {
                this.AngularAcceleration = (float)(Math.PI * 4f);
            }

            if (!(kState.IsKeyDown(Keys.Left) || kState.IsKeyDown(Keys.Right)))
            {
                this.AngularAcceleration = 0f;

                this.AngularVelocity = MoveTowardValue(this.AngularVelocity, 0f, 5f * elapsedTime);
            }

            //this.Velocity.X = MoveTowardValue(this.Velocity.X, 0f, 1f * elapsedTime);
            //this.Velocity.Y = MoveTowardValue(this.Velocity.Y, 0f, 1f * elapsedTime);

            Vector2 velocityDirection = this.Velocity;
            if (velocityDirection != Vector2.Zero)
            {
                velocityDirection.Normalize();
            }

            Vector2 forwardDirection = directionVector;
            Vector2 rightDirection = new Vector2(directionVector.Y, -directionVector.X);

            //Vector2 destinationVector2 = this.Velocity * Vector2.Dot(velocityDirection, directionVector);

            Vector2 forwardVelocity = forwardDirection * Vector2.Dot(forwardDirection, this.Velocity);
            Vector2 rightVelocity = rightDirection * Vector2.Dot(rightDirection, this.Velocity);

            float driftFactor = Math.Abs(AngularVelocity * 5f);

            Vector2 destinationVector2 = forwardVelocity + (rightVelocity * driftFactor * -1);

            this.Velocity.X = MoveTowardValue(this.Velocity.X, destinationVector2.X, 3f * elapsedTime);
            this.Velocity.Y = MoveTowardValue(this.Velocity.Y, destinationVector2.Y, 3f * elapsedTime);

            this.Velocity.X = MoveTowardValue(this.Velocity.X, 0, 0.02f * elapsedTime);
            this.Velocity.Y = MoveTowardValue(this.Velocity.Y, 0, 0.02f * elapsedTime);

            this.Velocity += this.Acceleration * elapsedTime;
            this.Position += (this.Velocity * elapsedTime);

            if (this.Velocity.Length() > 0.3f)
            {
                this.Velocity.Normalize();
                this.Velocity = this.Velocity * 0.3f;
            }

            this.AngularVelocity += this.AngularAcceleration * elapsedTime;
            this.Rotation += this.AngularVelocity * elapsedTime;

            if (this.AngularVelocity > 3f)
            {
                this.AngularVelocity = 3f;
            }

            if (this.AngularVelocity < -3f)
            {
                this.AngularVelocity = -3f;
            }

            this.Rotation = this.Rotation % (float)(Math.PI * 2f);
        }

        //public static void KillOrthogonalVelocity(Car car, float drift = 0f)
        //{
        //    Vector2 forwardVelocity = car.Forward * Vector2.Dot(car.Velocity, car.Forward);
        //    Vector2 rightVelocity = car.Right * Vector2.Dot(car.Velocity, car.Right);
        //    car.Velocity = forwardVelocity + rightVelocity * drift;
        //}

        public void ResetDynamics()
        {
            this.Velocity = Vector2.Zero;
            this.Acceleration = Vector2.Zero;
            this.AngularVelocity = 0;
            this.AngularAcceleration = 0;
            this.Rotation = 0;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 screenDimensions = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);

            spriteBatch.Draw(this.Texture, screenDimensions * Position, null, Color.White, this.Rotation, new Vector2(this.Texture.Width, this.Texture.Height) / 2f, (this.Radius * screenDimensions.X) / this.Texture.Width, SpriteEffects.None, 0.75f);
        }

        public float MoveTowardValue(float current, float destination, float rate)
        {
            float distance = current - destination;

            if (current > destination)
            {
                if (current - rate > destination)
                {
                    return current - rate;
                }
                else
                {
                    return destination;
                }
            }
            else
            {
                if (current + rate < destination)
                {
                    return current + rate;
                }
                else
                {
                    return destination;
                }
            }
        }
    }
}
