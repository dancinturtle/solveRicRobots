using System;
using System.Collections.Generic;

namespace RicRobots
{
    class QueueNode
    {
        private Node val;
        public Node Val
        {
            get => val;
        }
        private QueueNode next;
        public QueueNode Next
        {
            get => next;
            set => next = value;
        }
        public QueueNode(Node val)
        {
            this.val = val;
            this.next = null;
        }
        
    }

}
