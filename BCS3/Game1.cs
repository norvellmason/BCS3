﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BCS_3.Zak;

namespace BCS_3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080
            }; 

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteBatch = new SpriteBatch(GraphicsDevice);

            StateManager.AddGameState(new ArcadeBroccoliShooter(GraphicsDevice, spriteBatch, this.Content));
            StateManager.AddGameState(new VisualNovelState(GraphicsDevice, spriteBatch, this.Content, "introVisualNovel.txt"));
            StateManager.AddGameState(new Mason.BroccoliAsteroidsGameState(GraphicsDevice, spriteBatch, this.Content));
            StateManager.AddGameState(new VisualNovelState(GraphicsDevice, spriteBatch, this.Content, "interimVisualNovel1.txt"));
            StateManager.AddGameState(new Zak.BroccoliGravitar(GraphicsDevice, spriteBatch, this.Content));
            StateManager.AddGameState(new VisualNovelState(GraphicsDevice, spriteBatch, this.Content, "interimVisualNovel2.txt"));
            StateManager.AddGameState(new JonsGame.JonsGameState(GraphicsDevice, spriteBatch, this.Content, GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width));
            StateManager.AddGameState(new VisualNovelState(GraphicsDevice, spriteBatch, this.Content, "finalVisualNovel.txt"));            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (StateManager.GetCurrentGameState() != null)
            {
                StateManager.GetCurrentGameState().Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (StateManager.GetCurrentGameState() != null)
            {
                StateManager.GetCurrentGameState().Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
