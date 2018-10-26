using System.Collections.Generic;

namespace Graphs
{
    public interface ITraverseNode
    {
        ITraverseNode Parent { get; set; }
        IEnumerable<ITraverseNode> Children { get; }
        int Level { get; }
        int SequenceNr { get; set; }
        int VisitCount { get; set; }
        void AddChild(ITraverseNode node);
        ITraverseNode GetChildAt(int index);
        int ChildCount { get; }
        int IndexOfChild(ITraverseNode node);
    }
}