using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3.Keenan
{
    class Broccoli_Keenan : Objects
    {
        public Broccoli_Keenan(Texture2D image) : base(image)
        {
            //this.hitbox = new Rectangle(new Point((int)coordinate.X, -40), new Point((int)coordinate.X, 0));
            //this.position = coordinate;

            this.image = image;
        }

        //Stores position obviously...
        public Vector2 position;

        public Rectangle hitbox { get; private set; }

        public Texture2D image { get; private set; }
    }
}
