using System;
using System.Collections.Generic;

namespace RicRobots
{
    class MinHeap
    {
        private List<RobotPriority> data;
        public List<RobotPriority> Data
        {
            get => data;
        }
        public int Count
        {
            get => data.Count;
        }
        public MinHeap()
        {
            this.data = new List<RobotPriority>();
        }
        public void insert(int steps, string color, int[] destination)
        {
            RobotPriority newPri = new RobotPriority(steps, color, destination);
            this.data.Add(newPri);
            int c = this.data.Count - 1;
            int p = 0;
            if (c % 2 == 0)
            {
                p = (c - 2) / 2;
            }
            else
            {
                p = (c - 1) / 2;
            }

            while (p >= 0 && this.data[c].Steps < this.data[p].Steps)
            {
                this.swapper(c, p);
                c = p;
                if (c % 2 == 0)
                {
                    p = (c - 2) / 2;
                }
                else
                {
                    p = (c - 1) / 2;
                }
            }

        }
        public RobotPriority remove()
        {
            if(this.data.Count == 0){
                return null;
            }
            RobotPriority obj = this.data[0];
            int len = this.data.Count;
            this.data[0] = this.data[len - 1];
            this.data.RemoveAt(len-1);
            if(this.data.Count == 0){
                return obj;
            }
            int pIdx = 0;
            int c1 = 1;
            int c2 = 2;

            RobotPriority pVal = this.data[0];
            while(c1 < this.data.Count){
                
                if(c2 < this.data.Count){
                    int smaller;
                    if(this.data[c1].Steps < this.data[c2].Steps){
                        smaller = c1;
                    }
                    else {
                        smaller = c2;
                    }
                    if(this.data[smaller].Steps < pVal.Steps){
                        this.swapper(smaller, pIdx);
                        pIdx = smaller;
                        c1 = pIdx*2 + 1;
                        c2 = pIdx*2 + 2;
                        
                    }
                    else {
                        return obj;
                    }
                }
                else if (this.data[c1].Steps < pVal.Steps){
                        this.swapper(c1, pIdx);
                        pIdx = c1;
                        c1 = pIdx * 2 + 1;
                        c2 = pIdx * 2 + 2;
                        
                }
                else {
                    return obj;
                }
             }
             return obj;
           
        }
        public void printMyHeaps()
        {
            foreach(var obj in this.data){
                Console.WriteLine("Robot color: " + obj.Color + " Destination: " + obj.Destination[0] + " " + obj.Destination[1] + " Steps: " + obj.Steps);
            }
        }

        public void swapper(int idx1, int idx2)
        {
            RobotPriority temp = this.data[idx1];
            data[idx1] = data[idx2];
            data[idx2] = temp;
        }
    }


}
