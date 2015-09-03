using System.Collections.Generic;
using UnityEngine;

public class Astar {

    private AstarMap _map;

    public Astar(AstarMap map) {
        _map = map;
    }

    public List<Node> FindPath(Node firstPosition, Node destinationPosition) {
        
        Node firstNode = firstPosition;
        Node destinationNode = destinationPosition;

        if (firstNode == null) {
            return null;
        }

        if (destinationNode == null || !destinationNode.Walkable) {
            return null;
        }

        if (destinationNode.IsConnectedNodesClosed) {
            return null;
        }

        List<Node> openNodes = new List<Node>();
        List<Node> closedNodes = new List<Node>();

        Node currentNode = firstNode;
        Node testNode;

        int l;
        int i;

        List<Node> connectedNodes;
        float travelCost = 1.0f;

        float g;
        float h;
        float f;

        int iteration = 0;

        while (currentNode != destinationNode) {

            connectedNodes = currentNode.Connected;

            l = connectedNodes.Count;

            for (i = 0; i < l; i++) {
                
                testNode = connectedNodes[i];

                if (testNode == currentNode || testNode.Walkable == false) {
                    continue;
                }

                g = currentNode.g + travelCost;
                h = EuclidianHeuristic(testNode, destinationNode, travelCost);
                f = g + h;

                if (IsContains(testNode, openNodes) || IsContains(testNode, closedNodes)) {

                    if (testNode.f > f) {
                        testNode.f = f;
                        testNode.g = g;
                        testNode.h = h;

                        testNode.ParentNode = currentNode;
                    }
                }
                else {
                    testNode.f = f;
                    testNode.g = g;
                    testNode.h = h;

                    testNode.ParentNode = currentNode;

                    openNodes.Add(testNode);
                }
            }

            closedNodes.Add(currentNode);

            if (openNodes.Count == 0) {
                return null;
            }

            openNodes.Sort(new CompareF());

            currentNode = openNodes[0];

            openNodes.RemoveAt(0);

            iteration++;
        }

        return BuildPath(destinationNode, firstNode);
    }

    private float EuclidianHeuristic(Node node, Node destinationNode, float cost = 1.0f) {

        float dx = node.X - destinationNode.X;
        float dy = node.Y - destinationNode.Y;

        return Mathf.Sqrt(dx * dx + dy * dy) * cost;
    }

    private bool IsContains(Node node, List<Node> list) {
        return list.Contains(node);
    }

    private List<Node> BuildPath(Node destinationNode, Node startNode) {
        
        List<Node> path = new List<Node>();
        Node node = destinationNode;

        path.Add(node);

        while (node != startNode) {
            node = node.ParentNode;
            path.Insert(0, node);
        }

        return path;
    }

    public class CompareF : IComparer<Node> {

        public int Compare(Node x, Node y) {
        
            Node nx = x;
            Node ny = y;

            if (nx.f > ny.f) {
                return 1;
            }
            if (nx.f < ny.f) {
                return -1;
            }
        
            return 0;
        
        }

    }
}
