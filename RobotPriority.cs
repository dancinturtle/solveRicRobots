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
        private int[] origin;
        public int[] Origin
        {
            get => origin;
        }
        int[] destination {get; set;}
        public int[] Destination
        {
            get => destination;
        }
        public RobotPriority(int steps, int[] origin, int[] destination)
        {
            this.steps = steps;
            this.origin = origin;
            this.destination = destination;
        }

    }
}