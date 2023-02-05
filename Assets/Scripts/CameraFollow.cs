using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraFollow : NetworkBehaviour {

    private Transform followTarget;

    public override void OnNetworkSpawn() {
        enabled = IsOwner;
        if (!enabled) return;

        followTarget = NetworkManager.SpawnManager.GetLocalPlayerObject().transform;
    }

    private void LateUpdate() {
        transform.position = followTarget.position;
    }

    public void SetFollowTarget(Transform followTarget) {
        this.followTarget = followTarget;
    }
}
