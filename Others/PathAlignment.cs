using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAlignment : MonoBehaviour
{
    private GameObject go;
    private void Start() {
        go = gameObject;
        AlignRotate();
    }

    private void AlignRotate()
    {
        float terrainHeight = Terrain.activeTerrain.SampleHeight(go.transform.position) + 145;
        go.transform.position = new Vector3(go.transform.position.x, terrainHeight, go.transform.position.z);

        float randomRotationY = Random.Range(0f, 360f);
        go.transform.position = new Vector3(go.transform.position.x, terrainHeight, go.transform.position.z);
        go.transform.rotation = Quaternion.Euler( go.transform.rotation.eulerAngles.x, randomRotationY,  go.transform.rotation.eulerAngles.z);
    }
}
