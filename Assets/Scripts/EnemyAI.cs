using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemyAI : MonoBehaviour
{

    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    private Path path;
    [SerializeField] private float activationDistance = 10f;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public bool activated;
    [SerializeField] GameObject lights;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        Restart();
    }

    public void Restart()
    {
        activated = false;
        lights.gameObject.SetActive(false);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete); // Create the path to the target
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void FixedUpdate() // Used whenever you want to work with Physics
    {
        if (!activated) // if not activated, don't do any of this movement
        {
            CheckCloseToPlayer();
            return;
        }
        
        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized; // Set direction. Explanation: https://www.youtube.com/watch?v=jvtFUfJ6CP8&t=17m

        Vector2 force = direction * speed * Time.deltaTime; // Set the movement speed

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]); // Distance between current position and next waypoint
        if (distance < nextWaypointDistance) // If already reached the next waypoint
        {
            currentWayPoint += 1; // Set to the next waypoint
        }
        else
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.MoveRotation(angle);
        }
    }

    private void CheckCloseToPlayer()
    {
        if (activated)
            return;

        if (Vector3.Distance(transform.position, target.position) < activationDistance) // check if distance between this unit and player is close enough
        {
            ActivateEnemy();
        }
    }

    private void ActivateEnemy()
    {
        activated = true;
        lights.gameObject.SetActive(true);
    }
}
