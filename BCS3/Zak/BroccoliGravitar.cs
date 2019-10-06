using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BCS_3.Zak
{
    public class BroccoliGravitar : GameState
    {
        private SoundEffect shoot;
        private SoundEffect bigShoot;
        private SoundEffect[] explosion;
        private SoundEffect zoomOut;
        private SoundEffect cruiserEnter;
        private SoundEffect pickup;
        private SoundEffect sizzle;

        private SoundEffect thrust;
        private SoundEffectInstance thrustInstance;
        private SoundEffect smallThrust;
        private SoundEffectInstance smallThrustInstance;

        private Random random = new Random();

        private Texture2D solid;

        private List<Planet> planets = new List<Planet>();

        private GravitarPlayer player = new GravitarPlayer(new Vector2(300, 300));
        private int score = 0;

        private float scale = 0.1f;
        private float goalScale = 0.9f;

        private DemoLevel level = new DemoLevel();
        private bool exiting = false;

        private float currentDeplyTime = 10.0f;

        private List<GravitarBullet> bullets = new List<GravitarBullet>();
        private List<GravitarBroccoli> broccolis = new List<GravitarBroccoli>();
        private List<GravitarEnemy> enemies = new List<GravitarEnemy>();
        private List<SungCruiser> cruisers = new List<SungCruiser>();
        private List<Particle> particles = new List<Particle>();

        private float screenMargin = 30;

        public bool Victory
        {
            get
            {
                return broccolis.Count == 0 && enemies.Count == 0 && cruisers.Count == 0;
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

            shoot = content.Load<SoundEffect>("Zak/sounds/shoot");
            bigShoot = content.Load<SoundEffect>("Zak/sounds/big_shoot");
            explosion = new SoundEffect[] {
                content.Load<SoundEffect>("Zak/sounds/explode"),
                content.Load<SoundEffect>("Zak/sounds/explode2"),
                content.Load<SoundEffect>("Zak/sounds/explode3")
            };
            zoomOut = content.Load<SoundEffect>("Zak/sounds/zoom_out");
            cruiserEnter = content.Load<SoundEffect>("Zak/sounds/enter");
            pickup = content.Load<SoundEffect>("Zak/sounds/pickup");
            sizzle = content.Load<SoundEffect>("Zak/sounds/sizzle");

            thrust = content.Load<SoundEffect>("Zak/sounds/thrust");
            thrustInstance = thrust.CreateInstance();
            thrustInstance.IsLooped = true;
            thrustInstance.Play();
            thrustInstance.Pause();

            smallThrust = content.Load<SoundEffect>("Zak/sounds/thrust_small");
            smallThrustInstance = smallThrust.CreateInstance();
            smallThrustInstance.IsLooped = true;
            smallThrustInstance.Volume = 0.5f;
            smallThrustInstance.Play();
            smallThrustInstance.Pause();
        }

        private void KillPlayer()
        {
            player.Alive = false;
            AddParticles(player.Position, 10, Color.White);

            foreach (SungCruiser cruiser in cruisers)
                cruiser.Behaviour = CruiserBehaviour.Leaving;

            explosion[Utils.random.Next(explosion.Length)].Play();

            score -= 150;
        }

        private void SpawnPlayer()
        {
            float angle = (float)random.NextDouble() * 2 * (float)Math.PI;
            float x = (float)Math.Cos(angle) * 800.0f;
            float y = (float)Math.Sin(angle) * 800.0f;

            player.Alive = true;
            player.Position = new Vector2(x, y);
            player.Velocity = new Vector2(0, 0);

            foreach (GravitarEnemy enemy in enemies)
                enemy.firingTimer = 5.0f + 5.0f * (float)random.NextDouble();
        }

        private void NextLevel()
        {
            level.NextLevel();

            planets.Clear();

            planets.Add(new Planet(Vector2.Zero, GetPlanetPolygon()) { Angle = (float)Utils.random.NextDouble() * 2 * (float)Math.PI });

            if (level.HasPlatform)
                planets.Add(new Planet(Vector2.Zero, GetPlatformPolygon()));

            foreach (Planet planet in planets)
            {
                for (int segmentIndex = 0; segmentIndex < planet.Polygon.Points.Count; segmentIndex++)
                {
                    float chance = (float)Utils.random.NextDouble();

                    if (chance < level.EnemyChance)
                    {
                        EnemyType type = EnemyType.Standard;
                        if (Utils.random.NextDouble() < level.BruteChance)
                            type = EnemyType.SungBrute;

                        if (Utils.random.NextDouble() < level.QuuenChance)
                            type = EnemyType.Queen;

                        enemies.Add(new GravitarEnemy(type, planet, segmentIndex, (float)random.NextDouble()));
                    }
                    else if (chance < level.EnemyChance + level.BroccoliChance)
                    {
                        broccolis.Add(new GravitarBroccoli(planet, segmentIndex, (float)random.NextDouble()));
                    }
                }
            }

            currentDeplyTime = level.CruiserDeployTime;

            exiting = false;
            SpawnPlayer();
        }

        private Polygon GetPlanetPolygon()
        {
            int pointCount = 30;
            Vector2[] points = new Vector2[pointCount];
            Random random = new Random();

            for (int index = 0; index < pointCount; index++)
            {
                float angle = index * (float)Math.PI * 2 / pointCount;
                float distance = 200 + (float)random.NextDouble() * level.GroundRandomness;

                points[index] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
            }

            return new Polygon(points);
        }

        private Polygon GetPlatformPolygon()
        {
            float startRange = 0;
            float endRange = (float)Math.PI / 2;
            int pointCount = 10;

            Vector2[] points = new Vector2[pointCount * 2];
            for (int index = 0; index < pointCount; index++)
            {
                float percent = (float)index / (float)(pointCount - 1);
                float angle = startRange + (endRange - startRange) * percent;
                float distance = 400 + level.GroundRandomness + (float)Utils.random.NextDouble() * 30;

                points[index] = Utils.VectorFrom(angle, distance);
            }

            for (int index = 0; index < pointCount; index++)
            {
                float percent = (float)index / (float)(pointCount - 1);
                float angle = endRange + (startRange - endRange) * percent;
                float distance = 500 + level.GroundRandomness + (float)Utils.random.NextDouble() * 30;

                points[pointCount + index] = Utils.VectorFrom(angle, distance);
            }

            return new Polygon(points);
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

        private bool OnScreen(Vector2 point, Viewport viewport, float margin)
        {
            if (point.X * scale < -viewport.Width / 2 + margin || point.X * scale > viewport.Width / 2 - margin)
                return false;

            if (point.Y * scale < -viewport.Height / 2 + margin || point.Y * scale > viewport.Height / 2 - margin)
                return false;

            return true;
        }

        private Vector2 GetOffscreenCoordinate(Viewport viewport)
        {
            if(Utils.random.NextDouble() < 0.5f)
            {
                float x = viewport.Width * (float)Utils.random.NextDouble() - viewport.Width / 2;
                float y = Utils.random.NextDouble() < 0.5 ? -viewport.Height / 2 : viewport.Height / 2;

                return new Vector2(x, y);
            }
            else
            {
                float x = Utils.random.NextDouble() < 0.5 ? -viewport.Width / 2 : viewport.Width / 2;
                float y = viewport.Height * (float)Utils.random.NextDouble() - viewport.Height / 2;

                return new Vector2(x, y);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Victory)
            {
                goalScale = 0.0f;

                if(!exiting)
                {
                    zoomOut.Play();
                    exiting = true;
                }
            }
            else if (!player.Alive)
            {
                goalScale = Math.Min(0.4f * SpriteBatch.GraphicsDevice.Viewport.Height / 900.0f, 1.0f);
            }
            else
            {
                goalScale = Math.Min(0.6f * SpriteBatch.GraphicsDevice.Viewport.Height / 900.0f, 1.0f);
            }

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

            if (currentDeplyTime <= 0 && !Victory)
            {
                Vector2 position = GetOffscreenCoordinate(SpriteBatch.GraphicsDevice.Viewport);
                CruiserType type = CruiserType.Cruiser;
                if (Utils.random.NextDouble() < level.DreadnaughtChance)
                    type = CruiserType.Dreadnaught;

                SungCruiser cruiser = new SungCruiser(position, 400.0f + 100 * (float)Utils.random.NextDouble(), type)
                {
                    Angle = Utils.AngleOf(position),
                    Clockwise = Utils.random.NextDouble() < 0.5
                };

                cruiserEnter.Play();
                cruisers.Add(cruiser);
                currentDeplyTime = level.CruiserDeployTime;
            }
            else if(cruisers.Count < level.MaxCruisers)
            {
                currentDeplyTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            foreach(SungCruiser cruiser in cruisers)
            {
                if(cruiser.Behaviour == CruiserBehaviour.Entering)
                {
                    cruiser.Position += Vector2.Normalize(-cruiser.Position) * cruiser.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (cruiser.Position.Length() <= cruiser.OrbitHeight)
                        cruiser.Behaviour = CruiserBehaviour.Orbiting;
                }
                else if(cruiser.Behaviour == CruiserBehaviour.Orbiting)
                {
                    cruiser.Angle += cruiser.OrbitSpeed / cruiser.OrbitHeight * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    cruiser.Position = Utils.VectorFrom(cruiser.Angle, cruiser.OrbitHeight);
                }
                else if(cruiser.Behaviour == CruiserBehaviour.Leaving)
                {
                    cruiser.Position -= Vector2.Normalize(-cruiser.Position) * cruiser.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (!OnScreen(cruiser.Position, SpriteBatch.GraphicsDevice.Viewport, -100))
                        cruiser.Remove = true;
                }

                if (cruiser.FiringTimer <= 0)
                {
                    if (player.Alive && cruiser.Behaviour != CruiserBehaviour.Leaving)
                    {
                        Vector2 playerOffset = player.Position - cruiser.Position;

                        Vector2 targetPosition = player.Position;
                        if (cruiser.Type == CruiserType.Dreadnaught)
                            targetPosition += player.Velocity * playerOffset.Length() / 300.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                        bullets.Add(new GravitarBullet(cruiser.Position, Vector2.Normalize(targetPosition - cruiser.Position) * 700, false));
                        cruiser.FiringTimer = cruiser.FiringDelay + (float)random.NextDouble() * 0.3f;
                        shoot.Play();
                    }
                }
                else
                {
                    cruiser.FiringTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
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
                    switch(enemy.Type) {
                        case EnemyType.Standard:
                            bullets.Add(new GravitarBullet(enemy.Position, Vector2.Normalize(player.Position - enemy.Position) * 300, false));
                            enemy.firingTimer = enemy.firingDelay + (float)random.NextDouble() * 0.3f;
                            shoot.Play();

                            break;

                        case EnemyType.SungBrute:
                            float angle = Utils.AngleOf(player.Position - enemy.Position);

                            bullets.Add(new GravitarBullet(enemy.Position, Utils.VectorFrom(angle - 0.15f, 300), false));
                            bullets.Add(new GravitarBullet(enemy.Position, Utils.VectorFrom(angle, 300), false));
                            bullets.Add(new GravitarBullet(enemy.Position, Utils.VectorFrom(angle + 0.15f, 300), false));

                            enemy.firingTimer = enemy.firingDelay + (float)random.NextDouble() * 0.3f;
                            bigShoot.Play();

                            break;

                        case EnemyType.Queen:
                            Vector2 playerOffset = player.Position - enemy.Position;
                            Vector2 targetPosition = player.Position + player.Velocity * playerOffset.Length() / 300.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                            bullets.Add(new GravitarBullet(enemy.Position, Vector2.Normalize(targetPosition - enemy.Position) * 300, false));

                            enemy.firingTimer = enemy.firingDelay + (float)random.NextDouble() * 0.3f;
                            shoot.Play();

                            break;
                    }
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

                        pickup.Play();
                        AddParticles(broccoli.Position, 10, Color.Green);
                    }
                }
            }

            if (particles.Any(particle => particle.Remove))
                sizzle.Play(0.2f, 0.0f, 0.0f);

            bullets.RemoveAll(bullet => bullet.Remove);
            broccolis.RemoveAll(broccoli => broccoli.Remove);
            enemies.RemoveAll(enemy => enemy.Remove);
            cruisers.RemoveAll(cruiser => cruiser.Remove);
            particles.RemoveAll(particle => particle.Remove);

            foreach(Planet planet in planets)
                planet.Angle += 0.001f;
        }

        private void UpdatePlayer(float timePassed)
        {
            if (!player.Alive && player.DeadTime > 3.0f)
                SpawnPlayer();

            KeyboardState keyboardState = Keyboard.GetState();
            player.Control(keyboardState);
            player.Update(timePassed);

            if (player.Thrusting && player.Alive && thrustInstance.State != SoundState.Playing)
                thrustInstance.Resume();
            if ((!player.Thrusting || !player.Alive) && thrustInstance.State != SoundState.Paused)
                thrustInstance.Pause();

            if ((player.TurningLeft || player.TurningRight) && player.Alive && smallThrustInstance.State != SoundState.Playing)
                smallThrustInstance.Resume();
            if (((!player.TurningLeft && !player.TurningRight) || !player.Alive) && smallThrustInstance.State != SoundState.Paused)
                smallThrustInstance.Pause();

            if (player.Alive && !Victory)
            {
                Vector2 offset = Vector2.Zero - player.Position;
                player.Velocity += Vector2.Normalize(offset) * (5000000 / offset.LengthSquared()) * timePassed;

                player.Position += player.Velocity * timePassed;

                if (player.Firing && player.firingTimer <= 0)
                {
                    bullets.Add(new GravitarBullet(player.Position + Utils.VectorFrom(player.Angle, 20), player.Velocity + Utils.VectorFrom(player.Angle, 400), true));
                    player.firingTimer = player.firingDelay;

                    shoot.Play();
                }

                // collide with asteroid
                foreach(Planet planet in planets)
                    if (planet.Polygon.ContainsPoint(planet.Position, planet.Angle, player.Position))
                        KillPlayer();
            }
        }

        private void UpdateBullets(float timePassed)
        {
            foreach (GravitarBullet bullet in bullets)
            {
                bullet.Update(timePassed);
                bullet.Position += bullet.Velocity * timePassed;

                foreach(Planet planet in planets)
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

                            explosion[Utils.random.Next(explosion.Length)].Play();
                            AddParticles(enemy.Position, 10, Color.Red);
                        }
                    }

                    foreach (SungCruiser cruiser in cruisers)
                    {
                        if (Sprites.Sungdroid.ContainsPoint(cruiser.Position, cruiser.Angle, bullet.Position))
                        {
                            score += 50;

                            bullet.Remove = true;
                            cruiser.Remove = true;

                            explosion[Utils.random.Next(explosion.Length)].Play();
                            AddParticles(cruiser.Position, 10, Color.Red);
                        }
                    }
                }

                if (!OnScreen(bullet.Position, SpriteBatch.GraphicsDevice.Viewport, -100))
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

            foreach(Planet planet in planets)
                DrawPolygon(SpriteBatch, planet.Polygon, planet.Position, planet.Angle, 1.0f, Color.Blue);

            if(OnScreen(player.Position, SpriteBatch.GraphicsDevice.Viewport, screenMargin))
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
                else if (enemy.Type == EnemyType.SungBrute)
                    polygon = Sprites.SungBrute;

                DrawPolygon(SpriteBatch, polygon, enemy.Position, enemy.Angle, 1.0f, Color.Red);
            }

            foreach (SungCruiser cruiser in cruisers)
            {
                Polygon polygon = Sprites.SungCruiser;
                if (cruiser.Type == CruiserType.Dreadnaught)
                    polygon = Sprites.SungDreadnaught;

                DrawPolygon(SpriteBatch, polygon, cruiser.Position, 0.0f, 1.0f, Color.Red);
            }

            foreach (Particle particle in particles)
                DrawPolygon(SpriteBatch, Sprites.Paticle, particle.Position, particle.Angle, particle.Scale, particle.Color);

            SpriteBatch.End();

            SpriteBatch.Begin();

            float screenScale = SpriteBatch.GraphicsDevice.Viewport.Height / 900.0f;

            DrawNumber(SpriteBatch, score, new Vector2(30, 30) * screenScale, screenScale, Color.White);
            DrawNumber(SpriteBatch, level.Number, new Vector2(SpriteBatch.GraphicsDevice.Viewport.Width - 50 * screenScale, 30 * screenScale) - new Vector2(30, 0) * (Utils.DigitsToRepresent(level.Number) - 1) * screenScale, screenScale, Color.White);
            if (!OnScreen(player.Position, SpriteBatch.GraphicsDevice.Viewport, screenMargin))
                DrawPlayerHUD();

            SpriteBatch.End();
        }

        private void DrawPlayer()
        {
            if (player.Alive)
            {
                DrawPolygon(SpriteBatch, Sprites.PlayerShip, player.Position, player.Angle, 1.0f, Color.White);

                if (player.Thrusting && random.NextDouble() < 0.5)
                    DrawPolygon(SpriteBatch, Sprites.PlayerThrust[Utils.random.Next(Sprites.PlayerThrust.Length)], player.Position, player.Angle, 1.0f, Color.Orange);

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
                DrawPolygon(SpriteBatch, Sprites.PlayerThrust[Utils.random.Next(Sprites.PlayerThrust.Length)], screenPosition, player.Angle, 1.0f, Color.Orange);

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
                DrawLine(spriteBatch, position + polygon.PointAt(0, angle) * scale, position + polygon.PointAt(polygon.Points.Count - 1, angle) * scale, 4, color);

            for (int index = 1; index < polygon.Points.Count; index++)
                DrawLine(spriteBatch, position + polygon.PointAt(index - 1, angle) * scale, position + polygon.PointAt(index, angle) * scale, 4, color);
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

        private void DrawNumber(SpriteBatch spritebatch, int number, Vector2 position, float scale, Color color)
        {
            string numberString = number.ToString();

            if (number < 0)
            {
                DrawPolygon(spritebatch, Sprites.Minus, position + new Vector2(0, 0), 0.0f, 20.0f * scale, color);

                for (int index = 1; index < numberString.Length; index++)
                    DrawPolygon(spritebatch, Sprites.Digits[numberString[index] - '0'], position + new Vector2(25, 0) * index * scale, 0.0f, 20.0f * scale, color);
            }
            else
            {
                for (int index = 0; index < numberString.Length; index++)
                    DrawPolygon(spritebatch, Sprites.Digits[numberString[index] - '0'], position + new Vector2(25, 0) * index * scale, 0.0f, 20.0f * scale, color);
            }
        }
    }
}
