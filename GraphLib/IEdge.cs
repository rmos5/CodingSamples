using System.Collections.Generic;
namespace Graphs
{
    public interface IEdge : ITraverseNode
    {
        IVertex Next { get; }
        void SetNext(IVertex vertex);
        new IEnumerable<IVertex> Children { get; }
    }
}
