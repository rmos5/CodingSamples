using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public partial class Node
    {
        public bool IsVisited { get; private set; }

        public Node Parent { get; set; }

        private IList<Node> SubNodesList { get; } = new List<Node>();
        public virtual IEnumerable<Node> SubNodes { get { return SubNodesList; } }

        public Node Next { get; private set; }

        public Node Previous { get; private set; }

        /// <summary>
        /// TODO: write appropriate comment. Currently there is no certainty of chain elements. 
        /// </summary>
        /// <returns>Collection of <see cref="Node"/> objects.</returns>
        public IEnumerable<Node> GetChain()
        {
            IList<Node> result = new List<Node> { this };
            foreach (Node w in LeftAncestorPath)
                result.Insert(0, w);

            foreach (Node w in RightAncestorPath)
                result.Add(w);

            return result;
        }

        public Node LeftAncestor
        {
            get
            {
                return LeftAncestorPath.LastOrDefault() ?? this;
            }
        }

        public Node RightAncestor
        {
            get
            {
                return RightAncestorPath.LastOrDefault() ?? this;
            }
        }

        public Node Head
        {
            get
            {
                return HeadPath.LastOrDefault() ?? this;
            }
        }

        public Node Tail
        {
            get
            {
                return TailPath.LastOrDefault() ?? this;
            }
        }

        public Node LeftParent
        {
            get
            {
                return Head.Parent;
            }
        }

        public Node RightParent
        {
            get
            {
                return Tail.Parent;
            }
        }

        public bool IsRoot { get { return LeftAncestor == this; } }

        public bool IsTail { get { return Tail == this; } }

        public bool IsHead { get { return Tail == this; } }

        public bool IsOrphan
        {
            get
            {
                return Head == this && Tail == this && SubNodesList.Count == 0;
            }
        }

        public IEnumerable<Node> HeadPath
        {
            get
            {
                IList<Node> result = new List<Node>();
                Node item = Previous;
                while (item != null && !item.IsVisited)
                {
                    item.IsVisited = true;
                    result.Add(item);
                    item = item.Previous;
                }

                foreach (Node w in result)
                    w.IsVisited = false;

                return result;
            }
        }

        public IEnumerable<Node> TailPath
        {
            get
            {
                IList<Node> result = new List<Node>();
                Node item = Next;
                while (item != null && !item.IsVisited)
                {
                    item.IsVisited = true;
                    result.Add(item);
                    item = item.Next;
                }

                foreach (Node w in result)
                    w.IsVisited = false;

                return result;
            }
        }

        public IEnumerable<Node> LeftAncestorPath
        {
            get
            {
                IList<Node> result = new List<Node>();

                foreach (Node w in HeadPath)
                {
                    result.Add(w);
                }

                Node item = Head.Parent;
                while (item != null && !item.IsVisited)
                {
                    item.IsVisited = true;
                    result.Add(item);
                    foreach (Node w in item.HeadPath)
                    {
                        w.IsVisited = true;
                        result.Add(w);
                    }

                    item = item.Head.Parent;
                }

                foreach (Node w in result)
                    w.IsVisited = false;

                return result;
            }
        }

        public IEnumerable<Node> RightAncestorPath
        {
            get
            {
                IList<Node> result = new List<Node>();
                foreach (Node w in TailPath)
                {
                    result.Add(w);
                }

                Node item = Tail.Parent;
                Node item2 = Head.Parent;
                if (item != null && item != item2)
                {
                    while (item != null && !item.IsVisited)
                    {
                        item.IsVisited = true;
                        result.Add(item);
                        foreach (Node w in item.TailPath)
                        {
                            w.IsVisited = true;
                            result.Add(w);
                        }
                        item = item.Tail.Parent;
                    }
                }

                foreach (Node w in result)
                    w.IsVisited = false;

                return result;
            }
        }

        public Node SubnodesSource
        {
            get
            {
                if (SubNodes.Count() > 0)
                    return this;

                Node result = this;

                foreach (Node w in HeadPath)
                    if (w.SubNodes.Count() > 0)
                    {
                        result = w;
                        break;
                    }

                return result;
            }
        }

        public bool HasSubnodes { get { return SubNodesList.Count > 0; } }

        public void SetParent(Node node)
        {
            if (this == node)
                throw new ArgumentException("Circular reference detected.");

            Parent = node;
        }

        public void Connect(Node previous, Node next)
        {
            previous = next;
            previous.Next = next;
            next.Previous = previous;
            next.Previous = previous;
        }

        protected void DisconnectFromNext()
        {
            Next.Previous = null;
            Next = null;
        }

        protected void DisconnectFromPrevious()
        {
            Previous = null;
        }

        public void Disconnect(bool reconnectPath, bool fromParent = false)
        {
            if (Previous != null)
                DisconnectFromPrevious();

            if (Next != null)
                DisconnectFromNext();

            if (reconnectPath)
            {
                if (Next != null && Previous != null)
                {
                    Next = Previous;
                    Previous.Next = Next;
                }
            }

            if (fromParent && Parent != null)
                Parent = null;
        }

        /// <summary>
        /// Inserts <paramref name="node"/> before this node.
        /// </summary>
        /// <param name="node">Node to be inserted.</param>
        /// <remarks>Clears <see cref="Previous"/> if passed in <paramref name="node"/> is null.</remarks>
        public void Insert(Node node)
        {
            if (node == null)
            {
                if (Previous != null)
                    DisconnectFromPrevious();
                return;
            }

            //TODO: revise parents/siblings/child handling
            if (this == node)
                throw new NotSupportedException("Inserting the same node is prohibited.");

            // already connected
            if (Previous == node)
            {
                return;
            }

            if (HeadPath.Contains(node) || TailPath.Contains(node))
            {
                node.Disconnect(true);
            }
            else
            {
                // connect to head
                if (Previous == null)
                {
                    if (node.Next != null)
                        node.DisconnectFromNext();
                }
                else
                {
                    node.Disconnect(true);
                }
            }

            if (Previous != null)
            {
                // connect to previous
                DisconnectFromPrevious();
                Connect(Previous, node);
            }

            Connect(node, this);
        }

        /// <summary>
        /// Appends <paramref name="node"/> after this node.
        /// </summary>
        /// <param name="node">Node to be appended.</param>
        /// <remarks>Clears <see cref="Next"/> if passed <paramref name="node" is null./></remarks>
        public void Append(Node node)
        {
            if (node == null)
            {
                if (Next != null)
                    DisconnectFromNext();
                return;
            }

            //TODO: revise parents/siblings/child handling
            if (this == node)
                throw new NotSupportedException("Appending the same node is prohibited.");

            // already connected
            if (Next == node)
            {
                return;
            }

            if (HeadPath.Contains(node) || TailPath.Contains(node))
            {
                // insert from head path
                node.Disconnect(true);
            }
            else
            {
                // append to head
                if (Next == null)
                {
                    if (node.Previous != null)
                        node.DisconnectFromPrevious();
                }
                else
                {
                    node.Disconnect(true);
                }
            }

            if (Next != null)
            {
                // connect to next
                DisconnectFromNext();
                Connect(node, Next);
            }

            Connect(this, node);
        }

        public void Replace(Node node)
        {
            //TODO: implement when appropriate
            throw new NotImplementedException();
        }
    }
}