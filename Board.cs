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
            Dictionary<Node, Object[]> solo = this.oneRobotPath(color, destination);
            int maxSteps = int.MaxValue;
            Dictionary<Node, Object[]> targetTracker = null;
            if (solo.ContainsKey(destinationNode))
            {
                maxSteps = (int)solo[destinationNode][0];
                Console.WriteLine($"solo in {maxSteps} steps.");
                Program.printTracker(solo, this, destinationNode);
                targetTracker = solo;
            }
            Answer nudgeRobots = nudgeRobotsOneMovement(color, destination, maxSteps);
            if (nudgeRobots != null){
                Console.WriteLine("got an answer by nudging");
            }
            else
            {
                Console.WriteLine("no answer by nudging");
            }
            Answer twoRobotPlay = this.twoRobots(color, destination, maxSteps);
            if (twoRobotPlay == null)
            {
                if (maxSteps < int.MaxValue)
                {
                    Console.WriteLine("doing it solo");
                    Answer soloAnswer = new Answer(null, null, null, color, targetRobot, destinationNode, null, solo, maxSteps, 0, this, maxSteps);
                    return soloAnswer;
                }
                Console.WriteLine("unsolvable");
                return null;

            }
            return twoRobotPlay;
        }
        public Answer nudgeRobotsOneMovement(string color, int[] destination, int maxSteps)
        {
            int[] robotLocation = this.robots[color];
            Answer bestAnswer = null;
            foreach(var kvp in this.robots)
            {
                if(kvp.Key != color)
                {
                    Node helperNode = this.matrix[kvp.Value[0], kvp.Value[1]];
                    if(helperNode.Up != null)
                    {
                        Answer nudgedPathUp = this.nudge(helperNode.Up, kvp.Key, color, destination, maxSteps);
                        if(nudgedPathUp != null)
                        {
                            bestAnswer = nudgedPathUp;
                            maxSteps = nudgedPathUp.TotalSteps;
                        }
                    }
                    if(helperNode.Right != null)
                    {
                        Answer nudgedPathRight = this.nudge(helperNode.Right, kvp.Key, color, destination, maxSteps);
                        if(nudgedPathRight != null)
                        {
                            bestAnswer = nudgedPathRight;
                            maxSteps = nudgedPathRight.TotalSteps;
                        }

                    }
                    if(helperNode.Down != null)
                    {
                        Answer nudgedPathDown = this.nudge(helperNode.Down, kvp.Key, color, destination, maxSteps);
                        if(nudgedPathDown != null)
                        {
                            bestAnswer = nudgedPathDown;
                            maxSteps = nudgedPathDown.TotalSteps;
                        }

                    }
                    if(helperNode.Left != null)
                    {
                        Answer nudgedPathLeft = this.nudge(helperNode.Left, kvp.Key, color, destination, maxSteps);
                        if(nudgedPathLeft != null)
                        {
                            bestAnswer = nudgedPathLeft;
                            maxSteps = nudgedPathLeft.TotalSteps;
                        }

                    }
                }
            }
            return bestAnswer;
        }
        public Answer nudge(Node node, string nudgeColor, string targetColor, int[] destination, int maxSteps)
        {
            Dictionary<string, int[]> nudgedRobot = new Dictionary<string, int[]>();
            foreach (var kvp in this.robots)
            {
                if (kvp.Key == nudgeColor)
                    {
                        nudgedRobot[kvp.Key] = new int[] { node.Row, node.Column };
                    }
                    else
                    {
                        nudgedRobot[kvp.Key] = new int[] { kvp.Value[0], kvp.Value[1] 
                    };
                }
            }
            Board newboard = new Board(this.x, this.y, this.walls, nudgedRobot);
            Node newBoardDestination = newboard.Matrix[destination[0], destination[1]];
            Dictionary<Node, Object[]> nudgedPath = newboard.oneRobotPath(targetColor, destination, maxSteps);
            if(nudgedPath.ContainsKey(newBoardDestination) && (int) nudgedPath[newBoardDestination][0] < maxSteps-1)
            {
                int totalSteps = (int) nudgedPath[newBoardDestination][0] + 1;
                Dictionary<Node, Object[]> helperPath = new Dictionary<Node, Object[]>();
                Node helperNode = this.matrix[this.robots[nudgeColor][0], this.robots[nudgeColor][1]];
                Node targetRobot = this.matrix[this.robots[targetColor][0], this.robots[targetColor][1]];
                helperPath.Add(helperNode, new Object[] { 0, null });
                helperPath.Add(node, new Object[] { 1, helperNode });

                Answer nudgeAnswer = new Answer(nudgeColor, helperNode, node, targetColor, targetRobot, this.matrix[destination[0], destination[1]], helperPath, nudgedPath, totalSteps, 1, newboard, totalSteps - 1);
                return nudgeAnswer;
            }
            return null;
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
                        Dictionary<Node, Object[]> tracker = this.oneRobotPath(kvp.Key, hDestination, maxSteps);
                        if (tracker.ContainsKey(hDestNode))
                        {
                            heap.insert((int)tracker[hDestNode][0], kvp.Key, hDestination, tracker);
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
            RobotPriority bestHelper = null;
            Board secondaryBoard = this;
            Node secondaryDestination = destinationNode;
            string helperColor = "";
            int[] helperDestination = null;
            int helperStepCount = int.MaxValue;
            Dictionary<Node, Object[]> bestTargetRobotPath = new Dictionary<Node, Object[]>();
            while (heap.Count > 0)
            {
                RobotPriority helper = heap.remove();
                int helperSteps = helper.Steps;
                if (helper.Steps >= maxSteps - 1)
                {
                    break;
                }
                Dictionary<string, int[]> movedRobots = new Dictionary<string, int[]>();
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
                Board newboard = new Board(this.x, this.y, this.walls, movedRobots);
                Node newDN = newboard.Matrix[destination[0], destination[1]];
                Dictionary<Node, Object[]> helpedPath = newboard.oneRobotPath(color, destination);
                if (helpedPath.ContainsKey(newDN))
                {
                    int mainRobotSteps = (int)helpedPath[newDN][0];
                    if (mainRobotSteps + helperSteps < maxSteps)
                    {
                        bestHelper = helper;
                        bestTargetRobotPath = helpedPath;
                        maxSteps = mainRobotSteps + helperSteps;
                        helperColor = helper.Color;
                        helperDestination = helper.Destination;
                        helperStepCount = helperSteps;
                        secondaryBoard = newboard;
                        secondaryDestination = newDN;
                    }
                }
            }
            if (bestHelper == null)
            {
                return null;
            }

            Console.WriteLine($"Line 136, helped by {helperColor} to {helperDestination[0]}-{helperDestination[1]} in {helperStepCount}, the shortest path is {maxSteps}.");
            Node helperRobot = this.matrix[this.robots[helperColor][0], this.robots[helperColor][1]];
            Node helperDestinationNode = this.matrix[helperDestination[0], helperDestination[1]];
            Node targetRobot = this.matrix[this.robots[color][0], this.robots[color][1]];
            Answer answer = new Answer(helperColor, helperRobot, helperDestinationNode, color, targetRobot, secondaryDestination, bestHelper.Path, bestTargetRobotPath, maxSteps, helperStepCount, secondaryBoard, maxSteps - helperStepCount);
            return answer;
        }

        // tracker is {destination node: [int steps, previous node]}
        public Dictionary<Node, Object[]> oneRobotPath(string color, int[] destination, int maxSteps = -1)
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
                            //    Console.WriteLine($"found the path for {color} robot to {destinationNode.Name}");
                            return tracker;
                        }
                        queue.insert(nextStop);
                    }
                }
            }
            //    Console.WriteLine("unreachable");
            return tracker;

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
