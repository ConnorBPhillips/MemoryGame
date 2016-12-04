using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame
{
        class GameBoard
        {
            private int gridSize;
            private bool[,] grid;           // Store the on/off state of the grid
            private Random rand;
            public const int MaxGridSize = 10;
            public const int MinGridSize = 3;
            public int GridSize
            {
                get
                {
                    return gridSize;
                }
                set
                {
                    if (value >= MinGridSize && value <= MaxGridSize)
                    {
                        gridSize = value;
                        grid = new bool[gridSize, gridSize];
                        NewGame();
                    }
                }
            }
            public GameBoard()
            {
                rand = new Random();
                GridSize = MinGridSize;
            }
            public bool GetGridValue(int row, int col)
            {
                return grid[row, col];
            }
            public void NewGame()
            {
                for (int r = 0; r < gridSize; r++)
                {
                    for (int c = 0; c < gridSize; c++)
                    {
                        // We need to put the logic for setting pictures to grid here
                        //grid[r, c] = rand.Next(2) == 1;
                    }
                }
            }
            // Maybe we can use this
            public void Move(int row, int col)
            {
                if (row < 0 || row >= gridSize || col < 0 || col >= gridSize)
                {
                    throw new ArgumentException("Row or column is outside the legal range of 0 to " + (gridSize - 1));
                }

            }
            // Needs to be changed to fit
            public bool IsGameOver()
            {
                for (int r = 0; r < gridSize; r++)
                {
                    for (int c = 0; c < gridSize; c++)
                    {
                        if (grid[r, c])
                        {
                            return false;
                        }
                    }
                }
                // All values must be false (off)
                return true;
            }
        }


    }

}
