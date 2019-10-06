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
        private static Queue<GameState> GameStateQueue = new Queue<GameState>();

        public StateManager()
        {

        }

        public static GameState GetCurrentGameState()
        {
            return StateManager.CurrentGameState;
        }

        public static void AddGameState(GameState gameState)
        {
            GameStateQueue.Enqueue(gameState);

            if (GameStateQueue.Count == 1)
            {
                StateManager.CurrentGameState = gameState;
            }
        }

        public static void AdvanceGameState()
        {
            if (GameStateQueue.Count > 0)
            {
                GameStateQueue.Dequeue();

                CurrentGameState = GameStateQueue.Peek();
            }
        }
    }
}
