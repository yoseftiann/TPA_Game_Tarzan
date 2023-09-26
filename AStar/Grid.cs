using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grids;
    float nodeDiameter;
    public int gridSizeX;
    public int gridSizeY;
    public Terrain terrain;
    public bool displayGridGizmos;

    [Header("Waypoints")]
    public List<Node> eastWaypoint;
    public List<Node> westWaypoint;
    public List<Node> northWaypoint;
    public List<Node> southWaypoint;

    public List<Node> eastPath;
    public List<Node> westPath;
    public List<Node> northPath;
    public List<Node> southPath;

    public Node centerPoint;
    public Node startingNode;
    public Node startSouth;
    public Node startWest;
    public Node startNorth;
    public Node startEast;


    void Awake()
    {
        Instantiate();
        createGrid();
        GetRandomPoint();
        SetCenterPoint();
        SetStartingNode();
    }

    private void Instantiate()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        eastWaypoint = new List<Node>();
        northWaypoint = new List<Node>();
        westWaypoint = new List<Node>();
        southWaypoint = new List<Node>();

        eastPath = new List<Node>();
        northPath = new List<Node>();
        westPath = new List<Node>();
        southPath = new List<Node>();
    }

    private void SetCenterPoint()
    {
        int maxy = gridSizeY-1;
        int maxx = gridSizeX-1;
        int midx = gridSizeX / 2;
        int midy = gridSizeY / 2;

        centerPoint = grids[gridSizeX/2, gridSizeY/2];

        eastWaypoint.Add(grids[maxx, midy]);
        grids[maxx, midy].isCenterEdge = true;
        startEast = grids[maxx, midy];
        eastWaypoint.Add(grids[gridSizeX/2, gridSizeY/2]);

        northWaypoint.Add(grids[midx, maxy]);
        grids[midx, maxy].isCenterEdge = true;
        startNorth = grids[midx, maxy];
        northWaypoint.Add(grids[gridSizeX/2, gridSizeY/2]);

        westWaypoint.Add(grids[0, midy]);
        grids[0, midy].isCenterEdge = true;
        startWest = grids[0, midy];
        westWaypoint.Add(grids[gridSizeX/2, gridSizeY/2]);

        southWaypoint.Add(grids[midx, 0]);
        grids[midx, 0].isCenterEdge = true;
        startSouth = grids[midx, 0];
        southWaypoint.Add(grids[gridSizeX/2, gridSizeY/2]);
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    private void createGrid()
    {
        grids = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y/2;
        float terrainYOffset = Terrain.activeTerrain.transform.position.y;
        for (int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 gridPosition = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                Vector3 worldPosition = new Vector3(gridPosition.x, terrainYOffset, gridPosition.z);
                float terrainHeight = Terrain.activeTerrain.SampleHeight(worldPosition);

                worldPosition.y += terrainHeight;
                bool walkable = !(Physics.CheckSphere(worldPosition, nodeRadius, unwalkableMask));
                grids[x, y] = new Node(walkable, worldPosition, x,y);

                //set quadrant
                if( x < y && x + y > gridSizeX)
                {
                    grids[x,y].quadrant = Quadrant.North;
                }
                else if( x > y && x + y > gridSizeX)
                {
                    grids[x,y].quadrant = Quadrant.East;
                }
                else if( x > y && x + y < gridSizeX)
                {
                    grids[x,y].quadrant = Quadrant.South;
                }
                else if( x < y && x + y < gridSizeX)
                {
                    grids[x,y].quadrant = Quadrant.West;
                }
                else
                {
                    grids[x,y].quadrant = Quadrant.None;
                }
            }
        }
    }

    private void GetRandomPoint()
    {
        int randomX;
        int randomY;
        int totalPointFound = 0;
        do
        {
            randomX = Random.Range(0, gridSizeX);
            randomY = Random.Range(0, gridSizeY);

            if(grids[randomX, randomY].quadrant == Quadrant.None)
            {
                continue;
            }
            else if(grids[randomX, randomY].quadrant == Quadrant.East && CheckAvailability(eastWaypoint))
            {
                eastWaypoint.Add(grids[randomX, randomY]);
                totalPointFound++;
            }
            else if(grids[randomX, randomY].quadrant == Quadrant.West && CheckAvailability(westWaypoint))
            {
                westWaypoint.Add(grids[randomX, randomY]);
                totalPointFound++;
            }
            else if(grids[randomX, randomY].quadrant == Quadrant.North && CheckAvailability(northWaypoint))
            {
                northWaypoint.Add(grids[randomX, randomY]);
                totalPointFound++;
            }
            else if(grids[randomX, randomY].quadrant == Quadrant.South && CheckAvailability(southWaypoint))
            {
                southWaypoint.Add(grids[randomX, randomY]);
                totalPointFound++;
            }
        }
        while(totalPointFound != 16);
    }

    private bool CheckAvailability(List<Node> currentQuadrant)
    {
        return currentQuadrant.Count < 4 ? true : false;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - transform.position;
        float percentX = Mathf.Clamp01(localPosition.x / gridWorldSize.x + 0.5f);
        float percentY = Mathf.Clamp01(localPosition.z / gridWorldSize.y + 0.5f);

        int x = Mathf.FloorToInt(Mathf.Clamp01(percentX) * (gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp01(percentY) * (gridSizeY - 1));
        return grids[x, y];
    }

    private void OnDrawGizmos()
    {
        Node previousPathNode = null;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        
        if (grids != null && displayGridGizmos)
        {
            //Node playerNode = NodeFromWorldPoint(player.position);
            foreach(Node n in grids)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.black;
                if(n.quadrant == Quadrant.North)
                {
                    Gizmos.color = Color.white;
                    if(n.isFinal)
                    {
                        Gizmos.color = Color.blue;
                    }
                }
                if(n.quadrant == Quadrant.South )
                {
                    Gizmos.color = Color.white;
                    if(n.isFinal)
                    {
                        Gizmos.color = Color.yellow;
                    }
                }
                if(n.quadrant == Quadrant.West )
                {
                    Gizmos.color = Color.white;
                    if(n.isFinal)
                    {
                        Gizmos.color = Color.red;
                    }
                }
                if(n.quadrant == Quadrant.East)
                {
                    Gizmos.color = Color.white;
                    if(n.isFinal)
                    {
                        Gizmos.color = Color.green;
                    }
                }
                if(n.quadrant == Quadrant.None)
                {
                    Gizmos.color = Color.black;
                }

                if (eastWaypoint.Contains(n) || westWaypoint.Contains(n) || northWaypoint.Contains(n) || southWaypoint.Contains(n))
                {
                    Gizmos.color = Color.magenta;
                }

                if(!n.walkable)
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x=-1; x<=1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x==0 && y == 0)
                {
                    continue;
                }
                else 
                {
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y; 

                    if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        if(grids[checkX, checkY].isVisited) continue;
                        neighbours.Add(grids[checkX, checkY]);
                    }
                }
            }
        }

        return neighbours;
    }

    private void SetStartingNode()
    {
        int randomX, randomY;
        do
        {
            randomX = Random.Range(0, gridSizeX);
            randomY = Random.Range(0, gridSizeY);
        }
        while (!grids[randomX, randomY].walkable);

        startingNode = grids[randomX, randomY];
    }
}
