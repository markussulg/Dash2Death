using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public TrailRenderer tr;
    public WeaponMovement weapon;

    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 2f;
    public float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) return;
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        rb.velocity = direction * speed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            
            StartCoroutine(Dash());
        }
        if (rb.velocity.magnitude > 0)
        {
            float angle = Vector3.Angle(Vector3.right, direction);
            if (direction.y < 0.0f)
            {
                angle = 360f - angle;
            }
            //print(angle);
            print(angle);
            print(weapon.transform.eulerAngles.z);
            if (Mathf.Abs(angle - weapon.transform.eulerAngles.z) > 180f)
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

    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        rb.velocity = direction * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        canDash = true;
        isDashing = false;
        tr.emitting = false;
    }
}
