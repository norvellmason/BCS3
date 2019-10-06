using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BCS_3.JonsGame
{
    class JonsGameState : GameState
    {
        SpriteFont myFont;

        Texture2D broccImage;
        Vector2 broccSize;

        Texture2D cookieImage;
        Texture2D donutImage;
        Vector2 cookieSize;
        Vector2 donutSize;
        Vector2 eaterSize;

        Vector2 scoreLocation;
        List<Eatable> eatables;
        Random rng;
        Color scoreColor;

        int WindowLength;
        int WindowHeight;

        Texture2D eaterImage;
        Eater player;



        public JonsGameState(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager contentManager) : base(graphicsDevice, spriteBatch, contentManager)
        {
            eatables = new List<Eatable>();
            
            rng = new Random(234234234);

            this.WindowHeight = 1080;
            this.WindowLength = 1920;

            this.scoreLocation = new Vector2(0, 0);
            this.scoreColor = Color.LightGreen;

            this.player = new Eater(this.WindowHeight, this.WindowLength, this.eaterSize.X, this.eaterSize.Y);
          
        }

        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(new Color(255, 255, 255));


            SpriteBatch.Begin();
            if (this.player.Weight < 19)
                SpriteBatch.DrawString(this.myFont, "Weight: " + this.player.Weight, scoreLocation, this.scoreColor);
            else if (this.player.Weight < 28)
                SpriteBatch.DrawString(this.myFont, "Weight: " + this.player.Weight, scoreLocation, Color.Yellow);
            else
                SpriteBatch.DrawString(this.myFont, "Weight: " + this.player.Weight, scoreLocation, Color.Red);


            SpriteBatch.Draw(eaterImage, this.player.position, null, Color.White, this.player.faceDirection, this.eaterSize, this.player.getSize(), SpriteEffects.None, 0);
            foreach (Eatable element in eatables)
            {
                if(element is Broccoli)
                    SpriteBatch.Draw(broccImage, element.position, null, Color.White, element.rotation, this.broccSize, 0.1f, SpriteEffects.None, 0);
                else if (element is Donut)
                    SpriteBatch.Draw(donutImage, element.position, null, Color.White, element.rotation, this.donutSize, .5f, SpriteEffects.None, 0);
                else if (element is Cookie)
                    SpriteBatch.Draw(cookieImage, element.position, null, Color.White, element.rotation, this.cookieSize, .5f, SpriteEffects.None, 0);

            }
            SpriteBatch.End();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent(ContentManager contentManager)
        {
            // load your content here
            this.broccImage = contentManager.Load<Texture2D>("Jon/broccoli");
            this.eaterImage = contentManager.Load<Texture2D>("Jon/BCBoy");
            this.myFont = contentManager.Load<SpriteFont>("Jon/myFont");

            this.donutImage = contentManager.Load<Texture2D>("Jon/donut");
            this.cookieImage = contentManager.Load<Texture2D>("Jon/cookie");

            this.broccSize = new Vector2(this.broccImage.Width / 2, this.broccImage.Height / 2);
            this.cookieSize = new Vector2(this.cookieImage.Width / 2, this.cookieImage.Height / 2);
            this.donutSize = new Vector2(this.donutImage.Width / 2, this.donutImage.Height / 2);


            this.eaterSize = new Vector2(this.eaterImage.Width / 2, this.eaterImage.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            int decider = rng.Next(0, 750);
            if (decider < 40)
            {
                if (decider < 10)
                {
                    eatables.Add(new Broccoli(this.WindowHeight, this.WindowLength, (int)this.player.position.X, (int)this.player.position.Y));
                } else if(decider < 25)
                {
                    eatables.Add(new Donut(this.WindowHeight, this.WindowLength, (int)this.player.position.X, (int)this.player.position.Y));

                } else if(decider < 40)
                {
                    eatables.Add(new Cookie(this.WindowHeight, this.WindowLength, (int)this.player.position.X, (int)this.player.position.Y));
                }
            }

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                this.player.MoveLeft();
            }

            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                this.player.MoveRight();
            }

            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                this.player.MoveUp();
            }

            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                this.player.MoveDown();
            }

            this.player.Update();

            for (int index = 0; index < eatables.Count; index++)
            {
                eatables[index].Update(this.player.position, this.player.Weight);
                if (eatables[index].isOutOfBounds)
                {
                    eatables.RemoveAt(index);
                    index--;
                    continue;
                }

                if (this.player.canEat(eatables[index]))
                {
                    if (! (eatables[index] is Broccoli))
                    {
                        this.player.Weight++;
                    }
                    else
                    {
                        this.player.Weight--;
                    }
                    this.eatables.RemoveAt(index);
                    index--;
                }
            }

        }
    }
}
