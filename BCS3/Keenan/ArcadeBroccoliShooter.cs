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
        GraphicsDevice thisGraphicsDevice;
        SpriteBatch thisSpriteBatch;

        public ArcadeBroccoliShooter(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager contentManager) :
            base(graphicsDevice, spriteBatch, contentManager)
        {
            this.thisGraphicsDevice = graphicsDevice;
            this.thisSpriteBatch    = spriteBatch;
        }

        protected override void LoadContent(ContentManager contentManager)
        {
            // load your content here
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
