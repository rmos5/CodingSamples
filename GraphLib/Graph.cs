using System.Collections.Generic;

namespace GraphLib
{
    public class Graph : LinkedList<Graph>
    {
        public Graph Previous { get; private set; }

        public Graph Next { get; private set; }

        public void SetPrevious(Graph node)
        {
            //todo: implement
        }

        public void SetNext(Graph node)
        {
            //todo: implement
        }
    }
}
