using Learning.Search.DepthFirstSearch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LearningTests
{
    [TestClass]
    public class DFSTests
    {
        public class Node
        {
            public int Info { get; set; }
            public Node()
            {


            }

            public override string ToString()
            {
                return Info.ToString();
            }
        }

        [TestMethod]
        public void DFS_Traverse()
        {

            var dfs = new DFS();

            var nodeList = new List<Node>();

            nodeList.Add(new Node { Info = 1 });
            nodeList.Add(new Node { Info = 2 });
            nodeList.Add(new Node { Info = 3 });

            var edges = new List<Tuple<Node, Node>>();
            edges.Add(new Tuple<Node, Node>(nodeList[0], nodeList[1]));
            edges.Add(new Tuple<Node, Node>(nodeList[0], nodeList[2]));

            var visitedNodes = dfs.VisitNodes(new Graph<Node>(nodeList, edges), nodeList[0]);
            foreach (var node in visitedNodes)
            {
                Console.WriteLine(node.ToString());
            }
        }

        [TestMethod]
        public void DFS_()
        {
            var vertices = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var edges = new[]{Tuple.Create(1,2), Tuple.Create(1,3),
                Tuple.Create(2,4), Tuple.Create(3,5), Tuple.Create(3,6),
                Tuple.Create(4,7), Tuple.Create(5,7), Tuple.Create(5,8),
                Tuple.Create(5,6), Tuple.Create(8,9), Tuple.Create(9,10)};

            var graph = new Graph<int>(vertices, edges);
            var dfs = new DFS();
            // Dive Deep: 1 -> 3 -> 6 -> 5 -> 8 -> 9 -> 10, and then backtrack to 5 and dive deep again: 7 -> 4-> 2.
            Console.WriteLine(string.Join(", ", dfs.VisitNodes(graph, 1)));
        }

    }
}
