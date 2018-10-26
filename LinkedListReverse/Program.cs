using System;

namespace LinkedListReverse
{
    class Program
    {
        class NodeBase<T>
        {
            public T Value { get; }
            public NodeBase<T> Next { get; set; }

            public NodeBase(NodeBase<T> previous, T value)
            {
                if (previous != null)
                    previous.Next = this;
                this.Value = value;
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        static NodeBase<int> CreateLinkedList(int count)
        {
            if (count < 1)
                return null;

            NodeBase<int> result = new NodeBase<int>(null, 1);
            NodeBase<int> temp = result;

            for (int i = 2; i <= count; i++)
            {
                temp = new NodeBase<int>(temp, i);
            }

            return result;
        }
        
        static void Main(string[] args)
        {
            int nodeCount = 10;

            var data = CreateLinkedList(nodeCount);

            PrintData(data);

            Console.WriteLine();

            var head = data;
            NodeBase<int> next = head?.Next, temp = null;
            if (head != null)
                head.Next = null;

            // make reverse
            while(next != null)
            {
                temp = next.Next;
                next.Next = head;
                head = next;
                next = temp;
            }

            PrintData(head);

            Console.WriteLine();

            Console.ReadLine();
        }

        private static void PrintData<T>(NodeBase<T> data)
        {
            var node = data;
            while (node != null)
            {
                Console.Write(node + "->");
                node = node.Next;
            }
        }
    }
}
