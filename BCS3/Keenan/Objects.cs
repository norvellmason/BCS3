using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3.Keenan
{
    public class Objects
    {
        public Objects(Texture2D image)
        {
            this.image = image;
        }
        public Texture2D image { get; private set; }
    }
}
