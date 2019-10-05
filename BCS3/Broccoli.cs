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

        public Broccoli()
        {
            this.hasBeenConsumed = false;
        }

        public void consume()
        {
            Console.WriteLine("num num");
            this.hasBeenConsumed = true;
        }
    }
}
