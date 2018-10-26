using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public class Vertex : TraverseNode, IVertex
    {
        public Vertex(IEdge parentNode)
            : base(parentNode)
        {
        }

        public new IEdge Parent { get { return base.Parent as IEdge; } }

        public override string ToString()
        {
            return string.Format("V {0}", base.ToString());
        }

        public new IEnumerable<IEdge> Children
        {
            get { return base.Children.Cast<IEdge>(); }
        }
    }
}
