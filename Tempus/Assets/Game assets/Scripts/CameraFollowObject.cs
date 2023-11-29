using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;

    private void FixedUpdate()
    {
        desiredPosition = target.position + offset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    private void LateUpdate()
    {
        transform.position = smoothedPosition;
    }
}
