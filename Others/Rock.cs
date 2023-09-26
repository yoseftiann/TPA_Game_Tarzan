using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float damage;
    void Update()
    {
        Rotate();   
    }

    private void Start() {
        Invoke("DestroySelf", 2f);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    private void Rotate()
    {
        transform.Rotate(0, 360 * Time.deltaTime, 360 * Time.deltaTime);
    }
}
