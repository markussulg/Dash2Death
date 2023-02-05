using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : NetworkBehaviour {

    private const string GAME_SCENE_NAME = "Kits";

    public static RoundManager Instance;

    public event Action OnRoundStarted;

    [SerializeField] float maxRoundTimeInSeconds = 120;

    public NetworkVariable<float> timeRemaining = new NetworkVariable<float>();
    public bool timerIsRunning = false;

    private NetworkVariable<int> currentRound = new NetworkVariable<int>();
    private int maxRounds = 3;
    private int startingRound = 1;

    private float spawnCircleRadius = 5f;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        LobbyOrchestrator.Instance.OnGameStarted += HandleGameStarted;
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    public override void OnNetworkSpawn() {
        if (IsServer) {
            timeRemaining.Value = maxRoundTimeInSeconds;
            currentRound.Value = startingRound;
            PauseControls();
        }
    }

    private void Update() {
        if (!IsOwner) return;

        UpdateTimer();
    }

    private void HandleGameStarted(int playerAmount) {
        SetSpawnLocations(playerAmount);
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        if (scene.name != GAME_SCENE_NAME) return;

        StartRoundCountdown();
    }

    private void SetSpawnLocations(int playerAmount) {
        float angleIncrement = Mathf.FloorToInt(360f / playerAmount);

        for (int i = 0; i < playerAmount; i++) {
            transform.Rotate(new Vector3(0f, 0f, angleIncrement));
            NetworkObject playerNO = NetworkManager.SpawnManager.GetPlayerNetworkObject((ulong)i);
            SetPlayerSpawnServerRpc(playerNO, transform.right * spawnCircleRadius);
        }
    }

    [ServerRpc]
    private void SetPlayerSpawnServerRpc(NetworkObjectReference player, Vector3 spawnLocation) {
        NetworkObject playerNO = player;
        playerNO.transform.position = spawnLocation;

        Debug.Log(playerNO);
    }

    private void UpdateTimer() {
        if (timerIsRunning) {
            if (timeRemaining.Value > 0) {
                timeRemaining.Value -= Time.deltaTime;
            }
            else {
                timeRemaining.Value = 0;
                timerIsRunning = false;
                EndRound();
            }
        }
    }

    private void EndRound() {
        PauseControls();
        if (currentRound.Value < maxRounds) {
            //Continue game
            StartRoundCountdown();
        }

        //End the game.
    }

    private void StartRoundCountdown() {
        RoundManagerUI.Instance.StartCountdown(StartRound);
    }

    private void StartRound() {
        ResumeControls();
        ResetTimer();
        timerIsRunning = true;
        currentRound.Value++;

        OnRoundStarted?.Invoke();
    }

    private void ResetTimer() {
        timeRemaining.Value = maxRoundTimeInSeconds;
    }

    public NetworkVariable<int> GetCurrentRoundNetworkVariable() {
        return currentRound;
    }

    public float GetRoundTime() {
        return timeRemaining.Value;
    }

    public int GetCurrentRound() {
        return currentRound.Value;
    }

    private void PauseControls() {
        NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<Movement>().enabled = false;
    }

    private void ResumeControls() {
        NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<Movement>().enabled = true;
    }
}
