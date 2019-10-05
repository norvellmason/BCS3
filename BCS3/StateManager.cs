using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS_3
{
    class StateManager
    {
        private static GameState CurrentGameState;

        public StateManager()
        {

        }

        public static GameState GetCurrentGameState()
        {
            return StateManager.CurrentGameState;
        }

        public static void SetGameState(GameState gameState)
        {
            StateManager.CurrentGameState = gameState;
        }
    }
}
