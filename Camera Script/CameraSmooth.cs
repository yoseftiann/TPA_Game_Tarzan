using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float xOffset;
    public float yOffset = 0.5f;
    public Movement playerMovement;

    [Header("Blobbing")]
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;
    private float defaultPosY = 0;
    private float timer = 0;


    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset + target.up * yOffset + target.right * xOffset;
    
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
