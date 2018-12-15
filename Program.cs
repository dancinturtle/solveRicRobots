using System;
using System.Collections.Generic;
using System.Reflection;
namespace RicRobots
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] dimensions = {16, 16};
            Dictionary<string, int[]> robots = new Dictionary<string, int[]> {
                {"green", new int[] {11, 2} },
                {"red", new int[] {1, 11} },
                {"blue", new int[] {3, 10} },
                {"yellow", new int[] {10, 14} }
            };
            Dictionary<int[], string[]> walls = new Dictionary<int[], string[]> {
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
            Board board = new Board(dimensions[0], dimensions[1], walls, robots);
            MinHeap heap = board.twoRobots(new int[] {10, 14}, new int[] {2, 14});
            // Console.WriteLine("heap's count " + heap/.Data.Count);
            while(heap.Data.Count > 0)
            {
                // Console.WriteLine("line 57 length " + heap.Data.Count);
                RobotPriority top = heap.removeFromTheHeap();
                Console.WriteLine(top.Steps);
            }
            // Console.WriteLine("heap's count " + heap.Data.Count);
            



            // List<int[]> secondaryD = board.secondaryDestinations(new int[] {14, 2});
            // foreach(int[] destination in secondaryD)
            // {
            //     Console.WriteLine(destination[0] + " " + destination[1]);
            // }

            // Node testNode = board.Matrix[7, 9];
            // Dictionary<Node, Object[]> tracker = board.oneRobotPath(new int[] {14, 9}, new int[]{4, 6});
            // printTracker(tracker);
            // printAdjMap(board);
            // Node testNode = board.Matrix[1,9];
            // Console.WriteLine(testNode.Name + ", " + testNode.Row + ", " + testNode.Column + ", " + testNode.Up.Name + ", " + testNode.Right.Name);
            // Type t = testNode.GetType();
            // Console.Write("type t " + t);
            // PropertyInfo[] props = t.GetProperties();
            // foreach(var prop in props)
            // {
            //     Console.WriteLine("prop " + prop + " " + prop.GetValue(testNode));
            // }
            // printBoard(board);
        }
        static string printTracker(Dictionary<Node, Object[]> tracker)
        {
            string result = "";
            foreach(var kvp in tracker)
            {
                result += kvp.Key.Name + " : ";
                result += kvp.Value[0].ToString() + " steps through ";
                if(kvp.Value[1] != null)
                {
                    Node via = (Node)kvp.Value[1];
                    result += via.Name;
                }
                else
                {

                    result += "none";
                }
                result += "\n";
            }
            Console.WriteLine(result);
            return result;
        }
        static string printNeighbors(Board board, int y, int x)
        {
            Node node = board.Matrix[y, x];
            string result = "Neighbors of " + node.Name + ": ";
            foreach(var kvp in board.adjMap[node])
            {
                result += kvp.Key + " ";
                if(kvp.Value != null)
                {
                    result += kvp.Value.Name + ", ";
                }
                else
                {
                    result += "null, ";
                }
            }
            Console.WriteLine(result);
            return result;
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
