using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Search.BreadthFirstSearch
{

    public class BFS
    {
        public List<T> Traverse<T>(Graph<T> graph, T start)
        {
            var visitedNodes = new List<T>();

            if (!graph.NodeList.ContainsKey(start))
            {
                return visitedNodes;
            }

            var queue = new ConcurrentQueue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                if (queue.TryDequeue(out T node) && !visitedNodes.Contains(node))
                {
                    visitedNodes.Add(node);
                    foreach (var childNodes in graph.NodeList[node])
                    {
                        if (!visitedNodes.Contains(childNodes))
                        {
                            queue.Enqueue(childNodes);
                        }
                    }
                }
            }

            return visitedNodes;

        }
    }
}
