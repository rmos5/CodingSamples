
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public class Edge : TraverseNode, IEdge
    {
        public Edge(IVertex parentNode)
            : base(parentNode)
        {
        }

        public void SetNext(IVertex vertex)
        {
            Next = vertex;
        }

        public new IVertex Parent { get { return base.Parent as IVertex; } }

        public IVertex Next { get; private set; }

        public override string ToString()
        {
            return string.Format("E {0}", base.ToString());
        }

        public new IEnumerable<IVertex> Children
        {
            get { return base.Children.Cast<IVertex>(); }
        }
    }
}