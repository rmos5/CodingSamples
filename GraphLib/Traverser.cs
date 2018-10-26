using System.Collections.Generic;

namespace Graphs
{
    public class Traverser<V, E>
        where V : IVertex
        where E : IEdge
    {
        protected virtual void OnTraverseStarted(V vertex) { }
        protected virtual void OnVertexReached(V vertex) { }
        protected virtual void OnEdgeReached(E edge) { }
        protected virtual void OnPathCompleted(ITraverseNode node) { }
        protected virtual void OnTraverseCompleted() { }

        public void Traverse(V vertex, TraverseMode mode)
        {
            Reset(vertex);
            switch (mode)
            {
                case TraverseMode.DepthFirst:
                    TraverseDFS(vertex);
                    break;
                default:
                    TraverseBFS(vertex);
                    break;
            }
        }

        public void ResetSequenceNrs(ITraverseNode node)
        {
            node.SequenceNr = -1;
            foreach (ITraverseNode n in node.Children)
                ResetSequenceNrs(n);
        }

        public void ResetVisitCount(ITraverseNode node)
        {
            node.VisitCount = 0;
            foreach (ITraverseNode n in node.Children)
                ResetVisitCount(n);
        }

        public void Reset(ITraverseNode node)
        {
            ResetSequenceNrs(node);
            ResetVisitCount(node);
        }

        #region DFS

        void TraverseDFS(V vertex)
        {
            int nr = 0;

            vertex.SequenceNr = nr;

            OnTraverseStarted(vertex);

            ProcessVertex(vertex, ref nr);

            OnTraverseCompleted();
        }

        private void ProcessVertex(V vertex, ref int sequenceNr)
        {
            vertex.VisitCount++;
            vertex.SequenceNr = ++sequenceNr;
            OnVertexReached(vertex);

            if (vertex.ChildCount == 0)
            {
                OnPathCompleted(vertex);
                return;
            }

            foreach (E e in vertex.Children)
            {
                ProcessEdge(e, ref sequenceNr);

                if (e.Next != null)
                    ProcessVertex((V)e.Next, ref sequenceNr);
            }
        }

        private void ProcessEdge(E edge, ref int sequenceNr)
        {
            edge.VisitCount++;
            edge.SequenceNr = ++sequenceNr;
            OnEdgeReached(edge);

            if (edge.Next == null)
                OnPathCompleted(edge);
        }

        #endregion

        #region BFS

        void TraverseBFS(V vertex)
        {
            Queue<V> queue = new Queue<V>();

            int idx = 0;
            queue.Enqueue(vertex);

            OnTraverseStarted(vertex);

            V vtx;

            while (queue.Count > 0)
            {
                vtx = queue.Dequeue();

                if (vtx.VisitCount > 0)
                    ProcessChildren(vtx, ref idx, queue);
                else
                {
                    ProcessVertex(vtx, ref idx, queue);
                    queue.Enqueue(vtx);
                }
            }

            OnTraverseCompleted();
        }

        private void ProcessChildren(V vertex, ref int sequenceNr, Queue<V> queue)
        {
            vertex.VisitCount++;
            OnVertexReached(vertex);
            foreach (E e in vertex.Children)
                ProcessEdge(e, ref sequenceNr, queue);
        }

        private void ProcessVertex(V vertex, ref int sequenceNr, Queue<V> queue)
        {
            vertex.VisitCount++;
            if (vertex.VisitCount == 1)
                vertex.SequenceNr = ++sequenceNr;
            OnVertexReached(vertex);
        }

        private void ProcessEdge(E edge, ref int sequenceNr, Queue<V> queue)
        {
            edge.VisitCount++;
            if (edge.VisitCount == 1)
                edge.SequenceNr = ++sequenceNr;
            OnEdgeReached(edge);
            if (edge.Next != null)
                queue.Enqueue((V)edge.Next);
        }

        #endregion
    }
}
