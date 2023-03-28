using UnityEngine;

/*To create a node structure to use, we need to create a class for the Node. It holds a parent position, for a breadcrumb trail path to the goal node, and a Vector3Int position (the nodes current position).*/

public class Node
{
    public Node Parent { get; set; }
    public Vector3Int Position { get; set; }

    public Node(Vector3Int position)
    {
        this.Position = position;
    }
}