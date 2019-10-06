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
    class Cookie : Eatable
    {

        public Cookie (int MaxLength, int MaxHeight, int PlayerX, int PlayerY) : base(MaxHeight, MaxLength, PlayerX, PlayerY){
            this.speed = rng.Next(1, 2);
            this.rotationRate = 0.01f * this.speed;
        }
    }
}
