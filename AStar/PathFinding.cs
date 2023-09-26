using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour
{
    public Grid grid;
    
    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start() {
        SouthFindPath();
        WestFindPath();
        EastFindPath();
        NorthFindPath();
    }

    void SouthFindPath()
    {   
        List<Node> points = new List<Node>(grid.southWaypoint);
        Node currNode = FindCenterEdge(points);
        Node nextNode;

        while(points.Count > 0)
        {
            nextNode  =  GetNearestNode(currNode, points);
            points.Remove(currNode);
            if(nextNode != null)
            {
                FindPath(currNode, nextNode);
                currNode = nextNode;
            }
        }
        FindPath(currNode, grid.centerPoint);
    }   

    void WestFindPath()
    {
        List<Node> points = new List<Node>(grid.westWaypoint);
        Node currNode = FindCenterEdge(points);
        Node nextNode;

        while(points.Count > 0)
        {
            nextNode  = GetNearestNode(currNode, points);
            points.Remove(currNode);
            if(nextNode != null) //masih ada
            {
                FindPath(currNode, nextNode);
                currNode = nextNode;
            }
            else //kalau udah ke target
            {
                FindPath(currNode, grid.centerPoint);
            }
        }
    }

    void EastFindPath()
    {
        List<Node> points = new List<Node>(grid.eastWaypoint);
        Node currNode = FindCenterEdge(points);
        Node nextNode;

        while(points.Count > 0)
        {
            nextNode  = GetNearestNode(currNode, points);
            points.Remove(currNode);
            if(nextNode != null) //masih ada
            {
                FindPath(currNode, nextNode);
                currNode = nextNode;
            }
        }
        FindPath(currNode, grid.centerPoint);
    }

    void NorthFindPath()
    {
        List<Node> points = new List<Node>(grid.northWaypoint);
        Node currNode = FindCenterEdge(points);
        Node nextNode;

        while(points.Count > 0)
        {
            nextNode  = GetNearestNode(currNode, points);
            points.Remove(currNode);
            if(nextNode != null) //masih ada
            {
                FindPath(currNode, nextNode);
                currNode = nextNode;
            }
        }
        FindPath(currNode, grid.centerPoint);
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
                if (!neighbour.walkable || closedSet.Contains(neighbour) || neighbour.quadrant != startNode.quadrant || neighbour.quadrant == Quadrant.None)
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
            currentNode.isPath = true;
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        AssignPathsToQuadrants(path);
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

    void AssignPathsToQuadrants(List<Node> path)
{
    foreach (Node node in path)
    {
        
        switch (node.quadrant)
        {
            case Quadrant.East:
                grid.eastPath.Add(node);
                break;
            case Quadrant.West:
                grid.westPath.Add(node);
                break;
            case Quadrant.South:
                grid.southPath.Add(node);
                break;
            case Quadrant.North:
                grid.northPath.Add(node);
                break;
        }
    }
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
