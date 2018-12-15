using System;
using System.Collections.Generic;
using System.Reflection;

namespace RicRobots
{
    struct Board
    {
       private Node[,] matrix;
       public Node[,] Matrix
       {
           get => matrix;
       }
       private int x; // 1-indexed
       public int X
       {
           get => x;
       }
       private int y; // 1-indexed
       public int Y
       {
           get => y;
       }
       public Dictionary<string, int[]> robots;
       public Dictionary<Node, Dictionary<string, Node>> adjMap
       {
           get;
           set;
       }
       private Dictionary<int[], string[]> walls;
       public Dictionary<int[], string[]> Walls
       {
           get => walls;
       }

       public Board(int x, int y, Dictionary<int[], string[]> walls, Dictionary<string, int[]> robots)
       {
           this.x = x;
           this.y = y;
           this.walls = walls;
           this.matrix = new Node[y,x];
           this.adjMap = new Dictionary<Node, Dictionary<string, Node>>();
           this.robots = robots;
           this.makeMatrix();
           this.placeRobots();
           this.makeMap();
       }

    //    public void play(string color, int[] destination)
    //    {

    //    }
       
       public List<int[]> secondaryDestinations(int[] destination)
       {
           List<int[]> result = new List<int[]>();
           for(int y=0; y<this.y; y++)
           {
               if(destination[1] + 1 < this.x)
               {
                    int[] newDestinationRight = {y, destination[1]+1};
                    result.Add(newDestinationRight);
               }
               if(destination[1]-1 >= 0)
               {
                    int[] newDestinationLeft = {y, destination[1]-1};
                    result.Add(newDestinationLeft);
               }
           }
           for(int x=0; x<this.x; x++)
           {
               if(destination[0] + 1 < this.y)
               {
                   int[] newDestinationDown = {destination[0] + 1, x};
                   result.Add(newDestinationDown);
               }
               if(destination[0]-1 >= 0)
               {
                   int[] newDestinationUp = {destination[0]-1, x};
                   result.Add(newDestinationUp);
               }
           }
           return result;
       }
       public MinHeap helperRobots(string color, List<int[]> helperDestinations)
       {
           MinHeap heap = new MinHeap();
           int[] robotLocation = this.robots[color];
           Node originRobot = this.matrix[robotLocation[0], robotLocation[1]];
           if(originRobot.Occupied == false || originRobot.Color == "no robot")
           {
               throw new System.ArgumentException("Origin node must be occupied by a robot", "origin");
           }
           foreach (var kvp in this.robots)  // { red: [13, 5], "blue" [5,5]}
           {
               if(kvp.Key != color)
               {
                foreach(var hDestination in helperDestinations)
                {
                    Node hDestNode = this.matrix[hDestination[0], hDestination[1]];
                    Dictionary<Node, Object[]> tracker = this.oneRobotPath(kvp.Key, hDestination);
                    if(tracker.ContainsKey(hDestNode))
                    {
                        heap.insert((int)tracker[hDestNode][0], kvp.Key, hDestination);
                    }
                }
               }
           }
           return heap;
       }
       public Dictionary<Node, Object[]> twoRobots(string color, int[] destination)
       {
           Node destinationNode = this.matrix[destination[0], destination[1]];
           List<int[]> sd = this.secondaryDestinations(destination);
           MinHeap heap = this.helperRobots(color, sd);
           heap.printMyHeaps();
           string helperColor = "";
           int[] helperDestination = null;
           int helperStepCount = 100000;
           Dictionary<Node, Object[]> best = new Dictionary<Node, Object[]>();
           int shortestPath = 10000;
           while(heap.Count > 0)
           {
               RobotPriority helper = heap.remove();
               int helperSteps = helper.Steps;
               Dictionary<string, int[]> movedRobots = new Dictionary<string, int[]>();
               foreach (var kvp in this.robots)
               {
                   if(kvp.Key == helper.Color)
                   {
                       movedRobots[kvp.Key] = new int[] {helper.Destination[0], helper.Destination[1]};
                   }
                   else 
                   {
                       movedRobots[kvp.Key] = new int[] {kvp.Value[0], kvp.Value[1]};
                   }
               }
               Board newboard = new Board(this.x, this.y, this.walls, movedRobots);
               Node newDN = newboard.Matrix[destination[0], destination[1]];
               Dictionary<Node, Object[]> helpedPath = newboard.oneRobotPath(color, destination);
               if(helpedPath.ContainsKey(newDN))
               {
                   int mainRobotSteps = (int) helpedPath[newDN][0];
                   if(mainRobotSteps + helperSteps < shortestPath)
                   {
                       best = helpedPath;
                       shortestPath = mainRobotSteps + helperSteps;
                       helperColor = helper.Color;
                       helperDestination = helper.Destination;
                       helperStepCount = helperSteps;
                   }
               }
           }
           Console.WriteLine($"helped by {helperColor} to {helperDestination[0]}-{helperDestination[1]} in {helperStepCount}, the shortest path is {shortestPath}.");
           return best;
       }
       // tracker is {destination node: [int steps, previous node]}
       public Dictionary<Node, Object[]> oneRobotPath(string color, int[] destination)
       {
           int[] robotLocation = this.robots[color];
           Node originRobot = this.matrix[robotLocation[0], robotLocation[1]];
           Node destinationNode = this.matrix[destination[0], destination[1]];
           if(originRobot.Occupied == false || originRobot.Color == "no robot")
           {
               throw new System.ArgumentException("Origin node must be occupied by a robot", "origin");
           }
           Dictionary<Node, Object[]> tracker = new Dictionary<Node, Object[]> {
               {originRobot, new Object[] {0, null}}
           };
           Queue queue = new Queue();
           queue.insert(originRobot);
           while(queue.Head != null)
           {
               QueueNode current = queue.dequeue();
               Dictionary<string, Node> neighbors = this.adjMap[current.Val];
               foreach (var neighbor in neighbors)
               {
                   string direction = neighbor.Key;
                   Node nextStop = neighbor.Value;
                   if(nextStop != null)
                   {
                        switch(direction)
                        {
                            case "up":
                                if(nextStop.Up != null && nextStop.Up.Color == originRobot.Color)
                                {
                                    nextStop = null;
                                }
                                break;
                            case "right":
                                if(nextStop.Right != null && nextStop.Right.Color == originRobot.Color)
                                {
                                    nextStop = null;
                                }
                                break;
                            case "down":
                                if(nextStop.Down != null && nextStop.Down.Color == originRobot.Color)
                                {
                                    nextStop = null;
                                }
                                break;
                            case "left":
                                if(nextStop.Left != null && nextStop.Left.Color == originRobot.Color)
                                {
                                    nextStop = null;
                                }
                                break;
                            default:
                                Console.WriteLine("line 95 oops");
                                break;
                        }
                    }
                   if(nextStop != null && !tracker.ContainsKey(nextStop))
                   {
                       int steps = (int)tracker[current.Val][0] + 1;
                       tracker.Add(nextStop, new Object[]{steps, current.Val});
                       if(nextStop == destinationNode)
                       {
                           Console.WriteLine($"found the path for {color} robot to {destinationNode.Name}");
                           return tracker;
                       }
                       queue.insert(nextStop);
                   }
               }
           }
           Console.WriteLine("unreachable");
           return tracker;

       }
       private void placeRobots()
       {
           foreach(var robot in this.robots)
           {
                string color = robot.Key;
                Node node = this.matrix[robot.Value[0], robot.Value[1]];
                node.Occupied = true;
                node.Color = color;
           }
       }
       private void makeMap()
       {
           string[] directions = {"up", "right", "down", "left"};
           foreach(Node current in this.matrix)
           {
               foreach(string direction in directions)
               {
                    Node previous = current;
                    Node runner = null;
                    switch(direction)
                    {
                        case "up":
                            runner = current.Up;
                            while(runner != null && runner.Occupied == false)
                            {   
                                previous = runner;
                                runner = runner.Up;
                            }
                            break;
                        case "right":
                            runner = current.Right;
                            while(runner != null && runner.Occupied == false)
                            {   
                                previous = runner;
                                runner = runner.Right;
                            }
                            break;
                        case "down":
                            runner = current.Down;
                            while(runner != null && runner.Occupied == false)
                            {   
                                previous = runner;
                                runner = runner.Down;
                            }
                            break;
                        case "left":
                            runner = current.Left;
                            while(runner != null && runner.Occupied == false)
                            {   
                                previous = runner;
                                runner = runner.Left;
                            }
                            break;
                        default:
                            Console.WriteLine("oops");
                            break;
                    }
                    if(!this.adjMap.ContainsKey(current))
                    {
                        this.adjMap.Add(current, new Dictionary<string, Node>());
                    }
                    if(previous == current)
                    {
                        this.adjMap[current][direction] = null;
                    }
                    else
                    {
                        this.adjMap[current][direction] = previous;
                    }
               }
           }
       }
       private void makeMatrix()
       {
           for(int row=0; row < this.x; row++)
           {
               for(int col=0; col < this.y; col++)
               {
                   this.matrix[row, col] = new Node(row, col);
               } 
           }
           this.addPointers();
           this.addWalls();
       }
       private void addWalls()
       {
           foreach(KeyValuePair<int[], string[]> entry in this.walls)
           {
               Node current = this.matrix[entry.Key[0], entry.Key[1]];
               current.addWalls(entry.Value);
           }
       }
       private void addPointers()
       {
           for(int row=0; row < this.y; row++)
           {
               for(int col=0; col < this.x; col++)
               {   
                   Node current = this.matrix[row, col];
                   if (row-1 >= 0)
                   {
                       current.Up = this.matrix[row-1, col];
                   }
                   if (row+1 < this.y)
                   {
                       current.Down = this.matrix[row+1, col];
                   }
                   if (col-1 >= 0)
                   {
                       current.Left = this.matrix[row, col-1];
                   }
                   if (col+1 < this.x)
                   {
                       current.Right = this.matrix[row, col+1];
                   }
               }
           }
       }
    }
}
