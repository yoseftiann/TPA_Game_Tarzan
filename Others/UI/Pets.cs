using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pets : MonoBehaviour
{
    public TMP_Text valueText;

    public void changeUI(int value)
    {
        if (valueText != null)
        {
            valueText.text = value.ToString() + " PETS";
        }
    }
}
