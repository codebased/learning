﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Search.DepthFirstSearch
{
    // depth-first search aggresively follows a path until it can't go any futher and then backtracks a bit and continues to aggressively follow the next available path.
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
}
