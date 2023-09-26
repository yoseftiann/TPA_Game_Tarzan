using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarController : MonoBehaviour
{  
    public GameObject child;

    void Update()
    {
        // Debug.Log("isLockAWSD statusBarController : " + GameManager.instance.isLockAWSD);
        if(GameManager.instance.isLockAWSD)
        {
            child.SetActive(false);
        }
        else if(!GameManager.instance.isLockAWSD)
        {
            child.SetActive(true);
        }
    }
}
