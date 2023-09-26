using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCounter : MonoBehaviour
{
    public static PetCounter instance;
    public Pets petScript;
    private void Awake() {
        if(instance != null && instance != this){
            Destroy(this);
        }else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private int pets;

    private void Start() {
        pets = 0;
        petScript.changeUI(pets);
    }

    public void AddPet()
    {
        pets = pets + 1;
        petScript.changeUI(pets);
        
    }

    public int GetPet()
    {
        return pets;
    }
}
