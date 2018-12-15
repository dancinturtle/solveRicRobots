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
                {"green", new int[] {15, 6} },
                {"red", new int[] {11, 6} },
                {"blue", new int[] {10, 10} },
                {"yellow", new int[] {11, 13} }
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
            // MinHeap heap = board.twoRobots("yellow", new int[]{15,13});
            board.twoRobots("yellow", new int[] {9, 1});

        }
        static string printTracker(Dictionary<Node, Object[]> tracker, Board board, int[] destination)
        {
            Node destinationNode = board.Matrix[destination[0], destination[1]];
            string result = "";
            if(tracker.ContainsKey(destinationNode))
            {
                int stepsRequired = (int) tracker[destinationNode][0];
                result +=$"Destination {destinationNode.Name} found in {stepsRequired} steps. \n";
                
                string[] pathNodes = new string[stepsRequired + 1];
                for(int i=stepsRequired; i>=0; i--)
                {   
                    pathNodes[i] = destinationNode.Name;
                    Node viaNode = (Node) tracker[destinationNode][1];
                    destinationNode = viaNode;
                }
                foreach (string viaNodeName in pathNodes)
                {
                    result += $"{viaNodeName} -> ";
                }
                result += "end \n";
                Console.WriteLine(result);
                return result;
            }
            result += $"Destination {destinationNode.Name} could not be reached. \n";
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
