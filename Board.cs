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
            this.matrix = new Node[y, x];
            this.adjMap = new Dictionary<Node, Dictionary<string, Node>>();
            this.robots = robots;
            this.makeMatrix();
            this.placeRobots();
            this.makeMap();
        }

        public Answer play(string color, int[] destination)
        {
            Node targetRobot = this.matrix[this.robots[color][0], this.robots[color][1]];
            Node destinationNode = this.matrix[destination[0], destination[1]];
            int maxSteps = int.MaxValue;
            Answer bestAnswer = null;
            Answer solo = this.oneRobotPath(color, destination);
            Dictionary<Node, Object[]> targetTracker = null;
            if (solo != null)
            {
                bestAnswer = solo;
                maxSteps = solo.TotalSteps;
                Console.WriteLine($"solo in {maxSteps} steps.");
                Program.printTracker(solo.Trackers.Peek(), this, destinationNode);
                targetTracker = solo.Trackers.Peek();
            }
            else
            {
                Console.WriteLine("no solo");
            }
            Answer nudgeRobots = nudgeRobotsOneMovement(color, destination, maxSteps);
            if (nudgeRobots != null){
                if(nudgeRobots.TotalSteps < maxSteps)
                {
                    maxSteps = nudgeRobots.TotalSteps;
                    bestAnswer = nudgeRobots;
                }
            }
            else
            {
                Console.WriteLine("no answer by nudging");
            }
            // Answer twoRobotPlay = this.twoRobots(color, destination, maxSteps);
            Answer twoRobotAnswer = this.twoRobots(color, destination, maxSteps);
            if(twoRobotAnswer != null)
            {
                if(twoRobotAnswer.TotalSteps < maxSteps)
                {
                    maxSteps = twoRobotAnswer.TotalSteps;
                    bestAnswer = twoRobotAnswer;
                }
            }
            return bestAnswer;
            // if (twoRobotPlay == null)
            // {
            //     if (maxSteps < int.MaxValue)
            //     {
            //         Console.WriteLine("doing it solo");
            //         // Answer soloAnswer = new Answer(null, null, null, color, targetRobot, destinationNode, null, solo, maxSteps, 0, this, maxSteps);
            //         // return soloAnswer;
            //     }
            //     Console.WriteLine("unsolvable");
            //     return null;

            // }
            // return twoRobotPlay;
        }
        public Answer nudgeRobotsOneMovement(string color, int[] destination, int maxSteps)
        {
            int[] robotLocation = this.robots[color];
            Node destinationNode = this.matrix[destination[0], destination[1]];
            Answer bestAnswer = null;
            foreach(var kvp in this.robots)
            {
                if(kvp.Key != color)
                {
                    Node helperNode = this.matrix[kvp.Value[0], kvp.Value[1]];
                    Dictionary<string, Node> neighbors = this.adjMap[helperNode];
                    string[] directions = {"up", "right", "down", "left"};
                    foreach(string direction in directions)
                    {
                        if(neighbors[direction] != null)
                        {
                            Node targetNeighbor = this.matrix[neighbors[direction].Row,neighbors[direction].Column];
                            Dictionary<string, int[]> adjustedRobots = new Dictionary<string, int[]>();
                            foreach(var robotkvp in this.robots)
                            {
                                if(robotkvp.Key == kvp.Key)
                                {
                                    adjustedRobots[kvp.Key] = new int[] { neighbors[direction].Row, neighbors[direction].Column };
                                }
                                else
                                {
                                    adjustedRobots[robotkvp.Key] = new int[] {robotkvp.Value[0], robotkvp.Value[1]};
                                }
                            }
                            // here we created a new board with the helper robot nudged and seeing if our target robot can get there in fewer steps than our base case.
                            Board newboard = new Board(this.x, this.y, this.walls, adjustedRobots);
                            Node newDestinationNode = newboard.Matrix[destination[0], destination[1]];
                            Answer nudgedPath = newboard.oneRobotPath(color, destination, maxSteps);
                            if(nudgedPath != null)
                            {
                                Console.WriteLine($"nudged path found by nudging {kvp.Key} robot to {neighbors[direction].Name}");
                                if(nudgedPath.TotalSteps < maxSteps)
                                {
                                    // here the nudged robot helps find a path that is shorter than our base case.
                                    maxSteps = nudgedPath.TotalSteps + 1;
                                    Console.WriteLine($"And it's a shorter path too! {maxSteps} steps!");
                                    // to make an answer, we need to produce a tracker for our nudged robot.
                                    Dictionary<Node, Object[]> nudgedRobotTracker = new Dictionary<Node, Object[]>();
                                    nudgedRobotTracker[helperNode] = new Object[] { 0, null};
                                    nudgedRobotTracker[targetNeighbor] = new Object[] {1, helperNode};
                                    // now we can start making an answer
                                    Answer nudgeAnswer = new Answer(kvp.Key, 1, new int[] {targetNeighbor.Row, targetNeighbor.Column}, nudgedRobotTracker);
                                    nudgeAnswer.addRobot(color, maxSteps-1, destination, nudgedPath.Trackers.Peek());
                                    bestAnswer = nudgeAnswer;
                                }
                            } 
                        }
                    }
                }
            }
            return bestAnswer;
        }

        public List<int[]> secondaryDestinations(int[] destination)
        {
            List<int[]> result = new List<int[]>();
            for (int y = 0; y < this.y; y++)
            {
                if (destination[1] + 1 < this.x)
                {
                    int[] newDestinationRight = { y, destination[1] + 1 };
                    result.Add(newDestinationRight);
                }
                if (destination[1] - 1 >= 0)
                {
                    int[] newDestinationLeft = { y, destination[1] - 1 };
                    result.Add(newDestinationLeft);
                }
            }
            for (int x = 0; x < this.x; x++)
            {
                if (destination[0] + 1 < this.y)
                {
                    int[] newDestinationDown = { destination[0] + 1, x };
                    result.Add(newDestinationDown);
                }
                if (destination[0] - 1 >= 0)
                {
                    int[] newDestinationUp = { destination[0] - 1, x };
                    result.Add(newDestinationUp);
                }
            }
            return result;
        }
        public MinHeap helperRobots(string color, List<int[]> helperDestinations, int maxSteps)
        {
            MinHeap heap = new MinHeap();
            int[] robotLocation = this.robots[color];
            Node originRobot = this.matrix[robotLocation[0], robotLocation[1]];
            if (originRobot.Occupied == false || originRobot.Color == "no robot")
            {
                throw new System.ArgumentException("Origin node must be occupied by a robot", "origin");
            }
            foreach (var kvp in this.robots)  // { red: [13, 5], "blue" [5,5]}
            {
                if (kvp.Key != color)
                {
                    foreach (var hDestination in helperDestinations)
                    {
                        Node hDestNode = this.matrix[hDestination[0], hDestination[1]];
                        Answer tracker = this.oneRobotPath(kvp.Key, hDestination, maxSteps);
                        if (tracker != null)
                        {
                            heap.insert(tracker.TotalSteps, kvp.Key, hDestination, tracker.Trackers.Peek());
                        }
                    }
                }
            }
            return heap;
        }
        public Answer twoRobots(string color, int[] destination, int maxSteps)
        {
            Node destinationNode = this.matrix[destination[0], destination[1]];
            List<int[]> sd = this.secondaryDestinations(destination);
            MinHeap heap = this.helperRobots(color, sd, maxSteps);
            // heap.printMyHeaps();
            Answer twoRobotAnswer = null;
            Board secondaryBoard = this;
            Node secondaryDestination = destinationNode;
            
            Dictionary<Node, Object[]> bestTargetRobotPath = new Dictionary<Node, Object[]>();
            while (heap.Count > 0)
            {
                // pull off the heap to find a robot's path to a secondary destination
                RobotPriority helper = heap.remove();
                int helperSteps = helper.Steps;
                // heap is useless once we find steps greater than our max
                if (helper.Steps >= maxSteps - 1)
                {
                    break;
                }
                Dictionary<string, int[]> movedRobots = new Dictionary<string, int[]>();
                // move the helper robot to its spot
                foreach (var kvp in this.robots)
                {
                    if (kvp.Key == helper.Color)
                    {
                        movedRobots[kvp.Key] = new int[] { helper.Destination[0], helper.Destination[1] };
                    }
                    else
                    {
                        movedRobots[kvp.Key] = new int[] { kvp.Value[0], kvp.Value[1] };
                    }
                }
                // create the new board and try moving the target robot to its spot
                Board newboard = new Board(this.x, this.y, this.walls, movedRobots);
                Node newDN = newboard.Matrix[destination[0], destination[1]];
                Answer helpedPath = newboard.oneRobotPath(color, destination, maxSteps);
                if (helpedPath != null)
                {
                    int mainRobotSteps = helpedPath.TotalSteps;
                    if (mainRobotSteps + helperSteps < maxSteps)
                    {
                        maxSteps = mainRobotSteps + helperSteps;
                        twoRobotAnswer = new Answer(helper.Color, helper.Steps, helper.Destination, helper.RobotPath);
                        twoRobotAnswer.addRobot(color, helpedPath.TotalSteps, destination, helpedPath.Trackers.Peek());
                    }
                }
            }
            if (twoRobotAnswer == null)
            {
                Console.WriteLine("null on 259");
                return null;
            }

            // Console.WriteLine($"Line 136, helped by {helperColor} to {helperDestination[0]}-{helperDestination[1]} in {helperStepCount}, the shortest path is {maxSteps}.");
            return twoRobotAnswer;
            // Answer answer = new Answer(helperColor, helperRobot, helperDestinationNode, color, targetRobot, secondaryDestination, bestHelper.Path, bestTargetRobotPath, maxSteps, helperStepCount, secondaryBoard, maxSteps - helperStepCount);
            // return answer;
        }

        // tracker is {destination node: [int steps, previous node]}
        public Answer oneRobotPath(string color, int[] destination, int maxSteps = -1)
        {
            int[] robotLocation = this.robots[color];
            Node originRobot = this.matrix[robotLocation[0], robotLocation[1]];
            Node destinationNode = this.matrix[destination[0], destination[1]];
            if (originRobot.Occupied == false || originRobot.Color == "no robot")
            {
                throw new System.ArgumentException("Origin node must be occupied by a robot", "origin");
            }
            Dictionary<Node, Object[]> tracker = new Dictionary<Node, Object[]> {
               {originRobot, new Object[] {0, null}}
           };
            Queue queue = new Queue();
            queue.insert(originRobot);
            while (queue.Head != null)
            {
                QueueNode current = queue.dequeue();
                if(maxSteps > 0 && (int)tracker[current.Val][0] >= maxSteps)
                {
                    continue;
                }
                Dictionary<string, Node> neighbors = this.adjMap[current.Val];
                foreach (var neighbor in neighbors)
                {
                    string direction = neighbor.Key;
                    Node nextStop = neighbor.Value;
                    if (nextStop != null)
                    {
                        switch (direction)
                        {
                            case "up":
                                if (nextStop.Up != null && nextStop.Up.Color == originRobot.Color)
                                {
                                    nextStop = null;
                                }
                                break;
                            case "right":
                                if (nextStop.Right != null && nextStop.Right.Color == originRobot.Color)
                                {
                                    nextStop = null;
                                }
                                break;
                            case "down":
                                if (nextStop.Down != null && nextStop.Down.Color == originRobot.Color)
                                {
                                    nextStop = null;
                                }
                                break;
                            case "left":
                                if (nextStop.Left != null && nextStop.Left.Color == originRobot.Color)
                                {
                                    nextStop = null;
                                }
                                break;
                            default:
                                Console.WriteLine("line 95 oops");
                                break;
                        }
                    }
                    if (nextStop != null && !tracker.ContainsKey(nextStop))
                    {
                        int steps = (int)tracker[current.Val][0] + 1;
                        tracker.Add(nextStop, new Object[] { steps, current.Val });
                        if (nextStop == destinationNode)
                        {
                            Answer oneRobotAnswer = new Answer(color, steps, destination, tracker);
                            //    Console.WriteLine($"found the path for {color} robot to {destinationNode.Name}");
                            return oneRobotAnswer;
                        }
                        queue.insert(nextStop);
                    }
                }
            }
            //    Console.WriteLine("unreachable");
            return null;
        }
        private void placeRobots()
        {
            foreach (var robot in this.robots)
            {
                string color = robot.Key;
                Node node = this.matrix[robot.Value[0], robot.Value[1]];
                node.Occupied = true;
                node.Color = color;
            }
        }
        private void makeMap()
        {
            string[] directions = { "up", "right", "down", "left" };
            foreach (Node current in this.matrix)
            {
                foreach (string direction in directions)
                {
                    Node previous = current;
                    Node runner = null;
                    switch (direction)
                    {
                        case "up":
                            runner = current.Up;
                            while (runner != null && runner.Occupied == false)
                            {
                                previous = runner;
                                runner = runner.Up;
                            }
                            break;
                        case "right":
                            runner = current.Right;
                            while (runner != null && runner.Occupied == false)
                            {
                                previous = runner;
                                runner = runner.Right;
                            }
                            break;
                        case "down":
                            runner = current.Down;
                            while (runner != null && runner.Occupied == false)
                            {
                                previous = runner;
                                runner = runner.Down;
                            }
                            break;
                        case "left":
                            runner = current.Left;
                            while (runner != null && runner.Occupied == false)
                            {
                                previous = runner;
                                runner = runner.Left;
                            }
                            break;
                        default:
                            Console.WriteLine("oops");
                            break;
                    }
                    if (!this.adjMap.ContainsKey(current))
                    {
                        this.adjMap.Add(current, new Dictionary<string, Node>());
                    }
                    if (previous == current)
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
            for (int row = 0; row < this.x; row++)
            {
                for (int col = 0; col < this.y; col++)
                {
                    this.matrix[row, col] = new Node(row, col);
                }
            }
            this.addPointers();
            this.addWalls();
        }
        private void addWalls()
        {
            foreach (KeyValuePair<int[], string[]> entry in this.walls)
            {
                Node current = this.matrix[entry.Key[0], entry.Key[1]];
                current.addWalls(entry.Value);
            }
        }
        private void addPointers()
        {
            for (int row = 0; row < this.y; row++)
            {
                for (int col = 0; col < this.x; col++)
                {
                    Node current = this.matrix[row, col];
                    if (row - 1 >= 0)
                    {
                        current.Up = this.matrix[row - 1, col];
                    }
                    if (row + 1 < this.y)
                    {
                        current.Down = this.matrix[row + 1, col];
                    }
                    if (col - 1 >= 0)
                    {
                        current.Left = this.matrix[row, col - 1];
                    }
                    if (col + 1 < this.x)
                    {
                        current.Right = this.matrix[row, col + 1];
                    }
                }
            }
        }
    }
}
