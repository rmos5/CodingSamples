using System.Collections;
using System.Collections.Generic;

namespace Graphs
{
    public abstract class TraverserBase<V, E> : IEnumerator<ITraverseNode> where V : IVertex where E : IEdge
    {
        protected abstract void OnTraverseStarted(V vertex);
        protected abstract void OnVertexReached(V verteex);
        protected abstract void OnEdgeReached(E edge);
        protected abstract void OnPathCompleted(ITraverseNode node);
        protected abstract void OnTraverseCompleted();

        public TraverserBase(V root)
        {
            Root = root;
        }

        public V Root { get; private set; }

        private ITraverseNode current;

        public ITraverseNode Current
        {
            get { return current; }
            set { current = value; }
        }

        private ITraverseNode previous;

        public ITraverseNode Previous
        {
            get { return previous; }
            private set { previous = value; }
        }

        public virtual void Dispose()
        {
        }

        object IEnumerator.Current
        {
            get { return current; }
        }

        public bool MoveNext()
        {
            Previous = Current;

            ITraverseNode cnode = null;

            if (Current is IVertex)
            {
                cnode = ProcessVertex((IVertex)Current);
            }
            else if (Current is IEdge)
            {
                cnode = ProcessEdge((IEdge)Current);
            }

            if (cnode == null)
            {

            }
            else
                Current = cnode;

            return current != null;
        }

        private IVertex ProcessEdge(IEdge edge)
        {
            return edge.Next;
        }

        private IEdge ProcessVertex(IVertex vertex)
        {
            IEdge result = null;
            if (vertex.ChildCount > 0)
            {
                result = GetChild(vertex, CurrentChildIndex);
                CurrentChildIndex++;
                if (CurrentChildIndex == vertex.ChildCount)
                {
                    CurrentChildIndex = 0;
                }
            }

            return result;
        }

        private IEdge GetChild(IVertex vertex, int index)
        {
            return (IEdge)vertex.GetChildAt(index);
        }

        public virtual void Reset()
        {
            Reset(Root);
            Previous = null;
            Current = null;
            CurrentChildIndex = 0;
        }

        private void Reset(ITraverseNode node)
        {
            node.SequenceNr = -1;
            node.VisitCount = 0;
            foreach (ITraverseNode tn in node.Children)
                Reset(tn);
        }

        int CurrentChildIndex { get; set; }
    }
}
