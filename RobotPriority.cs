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
        private Dictionary<string, Object[]> robotPath;
        public Dictionary<string, Object[]> RobotPath
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
        public RobotPriority(int steps, string color, int[] destination, Dictionary<string, Object[]> path)
        {
            this.steps = steps;
            this.color = color;
            this.robotPath = path;
            this.destination = destination;
        }

    }
}