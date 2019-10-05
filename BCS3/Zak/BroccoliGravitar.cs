using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BCS_3
{
    public class BroccoliGravitar : GameState
    {
        private Texture2D arrow;

        public BroccoliGravitar(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
            : base(graphicsDevice, spriteBatch, content)
        {

        }

        protected override void LoadContent(ContentManager content)
        {
            arrow = content.Load<Texture2D>("Zak/arrow");
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(arrow, new Vector2(100, 100), Color.White);

            SpriteBatch.End();
        }
    }
}
