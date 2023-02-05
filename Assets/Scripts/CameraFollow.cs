using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraFollow : NetworkBehaviour {

    private Transform followTarget;
    private Vector3 cameraOffset = new Vector3(0, 0, -10);

    public override void OnNetworkSpawn() {
        gameObject.SetActive(IsOwner);
        if (gameObject.activeSelf) return;

        //followTarget = NetworkManager.SpawnManager.GetLocalPlayerObject().transform;
    }

    private void LateUpdate() {
        if (followTarget == null) return;
        if (!IsOwner) return;

        transform.position = followTarget.position + cameraOffset;
    }

    public void SetFollowTarget(Transform followTarget) {
        this.followTarget = followTarget;
    }
}
