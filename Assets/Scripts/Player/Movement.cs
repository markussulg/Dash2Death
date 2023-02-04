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
    private float startRotationSpeed;

    private Vector2 direction;

    [SerializeField] PlayerNonPooledDynamicSpawner playerSpawner;

    private void Awake() {
        playerSpawner.OnPlayerSpawned += HandlePlayerSpawned;
    }

    private void HandlePlayerSpawned(NetworkObject playerSword, NetworkObject playerVisual) {
        weapon = playerSword.GetComponent<WeaponMovement>();
        weapon.target = transform;
        startRotationSpeed = weapon.orbitDegreesPerSec;
    }

    // Update is called once per frame
    void Update() {
        if (weapon == null) return;
        if (isDashing) return;
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        rb.velocity = direction * speed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
            weapon.orbitDegreesPerSec = startRotationSpeed * 2;
            StartCoroutine(Dash());
        }
        if (rb.velocity.magnitude > 0) {
            float angle = Vector3.Angle(Vector3.right, rb.velocity.normalized);
            if (direction.y < 0.0f) {
                angle = 360f - angle;
            }
            float diff = angle - weapon.transform.eulerAngles.z;
            if (diff < 0f) {
                diff = diff + 360;
            }
            if (diff < 180f) {
                weapon.rotateLeft = true;
            }
            else {
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
        weapon.orbitDegreesPerSec = startRotationSpeed;
    }
}
