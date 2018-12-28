using System;
using System.Collections.Generic;
using System.Reflection;
namespace RicRobots
{
    class Program
    {
        static public Dictionary<int[], string[]> eliBoard = new Dictionary<int[], string[]> {
            { new int[] {0,4}, new string[] {"right"} },
            { new int[] {0,10}, new string[] {"right"} },
            { new int[] {1,2}, new string[] {"right", "down"} },
            { new int[] {1,9}, new string[] {"down", "left"} },
            { new int[] {2,14}, new string[] {"up", "right"} },
            { new int[] {3,1}, new string[] {"down", "left"} },
            { new int[] {4,0}, new string[] {"down"} },
            { new int[] {4,6}, new string[] {"up", "left"} },
            { new int[] {4,10}, new string[] {"right", "down"} },
            { new int[] {4,15}, new string[] {"down"} },
            { new int[] {6,5}, new string[] {"up", "right"} },
            { new int[] {6,7}, new string[] {"down"} },
            { new int[] {6,8}, new string[] {"down"} },
            { new int[] {6,12}, new string[] {"up", "left"} },
            { new int[] {7,3}, new string[] {"right", "down"} },
            { new int[] {7,6}, new string[] {"right"} },
            { new int[] {7,9}, new string[] {"left"} },
            { new int[] {8,6}, new string[] {"right"} },
            { new int[] {8,9}, new string[] {"left"} },
            { new int[] {9,1}, new string[] {"up", "right"} },
            { new int[] {9,5}, new string[] {"up", "left"} },
            { new int[] {9,7}, new string[] {"up"} },
            { new int[] {9,8}, new string[] {"up"} },
            { new int[] {9,14}, new string[] {"right", "down"} },
            { new int[] {10,0}, new string[] {"down"} },
            { new int[] {10,8}, new string[] {"up", "right"} },
            { new int[] {10,15}, new string[] {"down"} },
            { new int[] {11,13}, new string[] {"down", "left"} },
            { new int[] {12,6}, new string[] {"right", "down"} },
            { new int[] {13,10}, new string[] {"up", "left"} },
            { new int[] {14,2}, new string[] {"down", "left"} },
            { new int[] {15,5}, new string[] {"right"} },
            { new int[] {15,11}, new string[] {"right"} }
        };
        static public Dictionary<int[], string[]> dec27 = new Dictionary<int[], string[]> {
            { new int[] {0,3}, new string[] {"right"} },
            { new int[] {0,9}, new string[] {"right"} },
            { new int[] {1,1}, new string[] {"left", "down"} },
            { new int[] {1,13}, new string[] {"up", "right"} },
            { new int[] {2,6}, new string[] {"up", "right"} },
            { new int[] {3,9}, new string[] {"up", "left"} },
            { new int[] {4,2}, new string[] {"right", "down"} },
            { new int[] {4,15}, new string[] {"down"} },
            { new int[] {5,0}, new string[] {"down"} },
            { new int[] {5,7}, new string[] {"left", "up"} },
            { new int[] {6,7}, new string[] {"down"} },
            { new int[] {6,8}, new string[] {"down"} },
            { new int[] {6,10}, new string[] {"right", "down"} },
            { new int[] {6,14}, new string[] {"left", "down"} },
            { new int[] {7,6}, new string[] {"right"} },
            { new int[] {7,9}, new string[] {"left"} },
            { new int[] {8,6}, new string[] {"right"} },
            { new int[] {8,9}, new string[] {"left"} },
            { new int[] {9,4}, new string[] {"left", "down"} },
            { new int[] {9,15}, new string[] {"up"} },
            { new int[] {10,1}, new string[] {"up", "right"} },
            { new int[] {10,8}, new string[] {"left", "up"} },
            { new int[] {10,13}, new string[] {"left", "up"} },
            { new int[] {11,0}, new string[] {"down"} },
            { new int[] {11,10}, new string[] {"down", "right"} },
            { new int[] {12,14}, new string[] {"left", "down"} },
            { new int[] {13,6}, new string[] {"up", "left"} },
            { new int[] {14,2}, new string[] {"down", "right"} },
            { new int[] {14,9}, new string[] {"up", "right"} },
            { new int[] {15,3}, new string[] {"right"} },
            { new int[] {15,11}, new string[] {"right"} }
        };
        static public Dictionary<int[], string[]> bdayBoard = new Dictionary<int[], string[]> {
            { new int[] {0,3}, new string[] {"right"} },
            { new int[] {0,8}, new string[] {"right"} },
            { new int[] {1,6}, new string[] {"left", "down"} },
            { new int[] {1,11}, new string[] {"up", "left"} },
            { new int[] {2,15}, new string[] {"up"} },
            { new int[] {3,1}, new string[] {"up", "right"} },
            { new int[] {3,14}, new string[] {"up", "right"} },
            { new int[] {4,5}, new string[] {"left", "up"} },
            { new int[] {4,9}, new string[] {"down", "right"} },
            { new int[] {5,2}, new string[] {"right", "down"} },
            { new int[] {5,7}, new string[] {"down", "right"} },
            { new int[] {6,0}, new string[] {"down"} },
            { new int[] {6,7}, new string[] {"down"} },
            { new int[] {6,8}, new string[] {"down"} },
            { new int[] {6,12}, new string[] {"down", "left"} },
            { new int[] {7,6}, new string[] {"right"} },
            { new int[] {7,9}, new string[] {"left"} },
            { new int[] {8,6}, new string[] {"right"} },
            { new int[] {8,9}, new string[] {"left"} },
            { new int[] {9,7}, new string[] {"up"} },
            { new int[] {9,8}, new string[] {"up"} },
            { new int[] {10,5}, new string[] {"left", "down"} },
            { new int[] {10,11}, new string[] {"up", "left"} },
            { new int[] {11,1}, new string[] {"up", "left"} },
            { new int[] {11,13}, new string[] {"down", "left"} },
            { new int[] {11,15}, new string[] {"down"} },
            { new int[] {12,6}, new string[] {"right", "up"} },
            { new int[] {12,10}, new string[] {"right", "up"} },
            { new int[] {13,0}, new string[] {"down"} },
            { new int[] {13,12}, new string[] {"right", "down"} },
            { new int[] {14,3}, new string[] {"down", "right"} },
            { new int[] {15,5}, new string[] {"right"} },
            { new int[] {15,9}, new string[] {"right"} }
        };
        static void Main(string[] args)
        {
            int[] dimensions = {16, 16};
            Dictionary<string, int[]> robots = new Dictionary<string, int[]> {
                {"green", new int[] {12, 9 }},
                {"red", new int[] {3, 14} },
                {"blue", new int[] {4, 5} },
                {"yellow", new int[] {2, 2} }
            };
            Dictionary<int[], string[]> walls = dec27;
            Board board = new Board(dimensions[0], dimensions[1], walls, robots);
            Answer answer = board.play("blue", new int[] {14, 9});
            if(answer != null)
            {
                answer.printAnswer();
            }
            else 
            {
                Console.WriteLine("Hahahahah");
            }
           

        }
       
        static string printAdjMap(Board board)
        {
            string result = "";
            foreach(var pair in board.adjMap)
            {
                result += pair.Key.Name + ": ";
                foreach(var vp in pair.Value)
                {
                    result += vp.Key + " - ";
                    if(vp.Value != null)
                    {
                        result += vp.Value.Name + ", ";
                    }
                    else
                    {
                        result += "null,";
                    }
                }
                result += "\n";
            }
            Console.WriteLine(result);
            return result;
        }
        static string printMatrix(Board board)
        {
            string matrixString = "";
            for(int i=0; i<board.Y; i++)
           {
               for(int j=0; j<board.X; j++)
               {
                   matrixString += board.Matrix[i, j].Name + " ";
               }
               matrixString += "\n";
           }
           return matrixString;
        }
        static void printBoard(Board board)
        {
            string result = "";
            for(int row = 0; row < board.Y; row++)
            {
                string rowstring = "";
                for(int col=0; col < board.X; col++)
                {
                    int count = 0;
                    Node current = board.Matrix[row, col];
                    if(current.Left == null)
                    {
                        count += 1;
                        rowstring += "|";
                    }
                    if(current.Down == null)
                    {
                        rowstring += "_";
                        count += 1;
                    }
                    rowstring += current.Name;
                    count += 3;
                    if(current.Occupied == true)
                    {
                        rowstring += "*";
                        count += 1;
                    }
                    if(current.Down == null)
                    {
                        rowstring += "_";
                        count += 1;
                    }
                    if(current.Right == null)
                    {
                        count += 1;
                        rowstring += "|";
                    }
                    while(count < 7)
                    {
                        count += 1;
                        rowstring += " ";
                    }
                }
                result += rowstring + "\n";
            }
            Console.WriteLine(result);
        }

    }
}
