using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [Header("Object To Spawn")]
    public GameObject prefab;
    public Grid grid;
    private Unit unit;
    private Node spawnNode;
    private int currentQuadrantIndex;
    public bool waveFinished;
    private int activeEnemies;
    public int waveLevel;

    [Header("UI Reference")]
    public SliderController slider;
    public TextEnemiesController tec;
    public GameObject waveCanvas;

    private void Start() {
        waveFinished = true;
        grid = GetComponent<Grid>();
        unit = prefab.GetComponent<Unit>();
        waveLevel = 0;
        activeEnemies = waveLevel;
        
        //Start a Wave
        Wave();
    }

    private void Update() {
        if(activeEnemies > 0)
        {
            waveCanvas.SetActive(true);
            waveFinished = false;
        }
        else
        {
            waveCanvas.SetActive(false);
            waveFinished = true;
        }
    }

    public void waveIncrement()
    {
        waveLevel++;
        Wave();
    }

    private void Wave()
    {
        if(activeEnemies == 0)
        {
            waveFinished = false;
            for(int i=0;i<waveLevel;i++)
            {
                Debug.Log("instantiate");
                Invoke("InstantiateAnEnemy",0.5f * i);
            }
        }
        Debug.Log("start a wave level : " + waveLevel);

        // UpdateUI
        slider.setMaxValue(waveLevel);
        slider.UpdateSliderValue(waveLevel);
        tec.SetMax(waveLevel);
        tec.UpdateText(waveLevel);

    }

    public void RemoveActiveEnemies()
    {
        activeEnemies--;
        Debug.Log(activeEnemies);

        //Update UI
        slider.UpdateSliderValue(activeEnemies);
        tec.UpdateText(activeEnemies);
    }

    private void InstantiateAnEnemy()
    {
        //Get the quadrant + SpawnPosition
        currentQuadrantIndex = GetRandomQuadrant();

        //Instantiate
        GameObject objecToSpawn = Instantiate(prefab, spawnNode.worldPosition,Quaternion.identity);
        GetQuadrantPath(objecToSpawn, currentQuadrantIndex);
        activeEnemies++;
    }

    private int GetRandomQuadrant()
    {
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0:
                // unit.path = grid.westPath;
                spawnNode = grid.startWest;
                break;
            case 1:
                // unit.path = grid.southPath;
                spawnNode = grid.startSouth;
                break;
            case 2:
                // unit.path = grid.eastPath;
                spawnNode = grid.startEast;
                break;
            case 3:
                // unit.path = grid.northPath;
                spawnNode = grid.startNorth;
                break;
        }
        return rand;
    }

    private void GetQuadrantPath(GameObject go, int index)
    {
        Unit unit = go.GetComponent<Unit>();

        if(unit==null)
        {
            return;
        }

        switch (index)
        {
            case 0: 
                unit.path = grid.westPath;
                break;
            case 1:
                unit.path = grid.southPath;
                break;
            case 2:
                unit.path = grid.eastPath;
                break;
            case 3:
                unit.path = grid.northPath;
                break;
        }
        unit.StartFollowingPath();
    }
}
