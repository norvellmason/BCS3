using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BCS_3.Mason
{
    class BroccoliAsteroidsGameState : GameState
    {
        private RaceCar raceCar;
        private Texture2D carTexture;
        private List<Texture2D> trackTextures;
        private int currentTrackIndex;
        private Color[] currentTrackImageData;
        private Texture2D backgroundTexture;

        private ParticleEngine particleEngine = new ParticleEngine();
        private Texture2D circleParticle;

        private static GraphicsDevice staticGraphicsDevice;
        private static Random random = new Random();

        private float timeRemaining = 60f;
        private SpriteFont arialFont;

        private float secondsUntilRespawn = 1f;


        public BroccoliAsteroidsGameState(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager contentManager) : base(graphicsDevice, spriteBatch, contentManager)
        {
            this.raceCar = new RaceCar(carTexture, 0.05f);
            currentTrackIndex = -1;
            ResetRace();
            AdvanceRace();

            staticGraphicsDevice = graphicsDevice;
        }

        protected override void LoadContent(ContentManager contentManager)
        {
            this.carTexture = contentManager.Load<Texture2D>("Mason/car");

            this.trackTextures = new List<Texture2D>();

            for (int i = 0; i < 3; i++)
            {
                this.trackTextures.Add(contentManager.Load<Texture2D>("Mason/Track" + (i + 1)));
            }

            this.circleParticle = contentManager.Load<Texture2D>("Mason/Particle_Circle");

            this.backgroundTexture = contentManager.Load<Texture2D>("Mason/background");

            this.arialFont = contentManager.Load<SpriteFont>("Mason/File");

            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!this.raceCar.isAlive)
            {
                secondsUntilRespawn -= elapsedTime;

                if (secondsUntilRespawn <= 0)
                {
                    this.raceCar.isAlive = true;
                    particleEngine.AddPointParticleEmmiter(raceCar.Position, 1f, new List<Texture2D> { this.circleParticle }, new Color[2] { Color.Gray, Color.DarkGray }, new float[2] { 0.5f, 0.75f }, new float[2] { 0.005f, 0.02f }, new float[2] { 0.1f, 0.1f }, new float[2] { 0.2f, 0.3f }, 60, 0.3f);
                }
            }

            if (this.raceCar.isAlive)
            {
                this.raceCar.Update(gameTime);

                CheckForReset();

                CheckForVictory();

                AddDriftTracks();
            }

            particleEngine.Update(elapsedTime);

            timeRemaining -= elapsedTime;

            if (timeRemaining <= 0)
            {
                //Make player lose
            }

            base.Update(gameTime);
        }

        public void AddDriftTracks()
        {
            if (Math.Abs(this.raceCar.AngularAcceleration) > 0.2f)
            {
                int imagePosition = GetRaceCarImagePosition();
                Vector2 carImageCoordinates = new Vector2((int)(this.raceCar.Position.X * 1920), (int)(this.raceCar.Position.Y * 1080));

                List<Vector2> nearbyCarPositions = new List<Vector2>();

                Vector2 carDirection = Zak.Utils.VectorFrom(this.raceCar.Rotation);
                carDirection.Y = carDirection.Y * -1;

                Vector2 rightCarDirection = new Vector2(-carDirection.Y, carDirection.X);
                Vector2 leftCarDirection = rightCarDirection * -1;

                DrawDot(8, carImageCoordinates + rightCarDirection * 15 - carDirection * 25, new Color(1, 1, 1));
                DrawDot(8, carImageCoordinates + leftCarDirection * 15 - carDirection * 25, new Color(1, 1, 1));


                this.trackTextures[currentTrackIndex].SetData(this.currentTrackImageData);
            }
        }

        public void DrawDot(int radius, Vector2 center, Color color)
        {
            for (int x = (int)center.X - radius; x < center.X + radius + 1; x++)
            {
                for (int y = (int)center.Y - radius; y < center.Y + radius + 1; y++)
                {
                    if (Vector2.Distance(center, new Vector2(x, y)) < radius)
                    {
                        int pixelPosition = GetImagePosition(x, y);

                        if (pixelPosition < (1920 * 1080) && pixelPosition >= 0 && this.currentTrackImageData[pixelPosition] != Color.Transparent)
                        {
                            this.currentTrackImageData[pixelPosition] = color;
                        }
                    }
                }
            }
        }

        public void CheckForReset()
        {
            int imagePosition = GetRaceCarImagePosition();

            if (imagePosition > (1920 * 1080) || imagePosition < 0 || this.currentTrackImageData[imagePosition] == Color.Transparent || this.currentTrackImageData[imagePosition] == Color.White)
            {
                particleEngine.AddPointParticleEmmiter(raceCar.Position, 1f, new List<Texture2D> { this.circleParticle }, new Color[2] { Color.Yellow, Color.Red }, new float[2] { 0.75f, 1f }, new float[2] { 0.005f, 0.02f }, new float[2] { 0.2f, 0.4f }, new float[2] { 0.05f, 0.2f }, 40, 0.3f);

                Vector2 carImageCoordinates = new Vector2((int)(this.raceCar.Position.X * 1920), (int)(this.raceCar.Position.Y * 1080));

                for (int i = 0; i < 20; i++)
                {
                    DrawDot(10, carImageCoordinates + new Vector2(40 * (float)random.NextDouble() - 20, 40 * (float)random.NextDouble() - 20), new Color(random.Next(155) + 100, 0, 0) * ((float)(random.NextDouble() * 0.5f) + 0.5f));
                }

                this.trackTextures[currentTrackIndex].SetData(this.currentTrackImageData);

                ResetRace();
            }
        }

        public void CheckForVictory()
        {
            if (this.currentTrackImageData[GetRaceCarImagePosition()] == Color.Black)
            {
                AdvanceRace();
            }
        }

        public void ResetRace()
        {
            this.raceCar.Position = new Vector2(0.5f, 0.85f);
            this.raceCar.ResetDynamics();
            this.raceCar.isAlive = false;
            this.secondsUntilRespawn = 1f;
        }

        public void AdvanceRace()
        {
            currentTrackIndex++;

            if (currentTrackIndex < this.trackTextures.Count)
            {
                this.currentTrackImageData = new Color[1920 * 1080];
                this.trackTextures[currentTrackIndex].GetData(this.currentTrackImageData);
                ResetRace();
            }
            else
            {
                //Make player win
            }
        }

        public int GetRaceCarImagePosition()
        {
            Vector2 carImageCoordinates = new Vector2(this.raceCar.Position.X * 1920, this.raceCar.Position.Y * 1080);

            int x = (int)carImageCoordinates.X;
            int y = (int)carImageCoordinates.Y;
            int imagePosition = GetImagePosition(x, y);

            return imagePosition;
        }

        public int GetImagePosition(int x, int y)
        {
            return y * 1920 + x;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            SpriteBatch.Begin(SpriteSortMode.FrontToBack);

            SpriteBatch.Draw(this.backgroundTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, GraphicsDevice.Viewport.Width / (float)this.backgroundTexture.Width, SpriteEffects.None, 0f);

            Vector2 screenDimensions = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            if (this.raceCar.isAlive)
            {
                this.raceCar.Draw(gameTime, SpriteBatch);
            }

            Texture2D trackTexture = this.trackTextures[currentTrackIndex];
            SpriteBatch.Draw(trackTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, screenDimensions.X / trackTexture.Width, SpriteEffects.None, 0.1f);

            particleEngine.Draw(SpriteBatch);

            string timerString = "" + (int)timeRemaining;
            Vector2 textDimensions = arialFont.MeasureString(timerString);
            SpriteBatch.DrawString(arialFont, timerString, new Vector2(10, 10), Color.White, 0f, Vector2.Zero, (screenDimensions.X * 0.08f) / textDimensions.X, SpriteEffects.None, 1f);

            SpriteBatch.End();
        }

        public static Vector2 WorldToScreenCoordinates(Vector2 worldCoordinates)
        {
            return new Vector2(worldCoordinates.X * staticGraphicsDevice.Viewport.Width, worldCoordinates.Y * staticGraphicsDevice.Viewport.Height);
        }

        public static float WorldToScreenSize(float horizontalSize)
        {
            return horizontalSize * staticGraphicsDevice.Viewport.Width;
        }
    }
}
