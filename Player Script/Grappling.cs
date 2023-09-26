using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private Movement movementScript;
    public Transform cam;
    public Transform gunTip;
    public LayerMask WhatIsGrappelable;
    public LineRenderer lr;
    private Rigidbody rb;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    private Vector3 grapplePoint;
    public float overshootYAxis;
    public float forceAmount = 5f;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;
    private bool isGrappling;

    [Header("Rope")]
    private Spring spring;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;
    private Vector3 currentGrapplePosition;
    private Movement.MovementState previousMovementState;

    private void Start() {
        movementScript = GetComponent<Movement>();
        lr.positionCount = 0;
        spring = new Spring();
        spring.SetTarget(0);
        rb = GetComponent<Rigidbody>();
    }


    private void Update() {
        if(Input.GetKeyDown(grappleKey)) StartGrapple();

        if(grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate() {
        if(isGrappling)
        {
            lr.SetPosition(0, gunTip.position);
        }
        if (isGrappling) 
        {
            DrawRope();
        }
    }

    private void StartGrapple(){
        if(grapplingCdTimer > 0) return;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, WhatIsGrappelable)){
            isGrappling = true;
            movementScript.freeze = true;

            previousMovementState = movementScript.state;
            movementScript.state = Movement.MovementState.grappling;

            grapplePoint = hit.point;
            currentGrapplePosition = gunTip.position;
            lr.enabled = true;
            lr.SetPosition(1, grapplePoint);
            
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple(){
        movementScript.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y -1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if(grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;
       
        Vector3 directionToGrapple = grapplePoint - transform.position;
        movementScript.BoostSpeed(directionToGrapple, forceAmount);

        movementScript.JumpToPosition(grapplePoint, highestPointOnArc);

        
        
        Invoke(nameof(StopGrapple), 1f);
    }

    void DrawRope() 
    {
        if (!isGrappling) 
        {
            currentGrapplePosition = gunTip.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0) 
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 20f);

        var up = Quaternion.LookRotation((grapplePoint - gunTip.position).normalized) * Vector3.up;

        for (var i = 0; i < quality + 1; i++) 
        {
            var delta = i / (float) quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                         affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(gunTip.position, currentGrapplePosition, delta) + offset);
        }
    }

    private void StopGrapple(){
        movementScript.state = previousMovementState;
        isGrappling = false;
        movementScript.freeze = false;
        lr.positionCount = 0;
        spring.Reset();

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }
}
