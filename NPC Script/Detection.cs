using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public Movement movement;
    public float detectionRadius = 5f;
    public LayerMask playerLayer;
    [Header("Canvas Reference")]
    public Canvas npcUI;
    private GameObject dialogueUI;
    private GameObject interactUI;

    [Header("Player Reference")]
    private Movement tarzanMovement;
    private TarzanAnimation tarzanAnimation;
    private Jump tarzanJump;
    private Grappling tarzanGrappling;

    private Collider[] hits;

    private void Start() {
        dialogueUI = npcUI.transform.Find("Dialogue").gameObject;
        interactUI = npcUI.transform.Find("Interact").gameObject;
    }

    private void Update() {
        Detect();
        if(movement.isTalking)
        {
            Interact();
        }
        else
        {
            ResetInteract();
        }
    }

    private void ResetInteract()
    {
        tarzanMovement.enabled = true;
        tarzanAnimation.enabled = true;
        tarzanJump.enabled = true;
        tarzanGrappling.enabled = true;
        dialogueUI.gameObject.SetActive(false);
    }

    private void Detect()
    {
        hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if(hits.Length > 0)
        {
            if(movement.isTalking)
            {
                if (hits[0].gameObject.CompareTag("Player"))
                {
                    tarzanMovement = hits[0].gameObject.GetComponent<Movement>();
                    tarzanAnimation = hits[0].gameObject.GetComponent<TarzanAnimation>();
                    tarzanJump = hits[0].gameObject.GetComponent<Jump>();
                    tarzanGrappling = hits[0].gameObject.GetComponent<Grappling>();

                    if (tarzanMovement != null)
                        tarzanMovement.enabled = false;

                    if (tarzanAnimation != null)
                        tarzanAnimation.enabled = false;

                    if (tarzanJump != null)
                        tarzanJump.enabled = false;

                    if (tarzanGrappling != null)
                        tarzanGrappling.enabled = false;
                }
            }
            npcUI.gameObject.SetActive(true);
        }
        else //isTalking == false
        {
            npcUI.gameObject.SetActive(false);
            interactUI.gameObject.SetActive(true);
        }
    }

    private void Interact()
    {
        CursorLockManager.instance.UnLockCursor();
        interactUI.gameObject.SetActive(false);
        dialogueUI.gameObject.SetActive(true);
    }
}
