using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarzanAnimation : MonoBehaviour
{
    Animator animator;
    float velocity;
    int velocityHash;
    public float accel = 0.1f;
    public float decel = 0.5f;
    public float lerpSpeed;

    [Header("Punch Variables")]
    private float hitCd = 0.5f;
    int lastClicked = 0;
    public float maxComboDelay = 5f;

    private void Start() {
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
    }

    private void Update() {
        Animation();
        if(Input.GetMouseButtonDown(0))
        {   
            ComboHit();
        }
    }

    private void Animation()
    {
        bool walking = isWalking();
        bool running = isRunning();
        bool crouch = isCrouch();
        bool idle = isIdle();
        float target = 0f;

        animator.SetBool("isCrouch", isCrouch());

        if(walking)
        {
            target  = 0.1f;
        }
        
        if(running)
        {
            target = 1f;
        }

        if(idle)
        {
            target = 0.0f;
        }
        else if(!walking && velocity > 0)
        {
            target = 0.0f;
        }

        velocity = Mathf.Lerp(velocity, target, lerpSpeed);
        float threshold = 0.01f; // You can adjust this value
        if (Mathf.Abs(velocity - target) < threshold)
        {
            velocity = target;
        }
        animator.SetFloat(velocityHash, velocity);
    }

    private bool isCrouch()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isIdle()
    {
        if(!isWalking() && !isRunning())
        {
            return true;
        }
        else return false;
    }

    private bool isWalking()
    {
        if (Input.GetKey("d") || Input.GetKey("s") || Input.GetKey("w") || Input.GetKey("a"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isRunning()
    {
        if(Input.GetKey(KeyCode.LeftShift) && isWalking())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ComboHit()
    {   
        if(Time.time < hitCd) return;

        hitCd = Time.time + 0.5f;

        if(lastClicked == 0)
        {
            //do combo1
            animator.SetBool("Hit1", true);
            lastClicked = 1;
            Invoke("ResetCombo", maxComboDelay);
        }
        else if(lastClicked == 1)
        {
            //do combo2
            animator.SetBool("Hit2", true);
            lastClicked = 2;
            Invoke("ResetCombo", maxComboDelay);
        }
    }

    private void ResetCombo()
    {
        animator.SetBool("Hit1", false);
        animator.SetBool("Hit2", false);
        lastClicked = 0;
    }
}
