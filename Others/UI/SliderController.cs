using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    //Slider Enemies
    public Slider slider;
    public float minvalue;
    public float maxvalue;
    public bool isCrystal;
    public bool isEmpty;

    private void Start() {
        setMinValue(minvalue);
        setMaxValue(maxvalue);
        if(isEmpty)
        {
            UpdateSliderValue(minvalue);
        }
        else
        {
            UpdateSliderValue(maxvalue);
        }
    }

    public void UpdateSliderValue(float newValue)
    {
        Debug.Log("update with " + newValue);
        slider.value = newValue;
    }

    public void setMinValue(float newMin)
    {
        minvalue = newMin;
        slider.minValue = minvalue;
    }

    public void setMaxValue(float newMax)
    {
        maxvalue = newMax;
        slider.maxValue = maxvalue;
    }
}
