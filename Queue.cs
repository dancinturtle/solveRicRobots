using System;
using System.Collections.Generic;

namespace RicRobots
{
    class Queue
    {
        private QueueNode head;
        public QueueNode Head
        {
            get => head;
        }
        private QueueNode tail;
        public QueueNode Tail
        {
            get => tail;
        }
        public Queue()
        {
            this.head = null;
            this.tail = null;
        }

        public Queue insert(Node val)
        {
            QueueNode newNode = new QueueNode(val);
            if(this.head == null)
            {
                this.head = newNode;
                this.tail = this.head;
                return this;
            }
            this.tail.Next = newNode;
            this.tail = this.tail.Next;
            return this;
        }
        public QueueNode dequeue()
        {
            if(this.head == null)
            {
                return null;
            }
            QueueNode result = this.head;
            if (this.head == this.tail)
            {
                this.head = null;
                this.tail = null;
                return result;
            }
            this.head = this.head.Next;
            return result;
        }

    }

}
