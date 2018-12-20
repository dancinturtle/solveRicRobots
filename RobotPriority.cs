using System;
using System.Collections.Generic;

namespace RicRobots {
    class RobotPriority 
    {
        private int steps;
        public int Steps
        { 
            get => steps;
            set => steps = value;
        }
        private Dictionary<Node, Object[]> robotPath;
        public Dictionary<Node, Object[]> RobotPath
        {
            get => robotPath;
        }
        private string color;
        public string Color
        {
            get => color;
        }
        private int[] destination;
        public int[] Destination
        {
            get => destination;
        }
        public RobotPriority(int steps, string color, int[] destination, Dictionary<Node, Object[]> path)
        {
            this.steps = steps;
            this.color = color;
            this.robotPath = path;
            this.destination = destination;
        }

    }
}