using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public float health;
    private float maxHealth;
    private bool hasTakenPunch;
    public FloatingHealthBar healthBar;
    private NavigatorAI navigatorAI;
    public bool isWild;
    public bool isDino;
    public bool isDragon;

    private void Awake() {
        maxHealth = health;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        navigatorAI = GetComponent<NavigatorAI>();
        isWild =  true;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Damage taken");
        health -= damage;
        hasTakenPunch = true;
        healthBar.UpdateHealthBar(health, maxHealth);

        if(health <= 0 )
        {
            if(isDino)
            {
                GameObject aStarObject = GameObject.Find("AStar");
                if(aStarObject!=null)
                {
                    Spawner spawn = aStarObject.GetComponent<Spawner>(); 
                    if(spawn!=null)
                    {
                        spawn.RemoveActiveEnemies();
                    }
                }
                Destroy(this.gameObject);
            }
            else if(isDragon)
            {
                //Reset Dragon Health
                GameManager.instance.LoadSceneVictory();
            }
            else
            {
                navigatorAI.currentState = NavigatorAI.States.Pet;
                PetCounter.instance.AddPet();
                isWild = false;
                health = maxHealth;
                healthBar.UpdateHealthBar(health, maxHealth);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Detected collision with: " + other.gameObject.name);
        //Rock
        if(other.tag == "Rock" && isWild)
        {
            Rock rock = other.GetComponent<Rock>();
            if(rock!=null)
            {
                TakeDamage(rock.damage);
            }
        }
        //Player Hand
        else if(other.tag == "Player" & isWild)
        {
            Status status = other.GetComponentInChildren<Status>();
            if(status!=null)
            {
                TakeDamage(status.punchDamage);
            }
        }
        //Bear attack Bear
        else if(other.tag == "Bear")
        {
            Debug.Log("Bear as a pet has attacked");
            if(navigatorAI != null)
            {
                TakeDamage(navigatorAI.damage);
            }
        }
        //Bear attack Player
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player")
        {
            hasTakenPunch = false;
        }
    }
}
