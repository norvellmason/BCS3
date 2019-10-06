using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public float Rotation;
        public float Radius;
        Texture2D Texture;

        public RaceCar(Texture2D carTexture, float radius)
        {
            this.Texture = carTexture;
            this.Radius = radius;
            this.Position = new Vector2(0.5f, 0.5f);
            this.Rotation = 0f;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 screenDimensions = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);

            spriteBatch.Draw(this.Texture, screenDimensions * Position, null, Color.White, this.Rotation, new Vector2(this.Texture.Width, this.Texture.Height) / 2f, (this.Radius * screenDimensions.X) / this.Texture.Width, SpriteEffects.None, 1f);
        }
    }
}
