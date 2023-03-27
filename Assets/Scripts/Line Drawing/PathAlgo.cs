using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathAlgo : MonoBehaviour

{
    /* We create our private class variables that are used in PathAlgo. This includes the Vector3Int (vector datatype that holds x,y,z information, but only in integer format)
    variables that hold our start and end (or goal) positions. */

    /*Create an object for the lineDraw class, this is what will draw our lines for us.*/

    [SerializeField]
    private lineDraw line;
   
    private Vector3Int startPos;
    private Vector3Int endPos;
    /*
        Four boolean statements are used to segment the code's flow, for example, making sure specific code is ran when the first intended end position is reached and the code
        recurses*/
    private bool startFlag = false;

    private bool check = false;

    /*Create gameobjects that will hold the package and lucky driver objects, used to dynamically access their locations on the map.*/

    [SerializeField]
    public GameObject followPoint;

    [SerializeField]
    public GameObject startPoint;

    /* Four boolean statements are used to segment the code's flow, for example, making sure specific code is ran when the first intended end position is reached and the code
    recurses. As we are implementing Breadth First Search as our chosen pathfinding algorithm, we create a HashSet that holds the visited Nodes, and a Queue of Nodes to be visited. */
    private HashSet<Node> visited;
    private Node current;
    private Queue<Node> bfsQueue;

    /* The lists that will contain nodes for our path to our goal and the neighbors to be found in the findNeighbours function.*/
    private List<Node> neighbors;

    [SerializeField]
    public List<Node> path;

    /* A dictionary to hold all created nodes, used to delete everything in the end, or even check if nodes are created already in a certain position on the tilemap.*/
    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    /* Serialize the tilemap variable, making it available to be used in the unity inspector, therefore selectable to be used in our game.*/

    [SerializeField]
    public Tilemap tilemap;

    public bool ifPressed = false;
    private void Update()
    {
      
        if(Input.GetKeyDown("g")){ ifPressed = !ifPressed;}

        if(ifPressed){        
        /* If statements used to determine what the pathfinding is looking for in terms of a goal position, whether it be to the packages on the map,
         * or the customers (destination for package). */
        if(GameObject.FindGameObjectWithTag("Gem"))
        {
            followPoint = GameObject.FindGameObjectWithTag("Gem");

        }
       // else
       // {
       //     followPoint = GameObject.FindGameObjectWithTag("Customer");
       // }

        startPoint = GameObject.FindGameObjectWithTag("Player");
        /* Call our BFS function here in the update() class taken from the monobehaviour superclass, it is called throughout the game as a result. We only call it if the end position, in this case the package object, exists.*/
        if (followPoint) {

            BFS();
        
        
        
        
        }
        else
        { /*if not, we make sure to clear any remnants from the line drawing.*/
            line.clearLine();
        }
        }
        else if (!ifPressed){
            line.clearLine();
        }
    }

    private void BFS()
    {
        /* As update() calls BFS numerous times throughout the runtime of the game, we need to ensure our "state" is ongoing and unchanged till we want it to be.
         The startFlag boolean is used to indicate if the function has been started and undergone the initialization function startAlgo(), if it has, then there is no need to do so again yet. */
        if (!startFlag)
        {
            startAlgo();
        }


        /* We start off by creating a node at the start position of the sprite/gameobject.*/
        current = GetNode(startPos);

        /* Initialise our visited and bfsqueue data structures to be used in the bfs algorithm, and as such, add the start position to both. */
        visited = new HashSet<Node>();
        visited.Add(current);

        bfsQueue = new Queue<Node>();
        bfsQueue.Enqueue(current);

        /* While there is something in the queue, this loop will recurse, eventually breaking out when the end position has been reached and a node for it created. */
        while (bfsQueue.Count > 0)
        {
            /* We dequeue the first element from the queue.*/


            Node temp = bfsQueue.Dequeue();

            /* We first check if the node is the end node.*/
                
            if (temp == null)
            {
                reset();
                return;
            }
                

                if (temp.Position == endPos)
                {
                    /* If so we break out the loop and end the algorithm. */
                    break;
                }
            
          
            /* We find the list of neighbors or associated positions to the current node or tilemap position.*/
            neighbors = FindNeighbors(temp.Position);

            /* For each neighbor to the current node, if it hasnt been visited already, we add it to the visited list and put it in the queue. We make sure to add a connection between that neighbor and the current node by
               changing its value for .Parent (this holds the current nodes position in its object for a sort of breadcrumb path).*/
            foreach (Node x in neighbors)
            {
                if (!visited.Contains(x))
                {
                    visited.Add(x);
                    bfsQueue.Enqueue(x);
                    x.Parent = temp;
                }
            }
        }

        /* Like startFlag before it, check makes sure we only get the path once per algorithm run. We make sure to create a node for the end position finally.*/
        if (!check)
        {
            path = backTrack(GetNode(endPos));

            check = true;
        }

        /*Use the lineDraw object line, and its own setter method to set the path to be drawn by it. Once its points are updated it will now draw the line in real time.*/
        line.setLine(path);

        /*Making sure to reset all variables each time, so it can update the line in real time.*/
        reset();
    }

    /* Simple start algorithm used to initialise my start and end positions, compress the bounds of the tilemap to tiles that actually exist. The start position is converted from a world position, to a position in the tilemap. The same is done with
     the end position.*/

    private void startAlgo()
    {

        startPos = tilemap.WorldToCell(Vector3Int.FloorToInt(startPoint.transform.position));

        endPos = Vector3Int.FloorToInt(followPoint.transform.position);

        endPos = tilemap.WorldToCell(endPos);

        tilemap.CompressBounds();

        startFlag = true;
    }

    /* Function used to, when given a position, create a node if it hasnt already been created AND the position of the node exists in the tilemap.*/

    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else
        {
            if (tilemap.GetTile(position))
            {
                Node node = new Node(position);
                allNodes.Add(position, node);

                return node;
            }
            else
            {
                return null;
            }
        }
    }

    /* FindNeighbors function is used to find the associated tiles from the current position given in, it returns a list of neighbors of the current tile (with nodes created for them). It uses
     a nested loop structure.*/

    private List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();

        /* loop starts to the left of the current position. */
        for (int x = -1; x <= 1; x++)
        {
            /* nested loop starts in the y-axis, below the current position*/
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);

                /* if it is a neighbor*/
                if (y != 0 || x != 0)
                {
                    /* and the neighbor exists in the tilemap and is not the start position */
                    if (neighborPos != startPos && tilemap.GetTile(neighborPos))
                    {
                        /* we create a neighbor node and add it to the list of neighbors*/

                        Node neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                }
            }
        }
        return neighbors;
    }

    /* Uses the breadcrumb trail in each node to trace back a path between the start and end positions. Uses the reverse() function to flip the list to get the start position as the first one to be traversed. */

    private List<Node> backTrack(Node end)
    {
        Node current = end;

        List<Node> path = new List<Node>();

        while (current != null)
        {
            path.Add(current);

            current = current.Parent;
        }

        path.Reverse();

        return path;
    }

    /* Simple function to reset the various lists/variables/datatypes we use and essentially reset the whole algorithm. */

    private void reset()
    {
        current = null;
        path = null;
        check = false;

        startFlag = false;
        bfsQueue.Clear();
        visited.Clear();

        allNodes.Clear();
    }
}