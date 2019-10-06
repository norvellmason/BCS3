using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS_3.Keenan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BCS_3
{
    public class ArcadeBroccoliShooter : GameState
    {
        private Texture2D broccoli;
        private Texture2D trash;
        private Texture2D crosshair;

        private List<Broccoli_Keenan> broccolis;
        private List<Trash> trashCans;
        private List<Child> children;
        
        Random rand;
        int totalSpawned;

        public ArcadeBroccoliShooter(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager contentManager) :
            base(graphicsDevice, spriteBatch, contentManager)
        {
            broccolis = new List<Broccoli_Keenan>();
            trashCans = new List<Trash>();
            children = new List<Child>();

            rand = new Random();
            totalSpawned = 0;
        }

        protected override void LoadContent(ContentManager contentManager)
        {
            // load your content here
            broccoli = contentManager.Load <Texture2D>("Keenan/Broccoli");
            trash = contentManager.Load<Texture2D>("Keenan/trash");
            crosshair = contentManager.Load<Texture2D>("Keenan/crosshair");
        }

        public override void Draw(GameTime gameTime)
        {            
            GraphicsDevice.Clear(Color.White);

            SpriteBatch.Begin();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    //todo

            SpriteBatch.Draw(crosshair, new Vector2(Mouse.GetState().Position.X - crosshair.Width / 2, Mouse.GetState().Position.Y - crosshair.Height / 2), Color.White);
            
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            

            Console.WriteLine(Mouse.GetState().Position);
        }
    }
}
