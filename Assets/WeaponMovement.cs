using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    public Transform target;
    public float orbitDistance = 1.0f;
    public float orbitDegreesPerSec = 180.0f;
    public bool isRotating = false;
    public float targetAngle;
    public bool rotateLeft = false;
    public void Orbit()
    {
        if (target != null)
        {
            //print(rotateLeft);
            //print(transform.eulerAngles);
            //print(direction);
            // Keep us at orbitDistance from target
            if (Mathf.Abs(transform.eulerAngles.z - Mathf.Abs(targetAngle)) > 1f)
            {
                transform.position = target.position + (transform.position - target.position).normalized * orbitDistance;
                transform.RotateAround(target.position, rotateLeft? Vector3.forward : Vector3.back, orbitDegreesPerSec * Time.deltaTime);
            } else
            {
                isRotating = false;  
                //transform.eulerAngles = new Vector3(0f, 0f, direction);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isRotating)
        {
            Orbit();
        }
    }
}
