using System;

namespace CatAndMouseGame
{
    public class Player
    {
        public string name;
        public int location;
        public State state = State.NotInGame;
        public int distanceTraveled = 0;
        
        private int boardSize;

        public Player(string name, int boardSize)
        {
            this.name = name;
            this.boardSize = boardSize;
            this.location = -1;
        }

        public void Move(int steps)
        {
            if (state == State.NotInGame)
            {
                location = steps;
                state = State.Playing;
                return;
            }

            distanceTraveled += Math.Abs(steps);

            int newLocation = (location - 1 + steps) % boardSize;
            if (newLocation < 0)
            {
                newLocation += boardSize;
            }
            
            location = newLocation + 1;
        }
    }
}
