using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        position.z = -10;
        transform.position = position;
    }
}
