using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceAlignment : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public LayerMask groundLayer;
    public float alignmentSpeed;

    private void Start() {
        //agent rotation to false
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
    }

    private void FixedUpdate() {
        SF();
    }
    
    private void SF()
    {
        if (agent == null || agent.pathPending) return;

        RaycastHit hit;
        float raycastDistance = 2.0f;
        Quaternion currentRotation = transform.rotation;

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            Quaternion toRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            currentRotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * alignmentSpeed);
        }

        if (agent.velocity.sqrMagnitude > Mathf.Epsilon) // Checking if the agent is moving
        {
            Quaternion lookRotation = Quaternion.LookRotation(agent.velocity.normalized);
            currentRotation = Quaternion.Slerp(transform.rotation, lookRotation, agent.angularSpeed * Time.deltaTime);
        }

        transform.rotation = currentRotation;
    }
}
