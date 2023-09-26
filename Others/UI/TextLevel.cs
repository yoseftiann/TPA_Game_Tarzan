using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLevel : MonoBehaviour
{
    public Text textMesh;
    public int level;

    private void Start() {
        UpdateText(level);
    }

    public void UpdateText(int currLvl)
    {
        textMesh.text = currLvl.ToString();
    }
}
