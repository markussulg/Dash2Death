using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNonPooledDynamicSpawner : NetworkBehaviour {

    public event Action<NetworkObject, NetworkObject, NetworkObject, NetworkObject> OnPlayerSpawned;

    public PlayerSO playerSO;
    public bool DestroyWithSpawner;

    private GameObject playerSwordPrefab;
    private GameObject playerVisualPrefab;
    private GameObject playerCameraPrefab;

    private GameObject playerCanvasPrefab;
    private GameObject playerHealthPrefab;
    private GameObject playerHealthFillPrefab;

    public NetworkObject spawnedPlayerSword;
    public NetworkObject spawnedPlayerVisual;
    public NetworkObject spawnedPlayerCamera;

    public NetworkObject spawnedPlayerCanvas;
    public NetworkObject spawnedPlayerHealthFill;
    public NetworkObject spawnedPlayerHealth;

    public override void OnNetworkSpawn() {
        // Only the server spawns, clients will disable this component on their side
        playerSwordPrefab = playerSO.playerSwordPrefab;
        playerVisualPrefab = playerSO.playerVisualPrefab;
        playerCameraPrefab = playerSO.playerCameraPrefab;

        playerCanvasPrefab = playerSO.playerCanvasPrefab;
        playerHealthPrefab = playerSO.playerHealthPrefab;
        playerHealthFillPrefab = playerSO.playerHealthFillPrefab;

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
        playerCameraNetworkObject.transform.localPosition = new Vector3(0, 0, -16);


        //HEALTH BAR:

        NetworkObject playerCanvasNetworkObject = Instantiate<NetworkObject>(
            playerCanvasPrefab.GetComponent<NetworkObject>());

        NetworkObject playerHealthNetworkObject = Instantiate<NetworkObject>(
            playerHealthPrefab.GetComponent<NetworkObject>());

        NetworkObject playerHealthFillNetworkObject = Instantiate<NetworkObject>(
            playerHealthFillPrefab.GetComponent<NetworkObject>());

        playerCanvasNetworkObject.SpawnWithOwnership(clientId);
        playerHealthNetworkObject.SpawnWithOwnership(clientId);
        playerHealthFillNetworkObject.SpawnWithOwnership(clientId);

        playerCanvasNetworkObject.TrySetParent(transform);
        playerHealthNetworkObject.TrySetParent(playerCanvasNetworkObject.transform);
        playerHealthFillNetworkObject.TrySetParent(playerHealthNetworkObject.transform);

        playerCanvasNetworkObject.transform.localPosition = new Vector3(0, -1, 0);
        playerHealthNetworkObject.transform.localPosition = Vector3.zero;
        playerHealthFillNetworkObject.transform.localPosition = Vector3.zero;

        ClientRpcParams clientRpcParams = new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        PlayerSpawnedClientRpc(playerSwordNetworkObject, playerCameraNetworkObject, playerCanvasNetworkObject,
            playerHealthNetworkObject, playerHealthFillNetworkObject, clientRpcParams);
    }

    [ClientRpc]
    private void PlayerSpawnedClientRpc(NetworkObjectReference playerSword, NetworkObjectReference playerCamera,
        NetworkObjectReference playerCanvas, NetworkObjectReference playerHealth, NetworkObjectReference playerHealthFill,
        ClientRpcParams clientRpcParams) {

        spawnedPlayerSword = playerSword;
        spawnedPlayerCamera = playerCamera;
        spawnedPlayerCamera.transform.localPosition = new Vector3(0, 0, -16);
        //spawnedPlayerVisual = playerVisual;

        spawnedPlayerCanvas = playerCanvas;
        spawnedPlayerCanvas.transform.localPosition = new Vector3(0, -1, 0);
        spawnedPlayerHealth = playerHealth;
        spawnedPlayerHealth.transform.localPosition = Vector3.zero;
        spawnedPlayerHealthFill = playerHealthFill;
        spawnedPlayerHealthFill.transform.localPosition = Vector3.zero;

        OnPlayerSpawned?.Invoke(spawnedPlayerSword, spawnedPlayerCanvas, spawnedPlayerHealth, spawnedPlayerHealthFill);
    }

    /*public override void OnNetworkDespawn() {
        if (IsServer && DestroyWithSpawner && m_SpawnedNetworkObject != null && m_SpawnedNetworkObject.IsSpawned) {
            m_SpawnedNetworkObject.Despawn();
        }
        base.OnNetworkDespawn();
    }*/
}