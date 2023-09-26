using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quadrant
{
    None,
    West,
    South,
    East,
    North
}

public class Node : IHeapItem<Node>
{
    public List<int> possiblePrefabIndex = new List<int> {
        0,1,2,3,4,5
        //0-3 = empty
        //4 = kayu
        //5 = dry tree
        //6 = path
    };
    int heapIndex;
    public bool walkable;
    public Vector3 worldPosition;
    public Node parent;
    public Quadrant quadrant;
    public bool isCenterEdge;
    public bool isPath;
    public bool isFinal;
    public bool isVisited;

    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
        isCenterEdge = false;
        isFinal = false;
        isPath = false; 
        isVisited = false;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
