using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour {

    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    Vector2 direction;

    // Update is called once per frame
    void Update() {
        PlayerInputs();

        if (direction != Vector2.zero) {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, -direction);
            rotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate() {
        if (!IsOwner) return;

        MovePlayerServerRpc();
    }

    [ServerRpc]
    private void MovePlayerServerRpc() {
        body.velocity = direction * moveSpeed;
        Debug.Log(body.velocity);
    }

    private void PlayerInputs() {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
    }
}
