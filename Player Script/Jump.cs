using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public LayerMask GroundLayer;
    public float jumpHeight;
    [SerializeField] private Vector3 offset;
    private float Radius = 1f;
    private Rigidbody rb;
    public bool grounded;
    public float downwardForce = 1f;
    public Movement movementScript;
    private float timer = 0f;

    //Animation Jump - Fall - Land
    public Animator animator;
    public bool isClicked;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public bool isGrounded(){
        Collider[] colliders = Physics.OverlapSphere(transform.position + offset, Radius, GroundLayer);
        return colliders.Length > 0;
    }

    private void Update() {
        grounded = isGrounded();
        AnimationHandler();
    }

    private void AnimationHandler()
    {
        if(grounded)
        {
            timer = 0f;
            if(isClicked)
            {
                LandingAnimation();
                isClicked = false;
            }
            else
            {
                LandingAnimation();
            }
        }
        else
        {
            timer += Time.deltaTime;
            if(timer > 0.1f)
            {
                FallingAnimation();
            }
        }

        if(Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isClicked = true;
            JumpingAnimation();
        }
    }

    private void JumpingAnimation()
    {
        animator.SetBool("isJumping", true);
        animator.SetFloat("JumpTimer", 0f);
        animator.SetBool("isGrounded", false);
    }

    private void FallingAnimation()
    {
        animator.SetBool("isJumping", false);
        animator.SetFloat("JumpTimer", timer);
    }

    private void LandingAnimation()
    {
        animator.SetBool("isGrounded", true);
        animator.SetFloat("JumpTimer", timer);
    }

    private void Animation()
    {
        if(isClicked)
        {
            //Jump
            animator.SetBool("isJumping", true);
            
            //Falling if Timer > 0.2f

            //Landing if have reach the ground
        }
    }

    private void SetFalse()
    {
        animator.SetBool("isJumping", false);
    }

    private void FixedUpdate() 
    {
        if (!grounded)
        {
            rb.AddForce(Vector3.down * downwardForce, ForceMode.Acceleration);
        }
    }
}
