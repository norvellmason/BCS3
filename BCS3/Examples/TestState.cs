using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class TestState : State
    {
        private GroundSection ground;
        private float position = 0;
        private float velocity = 0;
        private float rotation = 0;

        public TestState()
        {
            int pointCount = 10;
            Vector2[] points = new Vector2[pointCount];

            float y = 240;
            float velocity = 0;

            Random random = new Random(480212674);
            for (int pointIndex = 0; pointIndex < pointCount; pointIndex++)
            {
                points[pointIndex] = new Vector2(pointIndex * (800.0f / (pointCount - 1)), y);

                velocity += (float)(random.NextDouble() * 2 - 1) * 40;
                y += velocity;
            }

            ground = new GroundSection(points);

            ground.getSegmentIndex(800);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            velocity += 10 * ground.Slope(ground.getSegmentIndex(position));
            velocity -= velocity * 0.01f;

            position = ground.MoveX(position, velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            rotation += (float)gameTime.ElapsedGameTime.TotalSeconds * (velocity / 25.0f);

            if (position < 0)
            {
                position = 0;
                velocity *= -1;
            }

            if (position > 800)
            {
                position = 800;
                velocity *= -1;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DodgerBlue);

            spriteBatch.Begin();

            for (int index = 1; index < ground.points.Length; index++)
                DrawLine(spriteBatch, ground.points[index - 1], ground.points[index], 1, Color.Black);

            float y = ground.GetYPosition(position);
            spriteBatch.Draw(Assets.Barrel, new Vector2(position, y - 25.0f), null, Color.White, rotation, new Vector2(50, 50), 0.5f, SpriteEffects.None, 1.0f);

            spriteBatch.End();
        }

        private void DrawLine(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float width, Color color)
        {
            float angle = (float)Math.Atan2(y2 - y1, x2 - x1);
            float length = (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

            spriteBatch.Draw(Assets.Solid, new Rectangle((int)x1, (int)y1, (int)length, (int)width), null, color, angle, new Vector2(0, 0.5f), SpriteEffects.None, 1.0f);
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, float width, Color color)
        {
            DrawLine(spriteBatch, p1.X, p1.Y, p2.X, p2.Y, width, color);
        }
    }
}
