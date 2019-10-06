using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace BCS_3.Zak
{
    public enum CruiserBehaviour
    {
        Entering,
        Orbiting,
        Leaving
    }

    public enum CruiserType{
        Cruiser,
        Dreadnaught
    }

    public class SungCruiser
    {
        public Vector2 Position { get; set; }
        public float OrbitHeight { get; set; }

        public float Angle { get; set; }
        public bool Clockwise { get; set; } = true;

        public CruiserBehaviour Behaviour { get; set; } = CruiserBehaviour.Entering;
        public CruiserType Type { get; set; }

        public float Speed = 300.0f;
        public float OrbitSpeed = 100.0f;

        public bool Remove { get; set; } = false;

        public float FiringTimer { get; set; } = 5.0f;
        public float FiringDelay { get; set; } = 1.0f;

        public SungCruiser(Vector2 position, float orbitHeight, CruiserType type)
        {
            Position = position;
            OrbitHeight = orbitHeight;

            Type = type;
        }
    }
}
