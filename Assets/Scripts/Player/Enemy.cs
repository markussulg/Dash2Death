using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public WeaponMovement weapon;
    private Rigidbody2D rb;
    private Vector3 direction;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  
    }

    // Update is called once per frame
    void Update()
    {
        direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
        //direction = -direction;
        if (rb.velocity.magnitude > 0)
        {
            float angle = Vector3.Angle(Vector3.left, rb.velocity.normalized);
            if (direction.y < 0.0f)
            {
                angle = 360f - angle;
            }
            float diff = angle - weapon.transform.eulerAngles.z;
            if (diff < 0f)
            {
                diff = diff + 360;
            }
            if (diff < 180f)
            {
                weapon.rotateLeft = true;
            }
            else
            {
                weapon.rotateLeft = false;
            }

            weapon.targetAngle = angle;
            weapon.isRotating = true;
        }
    }
}
