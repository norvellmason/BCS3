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
        private Polygon polygon = new Polygon(new Vector2(100, 100), new Vector2(50, 0), new Vector2(50, 0), new Vector2(0, 0), new Vector2(0, 50), new Vector2(-50, 0), new Vector2(0, -50));

        private Vector2 mousePosition = Mouse.GetState().Position.ToVector2();

        public BroccoliGravitar(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
            : base(graphicsDevice, spriteBatch, content)
        {

        }

        protected override void LoadContent(ContentManager content)
        {
            arrow = content.Load<Texture2D>("Zak/arrow");
            solid = content.Load<Texture2D>("Zak/solid");
        }

        public override void Update(GameTime gameTime)
        {
            mousePosition = Mouse.GetState().Position.ToVector2();
            polygon.Rotation += 0.02f;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();
            
            DrawPolygon(SpriteBatch, polygon);

            if(polygon.ContainsPoint(mousePosition))
                SpriteBatch.Draw(solid, mousePosition, null, Color.Red, 0.0f, new Vector2(0.5f, 0.5f), 10.0f, SpriteEffects.None, 1.0f);
            else
                SpriteBatch.Draw(solid, mousePosition, null, Color.White, 0.0f, new Vector2(0.5f, 0.5f), 10.0f, SpriteEffects.None, 1.0f);

            SpriteBatch.End();
        }

        private void DrawPolygon(SpriteBatch spriteBatch, Polygon polygon)
        {
            DrawLine(spriteBatch, polygon.PointAt(0), polygon.PointAt(polygon.Points.Count - 1), 1, Color.White);

            for (int index = 1; index < polygon.Points.Count; index++)
                DrawLine(spriteBatch, polygon.PointAt(index - 1), polygon.PointAt(index), 1, Color.White);
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
