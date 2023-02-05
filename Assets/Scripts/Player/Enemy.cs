using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Movement target;
    public float speed = 5f;
    public WeaponMovement weapon;
    public PlayerCanvas playerCanvas;
    public Vector3 direction;
    public bool knockback;
    public float knockbackForce = 1000f;
    public float dashDelay = 2f;
    public float dashingPower = 2f;
    public float dashingTime = 0.2f;
    public List<Vector2> offsets = new List<Vector2>();
    public float offsetMaxTime = 2f;

    private Rigidbody2D rb;
    private TrailRenderer tr;
    private float dashTimer = 0;
    private bool canDash = true;
    private bool isDashing;
    private float dashingCooldown = 1f;
    private Vector2 delayedPos;
    private Vector2 currentOffset = Vector2.zero;
    private int currentOffsetNumber = 0;
    private float offsetTimer = 0;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        currentOffset = offsets[0];
        if(false)
        {
            direction = (target.transform.position - transform.position).normalized;

            float angle = Vector3.Angle(Vector3.right, direction);
            if (direction.y < 0.0f) angle = 360f - angle;

            float diff = angle - weapon.transform.eulerAngles.z;
            if (diff < 0f) diff = diff + 360;

            weapon.rotateLeft = diff < 180f;
            weapon.targetAngle = angle;
            weapon.isRotating = true;
        }

    }

    void Update()
    {
        if (knockback) return;
        if (isDashing) return;
        if(target == null) rb.velocity = Vector3.zero;
        dashTimer += Time.deltaTime;
        offsetTimer += Time.deltaTime;
        //Vector3 offset = new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), 0);
        if (Vector2.Distance(target.transform.position + (Vector3)currentOffset, transform.position) < 3 || offsetTimer > offsetMaxTime)
        {
            offsetTimer = 0;
            //rb.velocity = Vector2.zero;
            // random change direction
            if (1 == Random.Range(0, 2))
            {
                offsets.Reverse();
                currentOffsetNumber -= 1;
            }
            currentOffsetNumber += 1;
            if (currentOffsetNumber >= offsets.Count)
            {

                currentOffsetNumber = 0;
            }
            currentOffset = offsets[currentOffsetNumber];
        }
        if (dashTimer > dashDelay)
        {
            dashTimer = 0;
            StartCoroutine(Dash());
            return;
        }

        direction = (target.transform.position + (Vector3)currentOffset - transform.position).normalized;
        rb.velocity = direction * speed;
        //direction = -direction;
        if (rb.velocity.magnitude > 0)
        {
            float angle = Vector3.Angle(Vector3.right, rb.velocity.normalized);
            if (direction.y > 0.0f) angle = 360f - angle;
            //angle = angle / 2 - 90;
            float diff = angle - weapon.transform.eulerAngles.z;
            if (diff < 0f) diff = diff + 360;

            weapon.rotateLeft = diff < 180f;
            float offset = weapon.rotateLeft ? 90 : -90;
            weapon.targetAngle = angle + offset;
            weapon.isRotating = true;
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = direction * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        canDash = true;
        isDashing = false;
        tr.emitting = false;
        //weapon.orbitDegreesPerSec = startRotationSpeed;
    }

    public IEnumerator GetHit(Vector3 dir, float duration, PolygonCollider2D collider)
    {
        bool isDead = playerCanvas.DecreaseHealth();
        knockback = true;
        rb.AddForce(dir * knockbackForce);
        yield return new WaitForSeconds(duration);
        knockback = false;
        collider.isTrigger = false;
        if (isDead)
        {            
            Destroy(gameObject);
        } 
        yield break;
    }

}
