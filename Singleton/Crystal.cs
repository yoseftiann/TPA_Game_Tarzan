using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public static Crystal instance {get; private set;}
    private void Awake() {
        if(instance != null && instance != this){
            Destroy(this);
        }else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [Header("Status")]
    public int health = 10;
    public SliderController slider;

    public void TakeDamage()
    {
        health--;
        slider.UpdateSliderValue(health);

        if(health <= 0)
        {
            GameManager.instance.LoadSceneDead();
        }
    }
}
