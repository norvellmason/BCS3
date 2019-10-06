using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3.Zak
{
    public class DemoLevel
    {
        public int Number { get; private set; } = 0;

        public float EnemyChance
        {
            get
            {
                if (Number == 1)
                    return 0.2f;

                if (Number == 2)
                    return 0.4f;

                return 0.6f;
            }
        }
        public float BroccoliChance
        {
            get
            {
                if (Number == 1)
                    return 0.2f;

                if(Number == 2)
                    return 0.3f;

                return 0.1f;
            }
        }

        public float QuuenChance
        {
            get
            {
                if (Number == 1)
                    return 0.0f;

                if (Number == 2)
                    return 0.3f;

                return 0.6f;
            }
        }

        public float BruteChance
        {
            get
            {
                if (Number == 1)
                    return 0.3f;

                if (Number == 2)
                    return 0.5f;

                return 0.6f;
            }
        }

        public int MaxCruisers
        {
            get
            {
                return Number - 1;
            }
        }

        public float CruiserDeployTime
        {
            get
            {
                if (Number == 2)
                    return 15;

                if (Number == 3)
                    return 10;

                return 100;
            }
        }

        public float DreadnaughtChance
        {
            get
            {
                if (Number == 3)
                    return 0.5f;

                return 0.0f;
            }
        }

        public float GroundRandomness
        {
            get
            {
                if (Number == 1)
                    return 100;

                if (Number == 2)
                    return 150;

                return 200;
            }
        }

        public bool HasPlatform
        {
            get
            {
                return Number >= 2;
            }
        }

        public void NextLevel()
        {
            Number += 1;
        }

        public int LastLevel
        {
            get
            {
                return 3;
            }
        }
    }
}
