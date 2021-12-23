using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        public List<GridBox> grids = new List<GridBox>();


        //public int xLenght; // fixed grammar
        public int xLength;
        public int yLength;


        public Grid(int lines, int columns)
        {

            //Inverted x and y for clarity
            xLength = columns;
            yLength = lines;


            Console.WriteLine($"\nThe battlefield has been created ({columns},{lines})\n");

            for (int i = 0; i < lines; i++)
            {
                //grids.Add(newBox); //There was an error here due to using the variable before declaration
                for (int j = 0; j < columns; j++)
                {
                    GridBox newBox = new GridBox(j, i, false, (columns * i + j));
                    //Console.Write($"{newBox.Index}\n"); original
                    grids.Add(newBox);
                }
            }
        }



        public void DrawBattlefield()
        {
            DrawBattlefield(yLength, xLength);
        }


        // prints the matrix that indicates the tiles of the battlefield
        public void DrawBattlefield(int Lines, int Columns)
        {

            //Id of the current pos
            int id = 0;

            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridBox currentgrid = new GridBox();

                    //Added method to get currentGrid data
                    id = ConvertGridPosToListPos(j, i, yLength, xLength);
                    currentgrid = grids[id];


                    if (currentgrid.ocupied)
                    {
                        Console.Write("[" + currentgrid.charId + "]\t");
                    }
                    else
                    {
                        //Increased the number of spaces to 5
                        Console.Write($"[     ]\t");
                        //Console.Write($"[" + currentgrid.Index + "  ]\t");

                    }
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }


        int ConvertGridPosToListPos(int x, int y, int Lines, int Columns)
        {
            int total = 0;
            total = (Columns * y) + x;
            return total;
        }

    }
}
