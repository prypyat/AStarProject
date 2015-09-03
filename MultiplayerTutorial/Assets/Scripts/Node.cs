using System.Collections.Generic;
using UnityEngine;

public class Node {

    private Vector2 _position;
    private List<Node> _connected;

    public Node(Vector2 position, Node parentNode = null, bool walkable = true, float g = 10) {

        _position = position;
        this.Walkable = walkable;
        ParentNode = parentNode;
        this.g = g;

    }

    public float f { get; set; }

    public float g { get; set; }

    public float h { get; set; }

    public float X {
        get {
            return _position.x;
        }
    }

    public float Y {
        get {
            return _position.y;
        }
    }

    public Vector2 Position {
        get {
            return _position;
        }

    }

    public Node ParentNode { get; set; }

    public bool Walkable { get; set; }

    public bool IsConnectedNodesClosed {
        get {

            foreach (Node n in _connected) {

                if (n.Walkable) {
                    return false;
                }
            }

            return true;
        }
    }

    public List<Node> Connected {
        get {
            return _connected;
        }
        set {
            _connected = value;
        }
    }
}
