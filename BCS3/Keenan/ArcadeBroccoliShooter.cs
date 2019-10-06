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
        // Textures
        private Texture2D broccoli;
        private Texture2D trash;
        private Texture2D crosshair;
        private Texture2D solid;

        private SpriteFont font;

        //Lists of objects
        private Dictionary<Objects, Rectangle> images;
        private Dictionary<Rectangle, Objects> imagesReverseLookup;
        private Dictionary<Rectangle, bool> boxes;

        //Other stuff
        private bool hide;
        private bool newRound;
        private bool done;
        private Random rand;
        private float timer;
        private int squaresDrawn;
        private int score;
        private int numberOfBroccoli;
        private int origNumBroccoli;

        
        public ArcadeBroccoliShooter(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager contentManager) :
            base(graphicsDevice, spriteBatch, contentManager)
        {
            images = new Dictionary<Objects, Rectangle>();
            imagesReverseLookup = new Dictionary<Rectangle, Objects>();
            boxes = new Dictionary<Rectangle, bool>();

            hide = false;
            newRound = true;
            done = false;
            rand = new Random();
            timer = 0;
            squaresDrawn = 0;
            score = 0;
            numberOfBroccoli = 0;
        }

        protected override void LoadContent(ContentManager contentManager)
        {
            // load your content here
            broccoli = contentManager.Load <Texture2D>("Keenan/Broccoli");
            trash = contentManager.Load<Texture2D>("Keenan/trash");
            crosshair = contentManager.Load<Texture2D>("Keenan/crosshair");
            solid = contentManager.Load<Texture2D>("Keenan/solid");

            font = contentManager.Load<SpriteFont>("Keenan/File");
        }

        public override void Draw(GameTime gameTime)
        {                      
            GraphicsDevice.Clear(Color.White);

            SpriteBatch.Begin();

            //Every position is calculated in the most garbage way possible but IDC
            Vector2 firstRectangle = new Vector2((int)(.075 * GraphicsDevice.Viewport.Width), (int)(.075 * GraphicsDevice.Viewport.Height));

            //Draw the elements
            //Draw 3 x 3 grid of  images            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    DrawImages(firstRectangle, i , j);
                }
            }     

            foreach (Objects obj in images.Keys)
            {
                SpriteBatch.Draw(obj.image, images[obj],Color.White);
            }

            //Draw the tiles
            if (hide)
            {
                DrawTiles(firstRectangle);
            }

            //No longer edgy
            SpriteBatch.Draw(crosshair, 
                new Rectangle(Mouse.GetState().Position.X, 
                Mouse.GetState().Position.Y, 
                (int)(.15 * GraphicsDevice.Viewport.Width), 
                (int)(.15 * GraphicsDevice.Viewport.Height)), 
                Color.White
                );


            //Draw score
            SpriteBatch.DrawString(font, score + "", new Vector2((int)(.025 * GraphicsDevice.Viewport.Width), (int)(.025 * GraphicsDevice.Viewport.Height)), Color.Black);

            //Draw end
            if (done && score == origNumBroccoli * 100)
            {
                SpriteBatch.DrawString(font, "You are ready my son", new Vector2((int)(.375 * GraphicsDevice.Viewport.Width), (int)(.5 * GraphicsDevice.Viewport.Height)), Color.Green);
            }
            else if (done && score < origNumBroccoli * 100)
            {
                SpriteBatch.DrawString(font, "You failed me son...", new Vector2((int)(.375 * GraphicsDevice.Viewport.Width), (int)(.5 * GraphicsDevice.Viewport.Height)), Color.Red);
            }

            SpriteBatch.End();
        }

        private void DrawScore()
        {
        }

        private void DrawImages(Vector2 firstRectangle, int i, int j)
        {
            Texture2D image;
            Objects brocOrTrash = null;
        
            if (squaresDrawn < 9)
            {
                if (numberOfBroccoli == 0)
                {
                    image = broccoli;
                    Broccoli_Keenan broc = new Broccoli_Keenan(image);
                    brocOrTrash = broc;
                    numberOfBroccoli++;
                    origNumBroccoli++;
                }
                else
                {
                    if (rand.Next(100) > 70)
                    {
                        image = broccoli;
                        Broccoli_Keenan broc = new Broccoli_Keenan(image);
                        brocOrTrash = broc;
                        numberOfBroccoli++;
                        origNumBroccoli++;
                    }
                    else
                    {
                        image = trash;
                        Trash trashObject = new Trash(image);
                        brocOrTrash = trashObject;
                    }
                }

                squaresDrawn++;
            }

            if (brocOrTrash != null && !images.ContainsKey(brocOrTrash))
            {
                Rectangle rectangle = new Rectangle(
                    (int)firstRectangle.X + (j * (int)(.30 * GraphicsDevice.Viewport.Width)),
                    (int)firstRectangle.Y + (i * (int)(.30 * GraphicsDevice.Viewport.Height)),
                    (int)(.25 * GraphicsDevice.Viewport.Width),
                    (int)(.25 * GraphicsDevice.Viewport.Height)
                    );


                images.Add(brocOrTrash, rectangle);
                imagesReverseLookup.Add(rectangle, brocOrTrash);
            }
           

            newRound = false;
        }

        private void DrawTiles(Vector2 firstRectangle)
        {
            //Draw 3 x 3 grid of black squares
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Rectangle box = new Rectangle(
                        (int)firstRectangle.X + (j * (int)(.30 * GraphicsDevice.Viewport.Width)),
                        (int)firstRectangle.Y + (i * (int)(.30 * GraphicsDevice.Viewport.Height)),
                        (int)(.25 * GraphicsDevice.Viewport.Width),
                        (int)(.25 * GraphicsDevice.Viewport.Height)
                        );

                    if (!boxes.ContainsKey(box))
                        boxes.Add(box, true);

                    if (boxes[box])
                        SpriteBatch.Draw(solid, box, Color.Orange);
                }
            }
        }

        private void CheckCollision(MouseState state)
        {
            foreach (Rectangle box in boxes.Keys.ToList())
            {
                if (box.Contains(state.Position) && !done)
                {
                    AwardPoints(box, state.Position);
                    boxes[box] = false;
                }
            }
        }

        private void AwardPoints(Rectangle box, Point position)
        {
            if (images.ContainsValue(box) && boxes[box] && !done)
            {
                if (imagesReverseLookup[box].image == broccoli)
                {
                    score += 100;
                    numberOfBroccoli--;
                }
                else
                {
                    score -= 100;
                }
            }                                        
        }

        public override void Update(GameTime gameTime)
        {
            //Hide the images after a bit
            if (timer > 50 && !this.hide)
            {
                this.hide = true;
                timer = 0;
            }
            else
            {
                timer++;
            }

            MouseState state = Mouse.GetState();

            if (state.LeftButton == ButtonState.Pressed)
            {
                CheckCollision(state);
            }

            Console.WriteLine(numberOfBroccoli);

            if (numberOfBroccoli <= 0 && !newRound)
                done = true;

            if (done && timer > 80)
                StateManager.AdvanceGameState();
            else
                timer++;

        }
    }
}
