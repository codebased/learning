using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Search.DepthFirstSearch
{
    public class DFS
    {
        public List<T> VisitNodes<T>(Graph<T> graph, T start )  
        {
            var visitedNodes = new List<T>();
 
            if (!graph.NodeList.ContainsKey(start))
            {
                return visitedNodes;
            }

            var stack = new ConcurrentStack<T>();
            stack.Push(start);
            while (stack.Count > 0)
            {
                if (stack.TryPop(out T node) && !visitedNodes.Contains(node))
                {
                    visitedNodes.Add(node);
                    foreach (var childNodes in graph.NodeList[node])
                    {
                        if (!visitedNodes.Contains(childNodes))
                        {
                            stack.Push(childNodes);
                        }
                    }
                }
            }

            return visitedNodes;

        }
    }

    public class Graph<T>
    {
        private readonly ConcurrentDictionary<T, HashSet<T>> _graph;

        public Graph()
        {
            _graph = new ConcurrentDictionary<T, HashSet<T>>();
        }

        public ConcurrentDictionary<T, HashSet<T>> NodeList
        {
            get
            {
                return _graph;
            }
        }
        public Graph(IEnumerable<T> nodes, IEnumerable<Tuple<T, T>> edges) : this()
        {

            foreach (var item in nodes)
            {
                // adding all nodes/vertex
                _graph.TryAdd(item, new HashSet<T>());
            }

            foreach (var edge in edges)
            {
                if (_graph.ContainsKey(edge.Item1) && _graph.ContainsKey(edge.Item2))
                {
                    _graph[edge.Item1].Add(edge.Item2);
                    _graph[edge.Item2].Add(edge.Item1);
                }
            }
        }

    }
}
