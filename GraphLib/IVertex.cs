using System.Collections.Generic;
namespace Graphs
{
    public interface IVertex : ITraverseNode
    {
        new IEdge Parent { get; }
        new IEnumerable<IEdge> Children { get; }
    }
}
