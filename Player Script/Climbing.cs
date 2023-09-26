using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;
    public Transform orientation;
    public LayerMask WhatIsTree;
    // [SerializeField] private Vector3 offset;

    [Header("Climbing Attribute's")]
    public float speed;
    public float maxClimbTimer;
    private float climbTimer;
    private bool isClimbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;
    private bool jumpClimbInitiated = false;


    private RaycastHit frontWallHit;
    private bool treeFront;
    public Jump JumpScript;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        // isTree();
        TreeCheck();
        StateMachine();

        if(isClimbing) ClimbingMovement();
    }

    private void TreeCheck(){
        treeFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, WhatIsTree);

        // if (treeFront) {
        //     Debug.Log("Tree Detected");
        // } else {
        //     Debug.Log("No Tree Detected");
        // }
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if(JumpScript.grounded)
        {
            climbTimer = maxClimbTimer;
        }
    }

    private void StartClimbing(){
        Debug.Log("start climbing");
        isClimbing = true;
        jumpClimbInitiated = true;

        //change fov
    }

    private void ClimbingMovement(){
        rb.velocity = new Vector3(rb.velocity.x, speed, rb.velocity.z);
    }

    private void StopClimbing(){
        isClimbing = false;

        // particle effect
    }

private void StateMachine(){
    bool isNearTreeAndTryingToClimb = treeFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle;
    bool isJumpClimbing = treeFront && Input.GetKeyDown(KeyCode.Space);

    if (isNearTreeAndTryingToClimb || (isJumpClimbing && !jumpClimbInitiated))
    {

        if (!isClimbing && climbTimer > 0) StartClimbing();

        if (climbTimer > 0) climbTimer -= Time.deltaTime;
        if (climbTimer < 0) StopClimbing();

        if(isJumpClimbing)
        {
            jumpClimbInitiated = true;
            // Add a small vertical boost to represent the "jump" aspect of the climb.
            rb.velocity += new Vector3(0, speed * 1.5f, 0); // The 1.5 multiplier can be adjusted to your needs.
        }
    }
    else
    {
        if(isClimbing) StopClimbing();
        jumpClimbInitiated = false; // Reset the jump climb state once not near a tree or climbing.
    }
}

}
