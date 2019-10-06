using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BCS_3.Zak
{
    public class BroccoliGravitar : GameState
    {
        private Random random = new Random();

        private Texture2D solid;

        private Planet planet;
        private GravitarPlayer player = new GravitarPlayer(new Vector2(300, 300));
        private int score = 0;

        private float scale = 0.1f;
        private float goalScale = 0.9f;

        private float queenChance = 0.0f;
        private int level = 0;

        private List<GravitarBullet> bullets = new List<GravitarBullet>();
        private List<GravitarBroccoli> broccolis = new List<GravitarBroccoli>();
        private List<GravitarEnemy> enemies = new List<GravitarEnemy>();
        private List<Particle> particles = new List<Particle>();

        private float screenMargin = 30;

        public bool Victory
        {
            get
            {
                return broccolis.Count == 0 && enemies.Count == 0;
            }
        }

        public BroccoliGravitar(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
            : base(graphicsDevice, spriteBatch, content)
        {
            NextLevel();
        }

        protected override void LoadContent(ContentManager content)
        {
            solid = content.Load<Texture2D>("Zak/solid");
        }

        private void KillPlayer()
        {
            player.Alive = false;
            AddParticles(player.Position, 10, Color.White);

            score -= 150;
        }

        private void SpawnPlayer()
        {
            float angle = (float)random.NextDouble() * 2 * (float)Math.PI;
            float x = (float)Math.Cos(angle) * 500.0f;
            float y = (float)Math.Sin(angle) * 500.0f;

            player.Alive = true;
            player.Position = new Vector2(x, y);
            player.Velocity = new Vector2(0, 0);

            foreach (GravitarEnemy enemy in enemies)
                enemy.firingTimer = 5.0f + 5.0f * (float)random.NextDouble();
        }

        private void NextLevel()
        {
            planet = new Planet(Vector2.Zero);
            queenChance += 0.1f;
            level += 1;

            for (int index = 0; index < 10; index++)
                broccolis.Add(new GravitarBroccoli(planet, random.Next(planet.Polygon.Points.Count), (float)random.NextDouble()));

            for (int index = 0; index < 10; index++)
            {
                EnemyType type = EnemyType.Standard;
                if (Utils.random.NextDouble() < queenChance)
                    type = EnemyType.Queen;

                enemies.Add(new GravitarEnemy(type, planet, random.Next(planet.Polygon.Points.Count), (float)random.NextDouble()));
            }

            SpawnPlayer();
        }

        private void AddParticles(Vector2 position, int count, Color color)
        {
            for(int index = 0; index < count; index++)
            {
                float angle = (float)Utils.random.NextDouble() * 2 * (float)Math.PI;
                float speed = 25 + 30 * (float)Utils.random.NextDouble();

                Vector2 velocity = Utils.VectorFrom(angle, speed);

                float life = 1 + (float)Utils.random.NextDouble();

                particles.Add(new Particle(position, velocity, color) { Life = life });
            }
        }

        private bool OnScreen(Vector2 point, Viewport viewport)
        {
            if (point.X * scale < -viewport.Width / 2 + screenMargin || point.X * scale > viewport.Width / 2 - screenMargin)
                return false;

            if (point.Y * scale < -viewport.Height / 2 + screenMargin || point.Y * scale > viewport.Height / 2 - screenMargin)
                return false;

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Victory)
                goalScale = 0.0f;
            else if (!player.Alive)
                goalScale = 0.7f;
            else
                goalScale = 0.9f;

            if(Victory && scale < 0.01)
            {
                NextLevel();
            }

            scale += (goalScale - scale) * 0.025f;

            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                enemies.Clear();
                broccolis.Clear();
            }

            UpdatePlayer((float)gameTime.ElapsedGameTime.TotalSeconds);
            UpdateBullets((float)gameTime.ElapsedGameTime.TotalSeconds);
            UpdateParticles((float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (GravitarBroccoli broccoli in broccolis)
                broccoli.MoveToParent();

            foreach (GravitarEnemy enemy in enemies)
            {
                enemy.MoveToParent();
                enemy.firingTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (enemy.firingTimer <= 0 && player.Alive)
                {
                    Vector2 playerOffset = player.Position - enemy.Position;

                    Vector2 targetPosition = player.Position;
                    if (enemy.Type == EnemyType.Queen)
                        targetPosition += player.Velocity * playerOffset.Length() / 7.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    bullets.Add(new GravitarBullet(enemy.Position, Vector2.Normalize(targetPosition - enemy.Position) * 7, false));
                    enemy.firingTimer = enemy.firingDelay + (float)random.NextDouble() * 0.3f;
                }
            }

            if(player.Tractoring)
            {
                foreach(GravitarBroccoli broccoli in broccolis)
                {
                    if(Sprites.TractorBeam.ContainsPoint(player.Position, player.Angle, broccoli.Position))
                    {
                        score += 100;
                        broccoli.Remove = true;

                        AddParticles(broccoli.Position, 10, Color.Green);
                    }
                }
            }

            bullets.RemoveAll(bullet => bullet.Remove);
            broccolis.RemoveAll(broccoli => broccoli.Remove);
            enemies.RemoveAll(enemy => enemy.Remove);
            particles.RemoveAll(particle => particle.Remove);

            planet.Angle += 0.001f;
        }

        private void UpdatePlayer(float timePassed)
        {
            if (!player.Alive && player.DeadTime > 3.0f)
                SpawnPlayer();

            KeyboardState keyboardState = Keyboard.GetState();
            player.Control(keyboardState);
            player.Update(timePassed);

            if (player.Alive && !Victory)
            {
                Vector2 offset = planet.Position - player.Position;
                player.Velocity += Vector2.Normalize(offset) * (5000000 / offset.LengthSquared()) * timePassed;

                player.Position += player.Velocity * timePassed;

                if (player.Firing && player.firingTimer <= 0)
                {
                    bullets.Add(new GravitarBullet(player.Position + Utils.VectorFrom(player.Angle, 20), Utils.VectorFrom(player.Angle, 8), true));
                    player.firingTimer = player.firingDelay;
                }

                // collide with asteroid
                if (planet.Polygon.ContainsPoint(planet.Position, planet.Angle, player.Position))
                    KillPlayer();
            }
        }

        private void UpdateBullets(float timePassed)
        {
            foreach (GravitarBullet bullet in bullets)
            {
                bullet.Update(timePassed);
                bullet.Position += bullet.Velocity;

                if (planet.Polygon.ContainsPoint(planet.Position, planet.Angle, bullet.Position))
                    bullet.Remove = true;

                if (Sprites.PlayerShip.ContainsPoint(player.Position, player.Angle, bullet.Position) && !bullet.Friendly && player.Alive)
                {
                    bullet.Remove = true;
                    KillPlayer();
                }

                if (bullet.Friendly)
                {
                    foreach (GravitarEnemy enemy in enemies)
                    {
                        if (Sprites.Sungdroid.ContainsPoint(enemy.Position, enemy.Angle, bullet.Position))
                        {
                            score += 50;

                            bullet.Remove = true;
                            enemy.Remove = true;

                            AddParticles(enemy.Position, 10, Color.Red);
                        }
                    }
                }

                if (bullet.Position.X < -2000 || bullet.Position.X > 2000 || bullet.Position.Y < -2000 || bullet.Position.Y > 2000)
                    bullet.Remove = true;
            }
        }

        private void UpdateParticles(float timePassed)
        {
            foreach (Particle particle in particles)
                particle.Update(timePassed);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(transformMatrix: Matrix.CreateScale(scale) * Matrix.CreateTranslation(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 0));

            DrawPolygon(SpriteBatch, planet.Polygon, planet.Position, planet.Angle, 1.0f, Color.Blue);

            if(OnScreen(player.Position, SpriteBatch.GraphicsDevice.Viewport))
                DrawPlayer();

            foreach (GravitarBullet bullet in bullets)
                DrawPolygon(SpriteBatch, Sprites.Bullet, bullet.Position, bullet.Angle, 1.0f, Color.Red);

            foreach (GravitarBroccoli broccoli in broccolis)
                DrawPolygon(SpriteBatch, Sprites.Broccoli, broccoli.Position, broccoli.Angle, 1.0f, Color.Green);

            foreach (GravitarEnemy enemy in enemies)
            {
                Polygon polygon = Sprites.Sungdroid;
                if (enemy.Type == EnemyType.Queen)
                    polygon = Sprites.QueenSungdroid;

                DrawPolygon(SpriteBatch, polygon, enemy.Position, enemy.Angle, 1.0f, Color.Red);
            }

            foreach (Particle particle in particles)
                DrawPolygon(SpriteBatch, Sprites.Paticle, particle.Position, particle.Angle, particle.Scale, particle.Color);

            SpriteBatch.End();

            SpriteBatch.Begin();

            DrawNumber(SpriteBatch, score, new Vector2(30, 30), Color.White);
            DrawNumber(SpriteBatch, level, new Vector2(SpriteBatch.GraphicsDevice.Viewport.Width - 50, 30) - new Vector2(30, 0) * (Utils.DigitsToRepresent(level) - 1), Color.White);
            if (!OnScreen(player.Position, SpriteBatch.GraphicsDevice.Viewport))
                DrawPlayerHUD();

            SpriteBatch.End();
        }

        private void DrawPlayer()
        {
            if (player.Alive)
            {
                DrawPolygon(SpriteBatch, Sprites.PlayerShip, player.Position, player.Angle, 1.0f, Color.White);

                if (player.Thrusting && random.NextDouble() < 0.5)
                    DrawPolygon(SpriteBatch, Sprites.PlayerThrust, player.Position, player.Angle, 1.0f, Color.Orange);

                if (player.TurningLeft && random.NextDouble() < 0.5)
                    DrawPolygon(SpriteBatch, Sprites.PlayerThrustLeft, player.Position, player.Angle, 1.0f, Color.Orange);

                if (player.TurningRight && random.NextDouble() < 0.5)
                    DrawPolygon(SpriteBatch, Sprites.PlayerThrustRight, player.Position, player.Angle, 1.0f, Color.Orange);

                if (player.Tractoring && random.NextDouble() < 0.8)
                    DrawPolygon(SpriteBatch, Sprites.TractorBeam, player.Position, player.Angle, 1.0f, Color.Cyan);
            }
        }

        private void DrawPlayerHUD()
        {
            Vector2 screenPosition = player.Position * scale;
            if (screenPosition.X < -SpriteBatch.GraphicsDevice.Viewport.Width / 2 + screenMargin)
                screenPosition = new Vector2(-SpriteBatch.GraphicsDevice.Viewport.Width / 2 + screenMargin, screenPosition.Y);
            else if (screenPosition.X > SpriteBatch.GraphicsDevice.Viewport.Width / 2 - screenMargin)
                screenPosition = new Vector2(SpriteBatch.GraphicsDevice.Viewport.Width / 2 - screenMargin, screenPosition.Y);

            if (screenPosition.Y < -SpriteBatch.GraphicsDevice.Viewport.Height / 2 + screenMargin)
                screenPosition = new Vector2(screenPosition.X, -SpriteBatch.GraphicsDevice.Viewport.Height / 2 + screenMargin);
            else if (screenPosition.Y > SpriteBatch.GraphicsDevice.Viewport.Height / 2 - screenMargin)
                screenPosition = new Vector2(screenPosition.X, SpriteBatch.GraphicsDevice.Viewport.Height / 2 - screenMargin);

            screenPosition += new Vector2(SpriteBatch.GraphicsDevice.Viewport.Width, SpriteBatch.GraphicsDevice.Viewport.Height) / 2;

            DrawPolygon(SpriteBatch, Sprites.PlayerShip, screenPosition, player.Angle, 1.0f, Color.White);

            if (player.Thrusting && random.NextDouble() < 0.5)
                DrawPolygon(SpriteBatch, Sprites.PlayerThrust, screenPosition, player.Angle, 1.0f, Color.Orange);

            if (player.TurningLeft && random.NextDouble() < 0.5)
                DrawPolygon(SpriteBatch, Sprites.PlayerThrustLeft, screenPosition, player.Angle, 1.0f, Color.Orange);

            if (player.TurningRight && random.NextDouble() < 0.5)
                DrawPolygon(SpriteBatch, Sprites.PlayerThrustRight, screenPosition, player.Angle, 1.0f, Color.Orange);

            if (player.Tractoring && random.NextDouble() < 0.8)
                DrawPolygon(SpriteBatch, Sprites.TractorBeam, screenPosition, player.Angle, 1.0f, Color.Cyan);

            DrawPolygon(SpriteBatch, Sprites.circle, screenPosition, 0.0f, 30.0f, Color.White);
        }

        private void DrawPolygon(SpriteBatch spriteBatch, Polygon polygon, Vector2 position, float angle, float scale, Color color)
        {
            if(polygon.Closed)
                DrawLine(spriteBatch, position + polygon.PointAt(0, angle) * scale, position + polygon.PointAt(polygon.Points.Count - 1, angle) * scale, 2, color);

            for (int index = 1; index < polygon.Points.Count; index++)
                DrawLine(spriteBatch, position + polygon.PointAt(index - 1, angle) * scale, position + polygon.PointAt(index, angle) * scale, 2, color);
        }

        private void DrawLine(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float width, Color color)
        {
            float angle = (float)Math.Atan2(y2 - y1, x2 - x1);
            float length = (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

            spriteBatch.Draw(solid, new Rectangle((int)x1, (int)y1, (int)length, (int)width), null, color, angle, new Vector2(0, 0.5f), SpriteEffects.None, 1.0f);
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, float width, Color color)
        {
            DrawLine(spriteBatch, p1.X, p1.Y, p2.X, p2.Y, width, color);
        }

        private void DrawNumber(SpriteBatch spritebatch, int number, Vector2 position, Color color)
        {
            string numberString = number.ToString();

            if (number < 0)
            {
                DrawPolygon(spritebatch, Sprites.Minus, position + new Vector2(0, 0), 0.0f, 20.0f, color);

                for (int index = 1; index < numberString.Length; index++)
                    DrawPolygon(spritebatch, Sprites.Digits[numberString[index] - '0'], position + new Vector2(25, 0) * index, 0.0f, 20.0f, color);
            }
            else
            {
                for (int index = 0; index < numberString.Length; index++)
                    DrawPolygon(spritebatch, Sprites.Digits[numberString[index] - '0'], position + new Vector2(25, 0) * index, 0.0f, 20.0f, color);
            }
        }
    }
}
