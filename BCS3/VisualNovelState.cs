using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BCS_3
{
    internal class VisualNovelState : GameState
    {
        Color bcsBlue = new Color(174, 225, 243);
        Color bcsOrange = new Color(246, 138, 39);
        Color bcsWhite = Color.White;
        Color bcsGrey = Color.DarkGray;

        int screenHeight;
        int screenWidth;
        Rectangle dialogueBox; 


        Rectangle boxRect = new Rectangle(0, 0, 1280, 250);

        public VisualNovelState(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) : base(graphicsDevice, spriteBatch)
        {
            this.screenHeight = graphicsDevice.Viewport.Height;
            this.screenWidth = graphicsDevice.Viewport.Width;
            this.dialogueBox = new Rectangle(0, 0, this.screenWidth, this.screenHeight / 3); 

        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this.bcsBlue); 
            Console.WriteLine(this.screenHeight);
            
        }

        public override void Update(GameTime gameTime)
        {
            SpriteBatch.Draw(dialogueBox); 
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
