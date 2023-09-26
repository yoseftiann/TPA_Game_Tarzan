using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEnemiesController : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public int currvalue;
    public int maxvalue;

    public void UpdateText(int curr)
    {
        textMesh.text = curr.ToString() + " / " + maxvalue.ToString();
    }

    public void SetMax(int max)
    {
        maxvalue = max;
    }
}
