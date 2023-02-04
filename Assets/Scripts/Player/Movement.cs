using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public TrailRenderer tr;
    public WeaponMovement weapon;
    public bool knockback = false;
    public float knockbackForce = 1000f;
    public float dashingPower = 2f;
    public float dashingTime = 0.2f;

    private bool canDash = true;
    private bool isDashing;
    private float dashingCooldown = 1f;
    private float startRotationSpeed;


    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        startRotationSpeed = weapon.orbitDegreesPerSec;
    }

    // Update is called once per frame
    void Update()
    {
        if (knockback) return;
        if (isDashing) return;
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        rb.velocity = direction * speed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            weapon.orbitDegreesPerSec = startRotationSpeed * 2;
            StartCoroutine(Dash());
        }
        if (rb.velocity.magnitude > 0)
        {
            float angle = Vector3.Angle(Vector3.right, rb.velocity.normalized);
            if (direction.y < 0.0f) angle = 360f - angle;
            
            float diff = angle - weapon.transform.eulerAngles.z;
            if (diff < 0f) diff = diff + 360;

            weapon.rotateLeft = diff < 180f;
            weapon.targetAngle = angle;
            weapon.isRotating = true;
        }

    }

    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        rb.velocity = direction * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        canDash = true;
        isDashing = false;
        tr.emitting = false;
        weapon.orbitDegreesPerSec = startRotationSpeed;
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
