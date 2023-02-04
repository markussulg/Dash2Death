using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class Movement : NetworkBehaviour {
    public Rigidbody2D rb;
    public float speed;
    public TrailRenderer tr;
    public WeaponMovement weapon;

    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 2f;
    public float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private Vector2 direction;

    [SerializeField] PlayerNonPooledDynamicSpawner playerSpawner;

    private void Awake() {
        playerSpawner.OnPlayerSpawned += HandlePlayerSpawned;
    }

    private void HandlePlayerSpawned(NetworkObject playerSword, NetworkObject playerVisual) {
        weapon = playerSword.GetComponent<WeaponMovement>();
        weapon.target = transform;
    }

    void Update()
    {
        if (!IsOwner) return;
        if (weapon == null) return;

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
