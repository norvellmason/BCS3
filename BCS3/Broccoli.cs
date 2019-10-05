using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3
{

    class Broccoli
    {
        private bool hasBeenConsumed;
        private float[] position;
        private float speed;
        public bool isOutOfBounds;

        private int maxLength;
        private int maxHeight;

        public Broccoli(int maxLength)
        {
            this.hasBeenConsumed = false;
            this.isOutOfBounds = false;
            this.maxHeight = maxHeight;
            this.maxLength = maxLength;

            Random r = new Random();
            position = new float[] {0f, r.Next(0, maxLength)};
            speed = r.Next(0, 5);

            Console.WriteLine("Created Broccoli at " + position[0] + ", " + position[1]);
        }

        public void consume()
        {
            Console.WriteLine("num num");
            this.hasBeenConsumed = true;
        }

        public void update()
        {
            position[1] += speed;

            if (position[1] >= maxLength)
            {
                this.isOutOfBounds = true;
            }
        }
    }
}
