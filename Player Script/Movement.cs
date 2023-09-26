using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Variables
    private Rigidbody rb;
    float velocity = 0.0f;
    float turnSpeed = 10f;
    float yRotation = 0.0f;
    float xRotation = 0.0f;

    [Header("Basic Movement")]
    //Walking
    public float walkingSpeed, counterMovementSpeed, lookSpeed;
    private float movementSpeed;
    //Sprint
    public float GetSprintValue;
    //Crouching
    public float GetCrouchValue;
    public float crouchYScale;
    private float startYScale;


    [Header("KeyBinds")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Camera FOV")]
    public float sprintFOV = 50f;
    public float normalFOV = 40f;
    public float crouchFOV = 30f;
    public float grapplingFOV = 75f;
    public float fovLerpTime = 0.1f;

    [Header("Other Reference's")]
    public bool freeze;
    public bool activeGrapple;
    private Vector3 velocityToSet;
    public bool isSlope;
    public bool isTalking;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        grappling,
    }

    private void InteractHandler()
    {
        if(!isTalking && Input.GetKeyDown(KeyCode.F))
        {
            //Do the interact
            isTalking = true;
        }
    }

    private void StateHandler()
    {
        if(state == MovementState.grappling)
        {
            changeFov(grapplingFOV);
        }
        else
        {
            //Crouching
            if(Input.GetKey(crouchKey))
            {
                
                state = MovementState.crouching;
                movementSpeed = walkingSpeed * GetCrouchValue;
                changeFov(crouchFOV);
            }
            //Sprint
            else if(Input.GetKey(sprintKey))
            {
                state = MovementState.sprinting;
                movementSpeed = walkingSpeed * GetSprintValue;
                changeFov(sprintFOV);
            }
            //Walking
            else
            {
                state = MovementState.walking;
                movementSpeed = walkingSpeed;
                changeFov(normalFOV);
            }
        }
    }

    public void Move(){
        if(activeGrapple) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 relativeX = x * Camera.main.transform.right;
        Vector3 relativeY = y * transform.forward;

        relativeX.y = 0;
        relativeY.y = 0;

        Vector3 direction = relativeX + relativeY;
    
        Vector3 velocityXZ = new Vector3(-rb.velocity.x, 0f, -rb.velocity.z);

        rb.AddForce(direction * movementSpeed);
        rb.AddForce(velocityXZ * counterMovementSpeed);
    }

    public void Rotation(){
        //Horizontal
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * lookSpeed;
        yRotation += mouseX;

        transform.rotation = Quaternion.Euler(0, yRotation, 0);

        //Vertical
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        normalFOV = Camera.main.fieldOfView;

        startYScale = transform.localScale.y;
    }

    private void Update() {
        StateHandler();
        Rotation();
        InteractHandler();
    }

    private void FixedUpdate() 
    {
        if(!GameManager.instance.isLockAWSD)
        {
            Move();
        }
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    } 

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 1.5f);
    }

    public void BoostSpeed(Vector3 direction, float forceAmount)
    {
        rb.AddForce(direction.normalized * forceAmount, ForceMode.VelocityChange);
    }

    private void SetVelocity()
    {
        rb.velocity = velocityToSet;
        //efek fov
    }

    public void ResetRestrictions()
    {
        //when touch the ground
        activeGrapple = false;
        //balikin fov
    }

    private void changeFov(float targetFOV){
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, fovLerpTime * Time.deltaTime);
    }
}
