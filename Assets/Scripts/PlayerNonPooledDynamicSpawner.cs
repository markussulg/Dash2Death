using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNonPooledDynamicSpawner : NetworkBehaviour {

    public event Action<NetworkObject> OnPlayerSpawned;

    public PlayerSO playerSO;
    public bool DestroyWithSpawner;

    private GameObject playerSwordPrefab;
    private GameObject playerVisualPrefab;
    private GameObject playerCameraPrefab;

    public NetworkObject spawnedPlayerSword;
    public NetworkObject spawnedPlayerVisual;
    public NetworkObject spawnedPlayerCamera;

    public override void OnNetworkSpawn() {
        // Only the server spawns, clients will disable this component on their side
        playerSwordPrefab = playerSO.playerSwordPrefab;
        playerVisualPrefab = playerSO.playerVisualPrefab;
        playerCameraPrefab = playerSO.playerCameraPrefab;

        enabled = IsOwner;
        if (!enabled) return;

        SpawnPlayerServerRpc(OwnerClientId);
    }

    [ServerRpc]
    private void SpawnPlayerServerRpc(ulong clientId) {
        NetworkObject playerSwordNetworkObject = Instantiate<NetworkObject>(
            playerSwordPrefab.GetComponent<NetworkObject>());

        NetworkObject playerCameraNetworkObject = Instantiate<NetworkObject>(
            playerCameraPrefab.GetComponent<NetworkObject>());

        //NetworkObject playerVisualNetworkObject = Instantiate<NetworkObject>(
        //    playerVisualPrefab.GetComponent<NetworkObject>());

        playerSwordNetworkObject.SpawnWithOwnership(clientId);
        //playerVisualNetworkObject.SpawnWithOwnership(clientId);
        playerCameraNetworkObject.SpawnWithOwnership(clientId);

        playerSwordNetworkObject.TrySetParent(transform);
        //playerVisualNetworkObject.TrySetParent(transform);
        playerCameraNetworkObject.TrySetParent(transform);

        playerCameraNetworkObject.enabled = playerCameraNetworkObject.OwnerClientId == clientId;
        playerCameraNetworkObject.GetComponent<CameraFollow>().SetFollowTarget(transform);
        playerCameraNetworkObject.transform.localPosition = playerCameraNetworkObject.transform.localPosition +
            new Vector3(0, 0, -10);

        ClientRpcParams clientRpcParams = new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        PlayerSpawnedClientRpc(playerSwordNetworkObject);
    }

    [ClientRpc]
    private void PlayerSpawnedClientRpc(NetworkObjectReference playerSword) {

        spawnedPlayerSword = playerSword;
        //spawnedPlayerVisual = playerVisual;

        OnPlayerSpawned?.Invoke(spawnedPlayerSword);
    }

    /*public override void OnNetworkDespawn() {
        if (IsServer && DestroyWithSpawner && m_SpawnedNetworkObject != null && m_SpawnedNetworkObject.IsSpawned) {
            m_SpawnedNetworkObject.Despawn();
        }
        base.OnNetworkDespawn();
    }*/
}