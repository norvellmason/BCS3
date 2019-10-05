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
    abstract class GameState
    {
        protected SpriteBatch SpriteBatch;
        protected GraphicsDevice GraphicsDevice;
        private List<Broccoli> broccolis;

        public GameState(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.GraphicsDevice = graphicsDevice;
            this.SpriteBatch = spriteBatch;
            this.LoadContent();
            this.broccolis = new List<Broccoli>();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected virtual void LoadContent()
        {
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected virtual void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {

            // TODO add a check to see if the face is at this location

            for(int index = 0; index < broccolis.Count; index++)
            {
                broccolis[index].update();
                if (broccolis[index].isOutOfBounds)
                {
                    Console.WriteLine("Missed Broccoli");
                    broccolis.RemoveAt(index);
                    index--;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Draw(GameTime gameTime)
        {
            
        }
    }
}
