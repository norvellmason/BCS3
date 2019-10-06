using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BCS_3.Mason
{
    class BroccoliAsteroidsGameState : GameState
    {
        private RaceCar raceCar;
        private Texture2D carTexture;

        public BroccoliAsteroidsGameState(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager contentManager) : base(graphicsDevice, spriteBatch, contentManager)
        {
            this.raceCar = new RaceCar(carTexture, 0.1f);
        }

        protected override void LoadContent(ContentManager contentManager)
        {
            contentManager.Load<Texture2D>("Mason/car");

            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.raceCar.Draw(gameTime, SpriteBatch);
        }
    }
}
