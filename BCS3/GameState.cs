using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3
{
    public abstract class GameState
    {
        protected SpriteBatch SpriteBatch;
        protected GraphicsDevice GraphicsDevice;

        public GameState(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.GraphicsDevice = graphicsDevice;
            this.SpriteBatch = spriteBatch;
            this.LoadContent();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected virtual void LoadContent()
        {
            // load your content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected virtual void UnloadContent()
        {
            // unload your content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Draw(GameTime gameTime);
    }
}
