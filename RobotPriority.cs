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
        private string color;
        public string Color
        {
            get => color;
        }
        int[] destination {get; set;}
        public int[] Destination
        {
            get => destination;
        }
        public RobotPriority(int steps, string color, int[] destination)
        {
            this.steps = steps;
            this.color = color;
            this.destination = destination;
        }

    }
}