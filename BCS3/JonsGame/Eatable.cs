using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3.JonsGame
{
    abstract class Eatable
    {
        private bool hasBeenConsumed;
        public Vector2 position;
        public Vector2 dicrection;

        public float speed;
        public bool isOutOfBounds;
        public float rotation;
        public float rotationRate;

        private int MaxLength;
        private int MaxHeight;

        private int PlayerX;
        private int PlayerY;
        public Random rng;

        public Eatable(int MaxHeight, int MaxLength, int PlayerX, int PlayerY)
        {
            this.hasBeenConsumed = false;
            this.isOutOfBounds = false;
            this.MaxHeight = MaxHeight;
            this.MaxLength = MaxLength;

            this.PlayerX = PlayerX;
            this.PlayerY = PlayerY;


            this.rng = new Random();

            int startX = 0;
            int startY = 0;
            int offset = 20;
            int startDecider = rng.Next(-1, 4);

            if(startDecider == 0){
                startY = rng.Next(offset, MaxHeight - offset);
                startX = MaxLength - offset;
            } else if(startDecider == 1) {
                startY = rng.Next(offset, MaxHeight - offset);
                startX = offset;
            } else if(startDecider == 2){
                startY = MaxHeight - offset;
                startX = rng.Next(offset, MaxLength - offset);
            } else if(startDecider == 3){
                startY = offset;
                startX = rng.Next(offset, MaxLength - offset);
            }

            position = new Vector2(startX, startY);
            speed = rng.Next(1, 9);
            this.rotation = rng.Next(-5, 5);
            this.rotationRate = 0.01f * this.speed;

            double dx = rng.Next(-10, 10);
            double dy = rng.Next(-10, 10);

            double magnitude = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

            this.dicrection = new Vector2((float)(dx / magnitude), (float)(dy / magnitude));
        }

        public void Update(Vector2 PlayerPosition, int PlayerWeight){


            float distanceFactor = (.1f * PlayerWeight) / ((PlayerPosition - this.position).Length() * 75);

            Vector2 toPlayer = (PlayerPosition - this.position);

            this.dicrection.X += (toPlayer.X * distanceFactor);
            this.dicrection.Y += (toPlayer.Y * distanceFactor);

            this.position.Y += this.dicrection.Y * this.speed;
            this.position.X += this.dicrection.X * this.speed;
            this.rotation += rotationRate;

            if (position.Y >= MaxHeight + 200 || position.Y < -200 || position.X < -200 || position.X >= MaxLength + 200)
            {
                this.isOutOfBounds = true;
            }
        }
    }
}
