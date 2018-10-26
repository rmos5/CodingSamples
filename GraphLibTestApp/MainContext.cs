using Graphs;
using System;
using System.Collections.Generic;

namespace GraphLibTestApp
{
    public class MainContext : ContextBase
    {
        public void GenerateGraph()
        {
            Vertex result = CreateVertex(null);
            if (result.Level < MaxLevels)
                CreateEdges(result, MaxEdgeCount, MaxLevels);

            Graph = result;
            RefreshGraphs();
        }

        private static void ThrowNodeNotSelected()
        {
            throw new ArgumentException("There must be a node selected.");
        }

        private static void ThrowNodeNotVertex()
        {
            throw new ArgumentException("Node must be a vertex.");
        }

        public void Reset()
        {
            if (SelectedNode == null)
                ThrowNodeNotSelected();
            Traverser.Reset(SelectedNode);
            RefreshGraphs();
        }

        public void GetPaths()
        {
            if (SelectedNode as IVertex == null)
                ThrowNodeNotVertex();

            //PathTraverser.FindPaths((Vertex)SelectedNode);
            //IList<LinkedList<ITraverseNode>> pp = new List<LinkedList<ITraverseNode>>();
            //foreach (LinkedList<ITraverseNode> p in PathTraverser.Paths)
            //    pp.Add(p);

            //Paths = pp;

            RefreshGraphs();
        }

        // just to make property change to work.
        private void RefreshGraphs()
        {
            Graphs = null;
            Graphs = new List<Vertex> { Graph };
        }

        Random rnd = new Random();

        private void CreateEdges(Vertex vertex, int maxEdges, int maxLevels)
        {
            Edge edge;
            int ec = rnd.Next(0, maxEdges);
            if (vertex.Parent == null && ec == 0)
            {
                do
                {
                    ec = rnd.Next(0, maxEdges);
                } while (ec == 0);
            }

            for (int i = 0; i < ec; i++)
            {
                edge = CreateEdge(vertex);

                if (edge.Next == null)
                    continue;

                if (edge.Next.Level >= maxLevels - 1)
                    continue;
                else
                    CreateEdges((Vertex)edge.Next, maxEdges, maxLevels);
            }
        }

        private Edge CreateEdge(Vertex vertex)
        {
            Edge result = new Edge(vertex);
            bool ok = (rnd.Next(1000) % 2) == 0;
            if (ok)
                result.SetNext(CreateVertex(result));
            return result;
        }

        private Vertex CreateVertex(IEdge edge)
        {
            return new Vertex(edge);
        }

        public void TraverseDFS()
        {
            if (SelectedNode == null)
                ThrowNodeNotSelected();
            if (!(SelectedNode is IVertex))
                ThrowNodeNotVertex();

            Traverser.Traverse((Vertex)SelectedNode, TraverseMode.DepthFirst);
            RefreshGraphs();
        }

        public void TraverseBFS()
        {
            if (SelectedNode == null)
                ThrowNodeNotSelected();
            if (!(SelectedNode is IVertex))
                ThrowNodeNotVertex();

            Traverser.Traverse((Vertex)SelectedNode, TraverseMode.BreadthFirst);
            RefreshGraphs();
        }

        private IList<TraverseNodeList> GetPaths(IEnumerable<LinkedList<ITraverseNode>> paths)
        {
            List<TraverseNodeList> result = new List<TraverseNodeList>();

            foreach (LinkedList<ITraverseNode> ll in paths)
                result.Add(GetList(ll));

            return result;
        }

        private TraverseNodeList GetList(LinkedList<ITraverseNode> path)
        {
            TraverseNodeList result = new TraverseNodeList();
            LinkedListNode<ITraverseNode> node = path.First;
            result.Add(node.Value);
            while ((node = node.Next) != null)
                result.Add(node.Value);

            return result;
        }

        private Traverser<Vertex, Edge> traverser = new DefaultTraverser();

        public Traverser<Vertex, Edge> Traverser
        {
            get { return traverser; }
            set { traverser = value; }
        }

        private Vertex graph;

        public Vertex Graph
        {
            get { return graph; }
            private set
            {
                if (value != graph)
                {
                    graph = value;
                    OnPropertyChanged("Graph");
                }
            }
        }

        private IEnumerable<Vertex> graphs;

        public IEnumerable<Vertex> Graphs
        {
            get { return graphs; }
            private set
            {
                if (value != Graphs)
                {
                    graphs = value;
                    OnPropertyChanged("Graphs");
                }
            }
        }

        private int maxLevels = 7;

        public int MaxLevels
        {
            get { return maxLevels; }
            set
            {
                if (value != MaxLevels)
                {
                    maxLevels = value;
                    OnPropertyChanged("MaxLevels");
                }
            }
        }

        private int maxEdgeCount = 5;

        public int MaxEdgeCount
        {
            get { return maxEdgeCount; }
            set
            {
                if (value != MaxEdgeCount)
                {
                    maxEdgeCount = value;
                    OnPropertyChanged("MaxEdgeCount");
                }
            }
        }

        private TraverseNode selectedNode;

        public TraverseNode SelectedNode
        {
            get { return selectedNode; }
            set
            {
                selectedNode = value;
                OnPropertyChanged("SelectedNode");
                IsVertexSelected = selectedNode != null && selectedNode is IVertex;
            }
        }

        private bool isVertexSelected;

        public bool IsVertexSelected
        {
            get { return isVertexSelected; }
            private set
            {
                isVertexSelected = value;
                OnPropertyChanged("IsVertexSelected");
            }
        }

        private IEnumerable<LinkedList<ITraverseNode>> paths;

        public IEnumerable<LinkedList<ITraverseNode>> Paths
        {
            get { return paths; }
            private set
            {
                paths = value;
                OnPropertyChanged("Paths");
                OnPropertyChanged("ExtractedPaths");
            }
        }

        public IEnumerable<TraverseNodeList> ExtractedPaths { get { return Paths == null ? null : GetPaths(Paths); } }
    }
}
