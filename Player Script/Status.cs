using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Player Status")]
    public int level = 1;
    private float maxHealth;
    public float health;
    public float experience;
    public float punchDamage;
    public float cooldown = 1f;
    private float nextDamageTime = 0f;

    [Header("UI Reference")]
    public SliderController sliderController;
    public TextLevel textLvl;

    public void TakeDamage(float damageTaken)
    {
        if (Time.time >= nextDamageTime)
        {
            health -= damageTaken;
            sliderController.UpdateSliderValue(health);
            nextDamageTime = Time.time + cooldown;
            //Update health bar

            if(health <=0 )
            {
                GameManager.instance.LoadSceneDead();
            }
        }
    }

    private void Start() {
        health = maxHealth;
    }

    public void LevelUp()
    {
        level++;
        maxHealth += 25;
        health = maxHealth;
        textLvl.UpdateText(level);
    }

    public void TakeExp(float expTaken)
    {
        experience += expTaken;

        if(experience >= 100)
        {
            LevelUp();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Bear")
        {
            TakeDamage(15);
        }
        if(other.tag == "Dragon")
        {
            TakeDamage(25);
        }
    }
}
