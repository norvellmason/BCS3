using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BCS_3.Zak
{
    public class BroccoliGravitar : GameState
    {
        private Texture2D arrow, solid;
        private Polygon polygon;

        private Polygon player = new Polygon(new Vector2(300, 300), new Vector2(30, 20), new Vector2(-30, 30), new Vector2(-30, -30), new Vector2(30, -20));
        private Vector2 velocity = new Vector2(0, 0);

        private Vector2 mousePosition = Mouse.GetState().Position.ToVector2();

        public BroccoliGravitar(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
            : base(graphicsDevice, spriteBatch, content)
        {
            int pointCount = 20;
            Vector2[] points = new Vector2[pointCount];
            Random random = new Random();

            for(int index = 0; index < pointCount; index++)
            {
                float angle = index * (float)Math.PI * 2 / pointCount;
                float distance = 100 + random.Next(100);

                points[index] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
            }

            polygon = new Polygon(new Vector2(0, 0), points);
        }

        protected override void LoadContent(ContentManager content)
        {
            arrow = content.Load<Texture2D>("Zak/arrow");
            solid = content.Load<Texture2D>("Zak/solid");
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            mousePosition = Mouse.GetState().Position.ToVector2();

            if (keyboardState.IsKeyDown(Keys.Up))
                velocity += Utils.VectorFrom(player.Rotation, 200) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 offset = polygon.Position - player.Position;
            velocity += Vector2.Normalize(offset) * (2000000 / offset.LengthSquared()) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Left))
                player.Rotation += 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Right))
                player.Rotation -= 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            player.Position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (polygon.ContainsPoint(player.Position))
            {
                player.Position = new Vector2(300, 300);
                velocity = new Vector2(0, 0);
            }

            polygon.Rotation += 0.001f;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 0));

            DrawPolygon(SpriteBatch, polygon);
            DrawPolygon(SpriteBatch, player);

            if (polygon.ContainsPoint(mousePosition))
                SpriteBatch.Draw(solid, mousePosition, null, Color.Red, 0.0f, new Vector2(0.5f, 0.5f), 10.0f, SpriteEffects.None, 1.0f);
            else
                SpriteBatch.Draw(solid, mousePosition, null, Color.White, 0.0f, new Vector2(0.5f, 0.5f), 10.0f, SpriteEffects.None, 1.0f);

            SpriteBatch.End();
        }

        private void DrawPolygon(SpriteBatch spriteBatch, Polygon polygon)
        {
            DrawLine(spriteBatch, polygon.PointAt(0), polygon.PointAt(polygon.Points.Count - 1), 2, Color.White);

            for (int index = 1; index < polygon.Points.Count; index++)
                DrawLine(spriteBatch, polygon.PointAt(index - 1), polygon.PointAt(index), 2, Color.White);
        }

        private void DrawLine(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float width, Color color)
        {
            float angle = (float)Math.Atan2(y2 - y1, x2 - x1);
            float length = (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

            spriteBatch.Draw(solid, new Rectangle((int)x1, (int)y1, (int)length, (int)width), null, color, angle, new Vector2(0, 0.5f), SpriteEffects.None, 1.0f);
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, float width, Color color)
        {
            DrawLine(spriteBatch, p1.X, p1.Y, p2.X, p2.Y, width, color);
        }
    }
}
