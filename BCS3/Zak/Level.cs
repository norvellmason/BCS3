using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3.Zak
{
    public class Level
    {
        public int Number { get; private set; } = 0;

        public float EnemyChance
        {
            get
            {
                if (Number <= 1)
                    return 0.1f;

                if(Number <= 2)
                    return 0.3f;

                if (Number <= 4)
                    return 0.5f;

                return 0.7f;
            }
        }
        public float BroccoliChance
        {
            get
            {
                if (Number <= 1)
                    return 0.2f;

                if(Number <= 2)
                    return 0.3f;

                if (Number <= 4)
                    return 0.2f;

                return 0.1f;
            }
        }

        public float QuuenChance
        {
            get
            {
                if (Number <= 2)
                    return 0.0f;

                if(Number <= 11)
                {
                    return (Number - 2) / 10.0f;
                }

                return 1.0f;
            }
        }

        public float BruteChance
        {
            get
            {
                if (Number <= 2)
                    return 0;

                if(Number <= 5)
                    return 0.2f;

                if (Number <= 10)
                    return 0.5f;

                return 0.9f;
            }
        }

        public int MaxCruisers
        {
            get
            {
                return (Number - 1) / 2;
            }
        }

        public float CruiserDeployTime
        {
            get
            {
                return Math.Max(21 - Number, 10);
            }
        }

        public float DreadnaughtChance
        {
            get
            {
                if(Number <= 3)
                    return 0.0f;

                if(Number <= 8)
                    return 0.2f;

                return 0.5f;
            }
        }

        public float GroundRandomness
        {
            get
            {
                if (Number <= 2)
                    return 100;

                if (Number <= 4)
                    return 150;

                return 200;
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
                return 6;
            }
        }
    }
}
