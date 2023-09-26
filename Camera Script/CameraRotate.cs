using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float rotationSpeed;
    public float smoothness;
    
    private float currentRotationSpeed = 0.0f;

    private void Update() {
        RotateWithLerp();
    }

    private void RotateWithLerp()
    {
        currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, rotationSpeed, Time.deltaTime * smoothness);
        transform.Rotate(Vector3.up * currentRotationSpeed * Time.deltaTime);
    }
}
