using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3.Mason
{
    class ParticleEngine
    {
        private Random random;

        private List<ParticleEmmiter> particleEmmiters;

        public ParticleEngine()
        {
            random = new Random();
            particleEmmiters = new List<ParticleEmmiter>();
        }

        public void AddPointParticleEmmiter(Vector2 position, float layer, List<Texture2D> particleTextures, Color[] spawnColorRange, float[] spawnTransparencyRange, float[] spawnSizeRange, float[] spawnSpeedRange, float[] spawnSecondsToLiveRange, int particlesPerSecond, float secondsToLive, Color? endColor = null, float? endTransparency = null, float? endSize = null, float? endSpeed = null)
        {
            //endColor = Color.Yellow;
            //endTransparency = 0.25f;
            //endSize = 0f;
            //endSpeed = 1f;
            ParticleEmmiter pointEmmiter = new PointParticleEmmiter(particleTextures, position, layer, particlesPerSecond, secondsToLive, spawnColorRange, spawnTransparencyRange, spawnSizeRange, spawnSpeedRange, spawnSecondsToLiveRange, endColor, endTransparency, endSize, endSpeed);
            particleEmmiters.Add(pointEmmiter);
        }

        public void AddAreaParticleEmmiter(Vector2 position, float radius, float layer, List<Texture2D> particleTextures, Color[] spawnColorRange, float[] spawnTransparencyRange, float[] spawnSizeRange, float[] spawnSpeedRange, float[] spawnSecondsToLiveRange, int particlesPerSecond, float secondsToLive, Color? endColor = null, float? endTransparency = null, float? endSize = null, float? endSpeed = null)
        {
            ParticleEmmiter pointEmmiter = new AreaParticleEmmiter(particleTextures, position, layer, radius, particlesPerSecond, secondsToLive, spawnColorRange, spawnTransparencyRange, spawnSizeRange, spawnSpeedRange, spawnSecondsToLiveRange, endColor, endTransparency, endSize, endSpeed);
            particleEmmiters.Add(pointEmmiter);
        }

        public void Update(float elapsedTime)
        {
            List<ParticleEmmiter> particleEmmitersToRemove = new List<ParticleEmmiter>();

            foreach (ParticleEmmiter particleEmmiter in particleEmmiters)
            {
                particleEmmiter.Update(elapsedTime);

                if (particleEmmiter.SecondsToLive <= 0 && particleEmmiter.numberOfParticles == 0)
                {
                    particleEmmitersToRemove.Add(particleEmmiter);
                }
            }

            foreach (ParticleEmmiter particleEmmiterToRemove in particleEmmitersToRemove)
            {
                particleEmmiters.Remove(particleEmmiterToRemove);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ParticleEmmiter particleEmmiter in particleEmmiters)
            {
                particleEmmiter.Draw(spriteBatch);
            }
        }

        abstract class ParticleEmmiter
        {
            public int numberOfParticles { get; private set; }
            protected LinkedList<Particle> particles;
            protected static Random random = new Random();
            protected Vector2 Position;
            protected List<Texture2D> Textures;
            protected int ParticlesPerSecond;
            protected double AccumulatedTime;
            public float SecondsToLive { get; private set; }

            protected float DrawLayer;

            protected Color[] SpawnColorRange;
            protected float[] SpawnTransparencyRange;
            protected float[] SpawnSizeRange;
            protected float[] SpawnSpeedRange;
            protected float[] SpawnSecondsToLiveRange;

            protected Color? EndColor;
            protected float? EndTransparency;
            protected float? EndSize;
            protected float? EndSpeed;

            public ParticleEmmiter(List<Texture2D> textures, Vector2 position, float drawLayer, int particlesPerSecond, float secondsToLive, Color[] spawnColorRange, float[] spawnTransparencyRange, float[] spawnSizeRange, float[] spawnSpeedRange, float[] spawnSecondsToLiveRange, Color? endColor = null, float? endTransparency = null, float? endSize = null, float? endSpeed = null)
            {
                this.Textures = textures;
                this.Position = position;
                this.ParticlesPerSecond = particlesPerSecond;
                this.AccumulatedTime = 0f;
                this.SecondsToLive = secondsToLive;

                this.DrawLayer = drawLayer;

                this.SpawnColorRange = spawnColorRange;
                this.SpawnTransparencyRange = spawnTransparencyRange;
                this.SpawnSizeRange = spawnSizeRange;
                this.SpawnSpeedRange = spawnSpeedRange;
                this.SpawnSecondsToLiveRange = spawnSecondsToLiveRange;

                this.EndColor = endColor;
                this.EndTransparency = endTransparency;
                this.EndSize = endSize;
                this.EndSpeed = endSpeed;

                particles = new LinkedList<Particle>();
            }

            public void Update(float elapsedTime)
            {
                if (this.SecondsToLive > 0)
                {
                    float emmiterElapsedTime = elapsedTime;

                    if (this.SecondsToLive < elapsedTime)
                    {
                        emmiterElapsedTime = this.SecondsToLive;
                    }

                    this.SecondsToLive -= emmiterElapsedTime;

                    this.AccumulatedTime += emmiterElapsedTime;

                    int particlesToSpawn = (int)(ParticlesPerSecond * AccumulatedTime);

                    float timeWorthOfParticlesSpawned = particlesToSpawn / ParticlesPerSecond;

                    this.AccumulatedTime -= timeWorthOfParticlesSpawned;

                    for (int i = 0; i < particlesToSpawn; i++)
                    {
                        particles.AddLast(GenerateNewParticle());
                        numberOfParticles += 1;
                    }
                }

                LinkedListNode<Particle> runner = particles.First;
                while (runner != null)
                {
                    runner.Value.Update(elapsedTime);
                    LinkedListNode<Particle> nextRunner = runner.Next;
                    if (runner.Value.SecondsToLive <= 0)
                    {
                        particles.Remove(runner);
                        numberOfParticles -= 1;
                    }
                    runner = nextRunner;
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                foreach (Particle particle in particles)
                {
                    particle.Draw(spriteBatch);
                }
            }

            protected abstract Particle GenerateNewParticle();

            protected float GenerateRandomNumberInRange(float number1, float number2)
            {
                float lower = Math.Min(number1, number2);
                float upper = Math.Max(number1, number2);
                float result = lower + ((upper - lower) * (float)random.NextDouble());
                return result;
            }
        }

        class PointParticleEmmiter : ParticleEmmiter
        {
            public PointParticleEmmiter(List<Texture2D> textures, Vector2 position, float drawLayer, int particlesPerSecond, float secondsToLive, Color[] spawnColorRange, float[] spawnTransparencyRange, float[] spawnSizeRange, float[] spawnSpeedRange, float[] spawnSecondsToLiveRange, Color? endColor = null, float? endTransparency = null, float? endSize = null, float? endSpeed = null)
                                    : base(textures, position, drawLayer, particlesPerSecond, secondsToLive, spawnColorRange, spawnTransparencyRange, spawnSizeRange, spawnSpeedRange, spawnSecondsToLiveRange, endColor, endTransparency, endSize, endSpeed)
            {

            }

            protected override Particle GenerateNewParticle()
            {
                Texture2D texture = Textures[random.Next(Textures.Count)];
                Vector2 position = this.Position;
                float rotation = (float)(random.NextDouble() * Math.PI * 2f);
                //Vector2 velocity = new Vector2(
                //        1f * (float)(random.NextDouble() * 2 - 1),
                //        1f * (float)(random.NextDouble() * 2 - 1));
                Vector2 rotationDirection = Zak.Utils.VectorFrom(rotation);
                rotationDirection = new Vector2(rotationDirection.X, -rotationDirection.Y);
                Vector2 velocity = (rotationDirection) * GenerateRandomNumberInRange(SpawnSpeedRange[0], SpawnSpeedRange[1]);
                float angle = 0;
                float angularVelocity = (float)Math.PI * 2f * ((float)random.NextDouble() + 1f) * ((-2 * random.Next(2)) + 1);
                //Color color = new Color(
                //        (float)random.NextDouble(),
                //        (float)random.NextDouble(),
                //        (float)random.NextDouble()) * (float) random.NextDouble();
                Color color = new Color((int)GenerateRandomNumberInRange(SpawnColorRange[0].R, SpawnColorRange[1].R), (int)GenerateRandomNumberInRange(SpawnColorRange[0].G, SpawnColorRange[1].G), (int)GenerateRandomNumberInRange(SpawnColorRange[0].B, SpawnColorRange[1].B));
                float transparency = GenerateRandomNumberInRange(SpawnTransparencyRange[0], SpawnTransparencyRange[1]);
                float size = GenerateRandomNumberInRange(SpawnSizeRange[0], SpawnSizeRange[1]);
                float ttl = GenerateRandomNumberInRange(SpawnSecondsToLiveRange[0], SpawnSecondsToLiveRange[1]);
                float layerOffset = (float)random.NextDouble() * 0.001f;

                return new Particle(texture, position, velocity, angle, angularVelocity, color, transparency, size, ttl, this.DrawLayer + layerOffset, EndColor, EndTransparency, EndSize, EndSpeed);
            }
        }

        class AreaParticleEmmiter : ParticleEmmiter
        {
            private float Radius;

            public AreaParticleEmmiter(List<Texture2D> textures, Vector2 position, float drawLayer, float radius, int particlesPerSecond, float secondsToLive, Color[] spawnColorRange, float[] spawnTransparencyRange, float[] spawnSizeRange, float[] spawnSpeedRange, float[] spawnSecondsToLiveRange, Color? endColor = null, float? endTransparency = null, float? endSize = null, float? endSpeed = null)
                                    : base(textures, position, drawLayer, particlesPerSecond, secondsToLive, spawnColorRange, spawnTransparencyRange, spawnSizeRange, spawnSpeedRange, spawnSecondsToLiveRange, endColor, endTransparency, endSize, endSpeed)
            {
                this.Radius = radius;
            }

            protected override Particle GenerateNewParticle()
            {
                Texture2D texture = Textures[random.Next(Textures.Count)];


                //Vector2 position = new Vector2(this.Position.X + Radius * (float)Math.Cos(cirlceAngle), this.Position.Y + Radius * (float)Math.Sin(cirlceAngle));

                float rotation = (float)(random.NextDouble() * Math.PI * 2f);

                Vector2 rotationDirection = Zak.Utils.VectorFrom(rotation);
                rotationDirection = new Vector2(rotationDirection.X, -rotationDirection.Y);

                Vector2 velocity = rotationDirection * GenerateRandomNumberInRange(SpawnSpeedRange[0], SpawnSpeedRange[1]);
                float angle = 0;
                //float angularVelocity = (float)Math.PI * 2f * ((float)random.NextDouble() + 1f);
                float angularVelocity = 0f;

                Color color = new Color((int)GenerateRandomNumberInRange(SpawnColorRange[0].R, SpawnColorRange[1].R), (int)GenerateRandomNumberInRange(SpawnColorRange[0].G, SpawnColorRange[1].G), (int)GenerateRandomNumberInRange(SpawnColorRange[0].B, SpawnColorRange[1].B));
                float transparency = GenerateRandomNumberInRange(SpawnTransparencyRange[0], SpawnTransparencyRange[1]);
                float size = GenerateRandomNumberInRange(SpawnSizeRange[0], SpawnSizeRange[1]);

                float circleAngle = (float)(random.NextDouble() * Math.PI * 2f);

                Vector2 circleDirection = Zak.Utils.VectorFrom(circleAngle);
                circleDirection = new Vector2(rotationDirection.X, -rotationDirection.Y);

                float distance = GenerateRandomNumberInRange(0f, Radius - (size * 0.5f)) * (float)(Math.Pow(GenerateRandomNumberInRange(0f, 1f), 2));
                Vector2 position = this.Position + (circleDirection * distance);


                float ttl = GenerateRandomNumberInRange(SpawnSecondsToLiveRange[0], SpawnSecondsToLiveRange[1]);
                float layerOffset = (float)random.NextDouble() * 0.001f;

                return new Particle(texture, position, velocity, angle, angularVelocity, color, transparency, size, ttl, this.DrawLayer + layerOffset, EndColor, EndTransparency, EndSize, EndSpeed);
            }
        }

        class Particle
        {
            public Texture2D Texture;
            public Vector2 Position;
            public Vector2 Velocity;
            private Vector2 StartingVelocity;
            public float Angle;
            public float AngularVelocity;
            public Color Color;
            private Color StartingColor;
            public float Transparency;
            private float StartingTransparency;
            public float Size;
            private float StartingSize;
            public float SecondsToLive;
            public float StartingSecondsToLive;
            public float DrawLayer;

            Color? endColor;
            float? endTransparency;
            float? endSize;
            float? endSpeed;

            public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float transparency, float size, float secondsToLive, float drawLayer, Color? endColor = null, float? endTransparency = null, float? endSize = null, float? endSpeed = null)
            {
                this.Texture = texture;
                this.Position = position;
                this.Velocity = velocity;
                this.StartingVelocity = velocity;
                this.Angle = angle;
                this.AngularVelocity = angularVelocity;
                this.Color = color;
                this.StartingColor = color;
                this.Transparency = transparency;
                this.StartingTransparency = transparency;
                this.Size = size;
                this.StartingSize = size;
                this.StartingSecondsToLive = secondsToLive;
                this.SecondsToLive = secondsToLive;
                this.DrawLayer = drawLayer;

                if (endColor != null)
                    this.endColor = endColor.Value;

                if (endTransparency != null)
                    this.endTransparency = endTransparency.Value;

                if (endSize != null)
                    this.endSize = endSize.Value;

                if (endSpeed != null)
                    this.endSpeed = endSpeed.Value;
            }

            public void Update(float elapsedTime)
            {
                if (this.SecondsToLive < elapsedTime)
                {
                    elapsedTime = this.SecondsToLive;
                }

                this.SecondsToLive -= elapsedTime;

                if (this.SecondsToLive < 0)
                {
                    this.SecondsToLive = 0;
                }

                if (endSpeed != null)
                {
                    float speed = GetProgress(this.StartingVelocity.Length(), this.endSpeed.Value);
                    if (this.Velocity != Vector2.Zero)
                    {
                        this.Velocity.Normalize();
                    }
                    this.Velocity = speed * this.Velocity;
                }

                if (endColor != null)
                {
                    this.Color = new Color((int)GetProgress(this.StartingColor.R, this.endColor.Value.R), (int)GetProgress(this.StartingColor.G, this.endColor.Value.G), (int)GetProgress(this.StartingColor.B, this.endColor.Value.B));
                }

                if (endSize != null)
                {
                    this.Size = GetProgress(this.StartingSize, this.endSize.Value);
                }

                if (endTransparency != null)
                {
                    this.Transparency = GetProgress(this.StartingTransparency, this.endTransparency.Value);
                }


                this.Position += Velocity * elapsedTime;
                this.Angle += AngularVelocity * elapsedTime;
            }

            private float GetProgress(float start, float destination)
            {
                float progress = start;
                float percentage = (float)Math.Pow(1f - (this.SecondsToLive / this.StartingSecondsToLive), 2);

                if (start < destination)
                {
                    progress = start + ((destination - start) * percentage);
                }
                else if (destination < start)
                {
                    progress = start - ((start - destination) * percentage);
                }
                else
                {
                    progress = destination;
                }

                return progress;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, BroccoliAsteroidsGameState.WorldToScreenCoordinates(Position), null, Color * this.Transparency, Angle, new Vector2(Texture.Width, Texture.Height) / 2f, BroccoliAsteroidsGameState.WorldToScreenSize(Size) / Texture.Width, SpriteEffects.None, Math.Min(DrawLayer, 1f));
            }
        }
    }
}
