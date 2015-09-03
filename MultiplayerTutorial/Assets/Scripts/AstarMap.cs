using System.Collections.Generic;
using UnityEngine;

public class AstarMap {

    private Dictionary<string, Node> _nodes;

    private int _mapWidth;
    private int _mapHeight;

    public AstarMap(int mapWidth, int mapHeight) {

        _mapWidth = mapWidth;
        _mapHeight = mapHeight;

        _nodes = new Dictionary<string, Node>();

        Node n;

        for (int i = 0; i < _mapHeight; i++) {

            for (int k = 0; k < _mapWidth; k++) {
                n = new Node(new Vector2(i, k));

                _nodes.Add(n.X + "_" + n.Y, n);
            }

        }

        for (int i = 0; i < _mapHeight; i++) {

            for (int k = 0; k < _mapWidth; k++) {
                n = GetNode(i, k);

                FindConnectedNodes(n);
            }
        }

        foreach (var node in _nodes) {
            if (node.Value.IsConnectedNodesClosed) {
                node.Value.Walkable = false;
            }
        }

    }

    public int MapSize {
        get {
            return _mapHeight * _mapWidth;
        }
    }

    public int mapWidth {
        get {
            return _mapWidth;
        }
    }

    public int mapHeight {
        get {
            return _mapHeight;
        }
    }

    private void FindConnectedNodes(Node n) {

        List<Node> connectedNodes = new List<Node>();
        /*
        Node cn;

        for (float i = n.X - 1; i < n.X + 2; i++) {

            for (float k = n.Y - 1; k < n.Y + 2; k++) {

                cn = GetNode(new Vector2(i, k));

                if (cn != null) {
                    connectedNodes.Add(cn);
                }
            }

        }
        */

        Node cn;

        if (n.X > 0) {
            cn = GetNode(new Vector2(n.X-1, n.Y));
            connectedNodes.Add(cn);
        }

        if (n.X < _mapWidth-1) {
            cn = GetNode(new Vector2(n.X +1, n.Y));
            connectedNodes.Add(cn);
        }

        if (n.Y > 0) {
            cn = GetNode(new Vector2(n.X, n.Y-1));
            connectedNodes.Add(cn);
        }

        if (n.Y < _mapHeight-1) {
            cn = GetNode(new Vector2(n.X, n.Y+1));
            connectedNodes.Add(cn);
        }

        n.Connected = connectedNodes;
    }

    public Node GetNode(Vector2 position) {

        if (_nodes.ContainsKey(position.x + "_" + position.y)) {
            return _nodes[position.x + "_" + position.y];
        }

        return null;
    }

    public Node GetNode(int x, int y) {
        return GetNode(new Vector2(x, y));
    }

    public Node GetNode(float x, float y) {
        return GetNode(new Vector2(x, y));
    }

    public void FillNodes(bool walkable) {

        foreach (var node in _nodes) {
            RegisterWalkability(node.Value.Position, walkable);
        }

    }

    public bool GetNodeWalkability(Vector2 grid) {
        Node n = GetNode(grid);
        if (n != null) {
            return n.Walkable;
        } 
        return false;
        
    }

    public void RegisterWalkability(Vector2 grid, bool walkability) {

        Node n = GetNode(grid);

        if (n != null) {
            n.Walkable = walkability;

        }

    }

}
