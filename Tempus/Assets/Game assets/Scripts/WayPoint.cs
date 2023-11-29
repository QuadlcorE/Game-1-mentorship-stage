using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public GameObject nextWayPoint;
    public float wayPointGizmosRadius = 0.4f;
    // Start is called before the first frame update
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wayPointGizmosRadius);

        Gizmos.DrawLine(transform.position, nextWayPoint.transform.position);
    }
}
