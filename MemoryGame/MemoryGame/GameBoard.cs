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
            public static int MinGridSize = 4;
            public static int ChosenDifficulty = 5;
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
            public void Flip(int row, int col)
            {
                if (row < 0 || row >= gridSize || col < 0 || col >= gridSize)
                {
                    throw new ArgumentException("Row or column is outside the legal range of 0 to " + (gridSize - 1));
                }
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i == row && j == col)
                    {
                        int temp = row * col;
                        

                    }
                }
            }


        }
        // Needs to be changed to fit
        public bool IsGameOver(int flippedCounter)
            {
                if(flippedCounter > (gridSize * gridSize / 2))
                {
                // put logic here
                return true;
                }
                else
                {
                return false;
                }
               
            }
        }


    }


