using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFC : MonoBehaviour
{
    private Grid grid;
    private int lowestIndex;
    private int highestIndex;
    public List<GameObject> prefabs; 

    private void Start() {
        grid = GetComponent<Grid>();
        Node start = grid.startingNode;
        TraverseAllNodes(start);
    }

    private void SpawnObject(Node current)
    {   
        if (current.possiblePrefabIndex.Count == 0)
        {
            // Debug.LogError("Contradiction encountered! No possible prefab to spawn.");
            return;
        }
        int randomPrefabIndex = Random.Range(0, current.possiblePrefabIndex.Count);
        GameObject prefab = prefabs[randomPrefabIndex]; 

        Vector3 positiion = current.worldPosition;
        positiion.y = Terrain.activeTerrain.SampleHeight(positiion) + 145; //offset;
    
        if(current.isFinal || current.isCenterEdge)
        {
            prefab = prefabs[6];
            Instantiate(prefabs[6], positiion, Quaternion.identity);
        }
        else
        {
            positiion.y = Terrain.activeTerrain.SampleHeight(positiion) + 144; //offset;
            Instantiate(prefab, positiion, Quaternion.identity);
        }
        List<Node> neighbors = grid.GetNeighbours(current);
        // Debug.Log("neighbor  - - > " + neighbors.Count);
        foreach (Node neighbor in neighbors)
        {
            // Debug.Log("Updating neighbor");
            UpdateNeigborIndex(neighbor, prefab);
        }
    }

    private void TraverseAllNodes(Node start)
    {
        for(int x=0;x<grid.gridSizeX; x++)
        {
            for(int y=0;y<grid.gridSizeY; y++)
            {
                Node currentNode = grid.grids[x,y];
                grid.grids[x,y].isVisited = true;

                SpawnObject(currentNode);
            }
        }
    }

    private void UpdateNeigborIndex(Node neighbor, GameObject prefab)
    {
        PrefabRestriction pr = prefab.GetComponent<PrefabRestriction>();

        if(pr)
        {
            foreach (int p in pr.restrictions)
            {
                if(neighbor.possiblePrefabIndex.Contains(p))
                {
                    neighbor.possiblePrefabIndex.Remove(p);
                    // Debug.Log("removed "+ p + " from neighborsIndex");
                }
            }
        }
    }
}
