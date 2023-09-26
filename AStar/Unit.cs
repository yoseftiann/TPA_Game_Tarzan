using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Reference")]
    public Grid grid;
    public List<Node> path;
    public float speed;
    public float rotationSpeed;
    private int currentIndex = 0;
    public int quadrantIndex;

    private Animator animator;
    private bool hasArrived;
    public Spawner spawn;

    private void Start() 
    {
        hasArrived = false;
        GameObject astar = GameObject.Find("AStar");
        grid = astar.GetComponent<Grid>();
        spawn = astar.GetComponent<Spawner>();
    }

    public void StartFollowingPath()
    {
        if(path != null)
        {
            StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[currentIndex].worldPosition;

        while (true)
        {
            if (Vector3.Distance(transform.position, currentWaypoint) < 0.1f)
            {
                currentIndex++;

                if (currentIndex >= path.Count - 1)
                {
                    hasArrived = true; 
                    Crystal.instance.TakeDamage();
                    DestroyObject();
                    yield break;
                }

                currentWaypoint = path[currentIndex].worldPosition;
            }

            Vector3 dir = (currentWaypoint - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); 

            transform.position += dir * speed * Time.deltaTime;

            yield return null;
        }
    }

    private void DestroyObject()
    {
        spawn.RemoveActiveEnemies();
        Destroy(this.gameObject);
    }
}