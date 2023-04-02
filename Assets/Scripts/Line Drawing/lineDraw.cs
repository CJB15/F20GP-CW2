using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class lineDraw : MonoBehaviour
{
    /*Create the class variables, one that holds as list of nodes (will hold our points to be drawn at), the tilemap of the project, and a LineRenderer object which shall draw our pathfinding lines.*/
    private LineRenderer lr;
    private List<Node> points;
    public Tilemap tilemap;

    /*Once the lineDraw object is created, it will get the lineRenderer and tilemap in our game (in the unity editor).*/

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        tilemap = GetComponent<PathAlgo>().tilemap;
    }

    /*Set the maximum positions that the line will be at, from the points given in to the function.*/

    public void setLine(List<Node> points)
    {
        if (points != null)
        {
            lr.positionCount = points.Count;
            this.points = points;
        }
    }

    /*Once the line is unneeded anymore (like when the package is destroyed), we will clear these points and make sure the line is removed.*/

    public void clearLine()
    {
        points = null;
        lr.positionCount = 0;
    }

    /*We use this Update() structure to check if there are any points to be drawn, if there are, we draw a line through each point to reach the intended goal position.*/

    private void Update()
    {
        if (points != null)
        {
            for (int i = 0; i < points.Count; i++)
            {
                
                lr.SetPosition(i, tilemap.GetCellCenterWorld(points[i].Position));
            }


        }
    }
}