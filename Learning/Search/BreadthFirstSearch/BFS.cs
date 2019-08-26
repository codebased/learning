using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Search.BreadthFirstSearch
{

    public class BFS
    {
        // It is Graph traversal strategy 
        /*
         * The major difference between BFS and DFS is that BFS proceeds level by level 
         * while DFS follows first a path form the starting to the ending node (vertex), 
         * then another path from the start to end, and so on until all nodes are visited. 
         * Furthermore, BFS uses the queue for storing the nodes whereas DFS uses the stack for traversal of the nodes.
         * Key Differences Between BFS and DFS

         *  BFS is vertex-based algorithm while DFS is an edge-based algorithm.
         *   Queue data structure is used in BFS. On the other hand, DFS uses stack or recursion.
         *   Memory space is efficiently utilized in DFS while space utilization in BFS is not effective.
         *   BFS is optimal algorithm while DFS is not optimal.
         *   DFS constructs narrow and long trees. As against, BFS constructs wide and short tree.

         *   REF: https://techdifferences.com/difference-between-bfs-and-dfs.html
         *   
         *   Usage: 
         *   BFS is also used in the famous Dijkstra’s algorithm for computing the shortest path in a graph and the Ford-Fulkerson 
         *   algorithm for computing the maximum​ flow in a flow network.
         * */
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
