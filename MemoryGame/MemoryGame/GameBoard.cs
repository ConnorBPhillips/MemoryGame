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
            
            // Maybe we can use this
          


        
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


