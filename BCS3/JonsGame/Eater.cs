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
    class Eater
    {
        public Vector2 position;
        public Vector2 direction;
        public float faceDirection;
        public Vector2 imageSize;
        int MaxHeight;
        int MaxLength;

        public int Weight;
        int EAT_RADIUS = 80;

        float MOVE_FACTOR = .1f;

        int MOVE_LIMIT = 10;


        public Eater(int MaxHeight, int MaxLength, float sizex, float sizey)
        {
            this.MaxHeight = MaxHeight;
            this.MaxLength = MaxLength;
            this.position = new Vector2(MaxLength / 2, MaxHeight / 2);
            this.direction = new Vector2(0, 0);
            this.imageSize = new Vector2(sizex, sizey);

            this.Weight = 10;

            this.faceDirection = 0f;
        }

        public void MoveLeft()
        {
            this.direction.X -= MOVE_FACTOR;
            if (this.direction.Length() > 1)
                this.direction = Vector2.Normalize(this.direction);
            this.faceDirection = AngleOf(this.direction);

        }

        public void MoveRight()
        {
            this.direction.X += MOVE_FACTOR;
            if (this.direction.Length() > 1)
                this.direction = Vector2.Normalize(this.direction);
            this.faceDirection = AngleOf(this.direction);

        }
        public void MoveUp()
        {
            this.direction.Y -= MOVE_FACTOR;
            if (this.direction.Length() > 1)
                this.direction = Vector2.Normalize(this.direction);
            this.faceDirection = AngleOf(this.direction);

        }

        public void MoveDown()
        {

            this.direction.Y += MOVE_FACTOR;
            if (this.direction.Length() > 1)
                this.direction = Vector2.Normalize(this.direction);
            this.faceDirection = AngleOf(this.direction);

        }

        public static float AngleOf(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X) + (float) Math.PI / 2;
        }


        public bool canEat(Eatable candidate)
        {
            return Math.Abs(this.position.X - candidate.position.X) < EAT_RADIUS &&
               Math.Abs(this.position.Y - candidate.position.Y) < EAT_RADIUS;
        }

        private float speed()
        {
            return 50 / this.Weight;
        }

        public void Update()
        {
            //this.direction = Vector2.Normalize(this.direction);
            this.position.X += (this.direction.X * speed());
            this.position.Y += (this.direction.Y * speed());

            //Wrap around
            if (this.position.X > this.MaxLength) {
                this.position.X = 0;
            }
            if (this.position.X < 0)
            {
                this.position.X = this.MaxLength;
            }

            if (this.position.Y > this.MaxHeight)
            {
                this.position.Y = 0;
            }

            if (this.position.Y < 0)
            {
                this.position.Y = this.MaxHeight;
            }

        }

        public float getSize()
        {
            return .01f * this.Weight;
        }
    }
}
