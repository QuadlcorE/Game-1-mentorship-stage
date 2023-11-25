using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class EnemyAI : MonoBehaviour
{
    // Field of view variables 
    public float viewRadius;
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();


    // Pathfinding Variables
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    // ============== Field of view ==================
    void checkForPlayerVisibility()
    {
        foreach (Transform targeted in visibleTargets)
        {
            Vector2 toTarget = (targeted.position - gameObject.transform.position).normalized;
            float angle = Vector2.Angle(gameObject.transform.up, toTarget);

            if ((Vector2.Distance(transform.position, target.position) < viewRadius) && (angle < viewAngle / 2))
            {
                Debug.Log(target.gameObject.name + " is within the radius");
                Debug.Log(target.gameObject.name + "is within the viewing angle");
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the view radius
        Handles.color = Color.white;
        Handles.DrawWireArc(gameObject.transform.position, Vector3.forward, Vector3.right, 360, viewRadius);

        Vector3 viewAngleA = new Vector3(Mathf.Sin(-viewAngle / 2 * Mathf.Deg2Rad), Mathf.Cos(-viewAngle / 2 * Mathf.Deg2Rad), 0);
        Vector3 viewAngleB = new Vector3(Mathf.Sin(viewAngle / 2 * Mathf.Deg2Rad), Mathf.Cos(viewAngle / 2 * Mathf.Deg2Rad), 0);

        Handles.DrawLine(gameObject.transform.position, gameObject.transform.position + viewAngleA * viewRadius);
        Handles.DrawLine(gameObject.transform.position, gameObject.transform.position + viewAngleB * viewRadius); 
    }


    // ============== Pathfinding ====================
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void TrackPath()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
        //rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // TrackPath();
    }
}
