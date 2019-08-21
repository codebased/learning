using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Learning.Search.DepthFirstSearch
{
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
