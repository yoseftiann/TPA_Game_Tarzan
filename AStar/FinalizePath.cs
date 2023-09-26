using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalizePath : MonoBehaviour
{
    private Grid grid;
    [Header("Spawn Object")]
    public GameObject objectSpawn;
    
    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start() { 
        DrawPath();
    }

    private void DrawPath()
    {
        SouthFinalizePath();
        WestFinalizePath();
        EastFinalizePath();
        NorthFinalizePath();
    }

    private void Debugger()
    {
        if(grid.southPath!=null)
        {
            Debug.Log(grid.southPath.Count);
        }
        else
        {
            Debug.Log("soutpath is null");
        }
        
        if(grid.westPath!=null)
        {
            Debug.Log(grid.westPath.Count);
        }
        else
        {
            Debug.Log("westpath is null");
        }

        if(grid.northPath!=null)
        {
            Debug.Log(grid.northPath.Count);
        }
        else
        {
            Debug.Log("northPath is null");
        }

        if(grid.eastPath!=null)
        {
            Debug.Log(grid.eastPath.Count);
        }
        else
        {
            Debug.Log("eastPath is null");
        }
    }

    void SouthFinalizePath()
    {   
        List<Node> points = new List<Node>(grid.southPath);
        Node currNode = FindCenterEdge(points);
        Node endNode = grid.centerPoint;

        FindPath(currNode, endNode);
    }

    void WestFinalizePath()
    {
        List<Node> points = new List<Node>(grid.westPath);
        Node currNode = FindCenterEdge(points);

        FindPath(currNode, grid.centerPoint);
    }

    void EastFinalizePath()
    {
        List<Node> points = new List<Node>(grid.eastPath);
        Node currNode = FindCenterEdge(points);
        Node endNode = grid.centerPoint;

        FindPath(currNode, endNode);
    }

    void NorthFinalizePath()
    {
        List<Node> points = new List<Node>(grid.northPath);
        Node currNode = FindCenterEdge(points);
        Node endNode = grid.centerPoint;

        FindPath(currNode, endNode);
    }

    Node GetNearestNode(Node startNode, List<Node> points)
    {
        Node currNode = null;
        int minDist = int.MaxValue;
        foreach(Node n in points)
        {
            int dist = GetDistance(startNode, n);
            if(dist < minDist)
            {
                minDist = dist;
                currNode = n;
            }
        }
        return currNode;
    }

    Node FindCenterEdge(List<Node> currentNode)
    {
        Node temp = null;
        foreach(Node n in currentNode)
        {
            if(n.isCenterEdge == true)
            {
                return n;
            }
        }
        return temp;
    }

    void FindPath(Node startNode, Node targetNode)
    {
        if(startNode == null || targetNode == null)
        {
            return;
        }
        targetNode.quadrant = startNode.quadrant;
        bool pathSuccess = false;

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                //kalau udah selesai, kita set isAttack true
                pathSuccess = true;
                break;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour) || neighbour.quadrant != startNode.quadrant || neighbour.quadrant == Quadrant.None || !neighbour.isPath)
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        
        if (pathSuccess)
        {
           retracePath(startNode, targetNode);
        }
    }

    public void retracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode.isFinal = true;
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();

        switch (startNode.quadrant)
        {
            case Quadrant.South:
                grid.southPath = path;
                break;
            case Quadrant.West:
                grid.westPath = path;
                break;
            case Quadrant.East:
                grid.eastPath = path;
                break;
            case Quadrant.North:
                grid.northPath = path;
                break;
        }
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
                directionOld = directionNew;
            }
        }
        return waypoints.ToArray();
    }

    public int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }
}
