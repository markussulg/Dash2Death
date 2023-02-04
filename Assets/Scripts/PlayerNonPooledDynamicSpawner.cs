using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNonPooledDynamicSpawner : NetworkBehaviour {

    public PlayerSO playerSO;
    public bool DestroyWithSpawner;

    private GameObject playerSwordPrefabInstance;
    private NetworkObject spawnedPlayerSword;

    private GameObject playerVisualPrefabInstance;
    private NetworkObject spawnedPlayerVisual;

    public override void OnNetworkSpawn() {
        // Only the server spawns, clients will disable this component on their side
        enabled = IsServer;
        if (!enabled || playerSO == null) {
            return;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;

        playerSwordPrefabInstance = Instantiate(playerSO.playerSwordPrefab);
        playerVisualPrefabInstance = Instantiate(playerSO.playerVisualPrefab);

        spawnedPlayerSword = playerSwordPrefabInstance.GetComponent<NetworkObject>();
        spawnedPlayerVisual = playerVisualPrefabInstance.GetComponent<NetworkObject>();

        spawnedPlayerSword.Spawn();
        spawnedPlayerVisual.Spawn(); 
    }

    private void Singleton_OnClientConnectedCallback(ulong obj) {
        NetworkObject clientPlayerNetworkObject = NetworkManager.SpawnManager.GetPlayerNetworkObject(obj);

        spawnedPlayerSword.TrySetParent(clientPlayerNetworkObject);
        spawnedPlayerVisual.TrySetParent(clientPlayerNetworkObject);
    }


    /*public override void OnNetworkDespawn() {
        if (IsServer && DestroyWithSpawner && m_SpawnedNetworkObject != null && m_SpawnedNetworkObject.IsSpawned) {
            m_SpawnedNetworkObject.Despawn();
        }
        base.OnNetworkDespawn();
    }*/
}