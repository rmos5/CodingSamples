using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public class TraverseNode : ITraverseNode
    {
        public TraverseNode(ITraverseNode parentNode)
        {
            Parent = parentNode;
            SequenceNr = -1;

            if (Parent != null)
            {
                Level = Parent.Level + 1;
                Parent.AddChild(this);
            }
        }

        public void AddChild(ITraverseNode node)
        {
            node.Parent = this;
            children.Add(node);
        }

        internal bool RemoveChild(TraverseNode child)
        {
            bool result = children.Remove(child);
            if (result)
            {
                child.Level = 0;
                child.Parent = null;
            }
            return result;
        }

        internal void RemoveChildren()
        {
            foreach (TraverseNode t in children.ToList())
                RemoveChild(t);
        }

        public override string ToString()
        {
            return string.Format("Level={0};SequenceNumber={1}", Level, SequenceNr);
        }


        public ITraverseNode GetChildAt(int index)
        {
            return children[index];
        }

        public int IndexOfChild(ITraverseNode node)
        {
            return children.IndexOf(node);
        }

        public ITraverseNode Parent { get; set; }

        private IList<ITraverseNode> children = new List<ITraverseNode>();

        public IEnumerable<ITraverseNode> Children
        {
            get { return children; }
        }

        public int SequenceNr { get; set; }

        public int VisitCount { get; set; }

        public int Level { get; set; }

        public int ChildCount { get { return children.Count; } }
    }
}
