namespace Graphs
{
    public class TraverserDFS : TraverserBase<IVertex, IEdge>
    {
        public TraverserDFS(IVertex root)
            : base(root)
        {
        }

        protected ITraverseNode GetChild()
        {
            ITraverseNode result = null;

            if (Current == null)
                result = Root;

            else if (Current is IVertex)
            {
                IVertex v = (IVertex)Current;
                result = GetChild(v, ChildIndex);
                ChildIndex++;
                if (ChildIndex == v.ChildCount)
                    ChildIndex = 0;
            }
            else if (Current is IEdge)
            {
                IEdge e = (IEdge)Current;
                result = e.Next;
            }

            return result;
        }

        private ITraverseNode GetChild(IVertex vertex, int index)
        {
            return vertex.GetChildAt(index);
        }

        protected override void OnTraverseStarted(IVertex vertex)
        {
        }

        protected override void OnVertexReached(IVertex verteex)
        {
        }

        protected override void OnEdgeReached(IEdge edge)
        {
        }

        protected override void OnPathCompleted(ITraverseNode node)
        {
        }

        protected override void OnTraverseCompleted()
        {
        }

        int ChildIndex { get; set; }
    }
}
