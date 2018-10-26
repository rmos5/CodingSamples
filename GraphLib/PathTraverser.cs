using System;
using System.Collections.Generic;

namespace Graphs
{
    public class PathTraverser<V, E>
        where V : IVertex
        where E : IEdge
    {
        public PathTraverser()
        {
            Traverser = new Traverser<V, E>();
        }

        public Traverser<V, E> Traverser { get; set; }

        public IEnumerator<ITraverseNode> GetPaths(V vertex)
        {
            throw new NotImplementedException();
        }
    }
}
