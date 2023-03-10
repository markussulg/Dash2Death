using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {
    public float speed = 5;
    public WeaponMovement weapon;
    public PlayerCanvas playerCanvas;
    public bool knockback = false;
    public float knockbackForce = 1000f;
    public float wallKnockbackForce = 500f;
    public float dashingPower = 20f;
    public float dashingTime = 0.2f;
    public GameObject canvas;

    private Rigidbody2D rb;
    private TrailRenderer tr;
    private bool canDash = true;
    private bool isDashing;
    private float dashingCooldown = 1f;
    private float startRotationSpeed;

    public Vector2 direction;

    [SerializeField] PlayerNonPooledDynamicSpawner playerSpawner;

    private void Awake() {
        if (playerSpawner != null)
        {
            playerSpawner.OnPlayerSpawned += HandlePlayerSpawned;
        }
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        if(startRotationSpeed == 0)
        {
            startRotationSpeed = weapon.orbitDegreesPerSec;
        }
    }

    private void HandlePlayerSpawned(NetworkObject playerSword, NetworkObject playerCanvas,
        NetworkObject playerHealth, NetworkObject playerHealthFill) {
        weapon = playerSword.GetComponent<WeaponMovement>();
        weapon.target = transform;
        this.playerCanvas = playerCanvas.GetComponent<PlayerCanvas>();
        this.playerCanvas.healthFill = playerHealthFill.GetComponent<Image>();
        startRotationSpeed = weapon.orbitDegreesPerSec;
    }

    void Update() {
        if (weapon == null) return;
        if (knockback) return;
        if (isDashing) return;
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        rb.velocity = direction * speed;

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space)) && canDash) {
            weapon.orbitDegreesPerSec = startRotationSpeed * 2;
            StartCoroutine(Dash());
        }
        if (rb.velocity.magnitude > 0) {
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
        AudioController.Instance.PlayDashAudio();
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

    public IEnumerator GetHit(Vector3 dir, float duration, PolygonCollider2D collider, bool getDmg = true) {
        if (playerCanvas != null) {
            AudioController.Instance.PlayPlayerHitAudio();
            bool isDead = playerCanvas.DecreaseHealth();
            knockback = true;
            rb.AddForce(dir * knockbackForce);
            yield return new WaitForSeconds(duration);
            knockback = false;
            collider.isTrigger = false;
            if (isDead)
            {
                canvas.SetActive(true);
                Destroy(gameObject);
            } 
            yield break;
        }
    }

    public IEnumerator WallHit(Vector3 dir, float duration, PolygonCollider2D collider)
    {
        AudioController.Instance.PlaySolidHitAudio();
        print("hit");
        knockback = true;
        rb.AddForce(dir * wallKnockbackForce);
        yield return new WaitForSeconds(duration);
        knockback = false;
        collider.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WeaponMovement>() == weapon) return;
    }
}
