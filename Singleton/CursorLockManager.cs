using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLockManager : MonoBehaviour
{
    //Singleton
    public static CursorLockManager instance {get; private set;}

    private void Awake() {
        if(instance != null && instance != this){
            Destroy(this);
        }else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UnLockCursor(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableCursor(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start() {
        DisableCursor();
    }
}
