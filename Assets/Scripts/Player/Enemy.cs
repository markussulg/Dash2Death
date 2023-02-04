using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public WeaponMovement weapon;
    public Vector3 direction;
    public bool knockback;
    public float knockbackForce = 1000f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        if (knockback) return;
        direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
        //direction = -direction;
        if (rb.velocity.magnitude > 0)
        {
            float angle = Vector3.Angle(Vector3.left, rb.velocity.normalized);
            if (direction.y < 0.0f) angle = 360f - angle;

            float diff = angle - weapon.transform.eulerAngles.z;
            if (diff < 0f) diff = diff + 360;

            weapon.rotateLeft = diff < 180f;
            weapon.targetAngle = angle;
            weapon.isRotating = true;
        }
    }

    public IEnumerator GetHit(Vector3 dir, float duration, BoxCollider2D collider)
    {
        print("hit");
        knockback = true;
        rb.AddForce(dir * knockbackForce);
        yield return new WaitForSeconds(duration);
        knockback = false;
        collider.isTrigger = false;

    }
}
