using System;

namespace RicRobots
{
    class Node
    {
        private string name;
        public string Name 
        {
            get => name; 
        }
        private int row;
        public int Row 
        {
            get => row;
        }
        private int column;
        public int Column 
        {
            get => column;
        }
    
        private Node up;
        public Node Up
        {
            get => up;
            set => up = value;
        }
        private Node right;
        public Node Right
        {
            get => right;
            set => right = value;
        }
        private Node down;
        public Node Down
        {
            get => down;
            set => down = value;
        }
        private Node left;
        public Node Left
        {
            get => left;
            set => left = value;
        }
        private bool occupied;
        public bool Occupied
        {
            get => occupied;
            set => occupied = value;
        }
        private string color;
        public string Color
        {
            get => color != null ? color : "no robot";
            set => color = value;
        }
        public Node(int row, int column)
        {
            string name = row + "-" + column;
            this.name = name;
            this.row = row;
            this.column = column;
            this.up = null;
            this.right = null;
            this.down = null;
            this.left = null;
            this.occupied = false;
            this.color = null;
        }
        public void addWalls(string[] walls)
        {
            foreach(string wall in walls)
            {
                if(wall == "up" && this.up != null)
                {
                    this.up.Down = null;
                    this.up = null;
                }
                else if (wall=="right" && this.right != null)
                {
                    this.right.Left = null;
                    this.right = null;
                }
                else if (wall == "down" && this.down != null)
                {
                    this.down.Up = null;
                    this.down = null;
                }
                else if (wall == "left" && this.left != null)
                {
                    this.left.Right = null;
                    this.left = null;
                }
            }
        }
    }
}
