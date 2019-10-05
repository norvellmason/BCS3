using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BCS_3
{
    public class ArcadeBroccoliShooter : GameState
    {
        private Texture2D arrow;

        public ArcadeBroccoliShooter(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager contentManager) :
            base(graphicsDevice, spriteBatch, contentManager)
        {

        }

        protected override void LoadContent(ContentManager contentManager)
        {
            // load your content here
            arrow = contentManager.Load < Texture2D>("Keenan/Arrow");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(arrow, new Vector2((GraphicsDevice.Viewport.Width/2 - arrow.Width/2), (GraphicsDevice.Viewport.Height/2 - arrow.Height/2)), Color.White);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
