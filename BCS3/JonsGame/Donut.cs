using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3.JonsGame
{
    class Donut : Eatable
    {
        public Donut(int MaxHeight, int MaxLength, int PlayerX, int PlayerY) : base(MaxHeight, MaxLength, PlayerX, PlayerY)
        {
            this.speed = rng.Next(1, 3);
            this.rotationRate = 0.01f * this.speed;
        }
    }
}
