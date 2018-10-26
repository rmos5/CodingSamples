using System;
using System.Collections.Generic;

namespace Graphs
{
    public class TraverserBFS : TraverserBase<IVertex, IEdge>
    {
        public TraverserBFS(IVertex root) : base(root)
        {
        }

        #region BFS

        void TraverseBFS(IVertex vertex)
        {
            Queue<IVertex> queue = new Queue<IVertex>();

            int idx = 0;
            queue.Enqueue(vertex);

            OnTraverseStarted(vertex);

            IVertex vtx;

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

        private void ProcessChildren(IVertex vertex, ref int sequenceNr, Queue<IVertex> queue)
        {
            vertex.VisitCount++;
            OnVertexReached(vertex);
            foreach (IEdge e in vertex.Children)
                ProcessEdge(e, ref sequenceNr, queue);
        }

        private void ProcessVertex(IVertex vertex, ref int sequenceNr, Queue<IVertex> queue)
        {
            vertex.VisitCount++;
            if (vertex.VisitCount == 1)
                vertex.SequenceNr = ++sequenceNr;
            OnVertexReached(vertex);
        }

        private void ProcessEdge(IEdge edge, ref int sequenceNr, Queue<IVertex> queue)
        {
            edge.VisitCount++;
            if (edge.VisitCount == 1)
                edge.SequenceNr = ++sequenceNr;
            OnEdgeReached(edge);
            if (edge.Next != null)
                queue.Enqueue((IVertex)edge.Next);
        }

        protected override void OnTraverseStarted(IVertex vertex)
        {
            throw new NotImplementedException();
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

        #endregion
    }
}