using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RoundManager : NetworkBehaviour {

    public static RoundManager Instance;

    public event Action OnRoundStarted;

    [SerializeField] float maxRoundTimeInSeconds = 10;

    public NetworkVariable<float> timeRemaining = new NetworkVariable<float>();
    public bool timerIsRunning = false;

    private NetworkVariable<int> currentRound = new NetworkVariable<int>();
    private int maxRounds = 3;
    private int startingRound = 1;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public override void OnNetworkSpawn() {
        if (IsServer) {
            timeRemaining.Value = maxRoundTimeInSeconds;
            currentRound.Value = startingRound;
        }
    }

    private void Start() {
        timerIsRunning = true;
    }

    private void Update() {
        if (!IsOwner) return;

        UpdateTimer();
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
        if (currentRound.Value < maxRounds) {
            //Continue game
        }

        //End the game.
    }

    private void StartRound() {
        ResetTimer();
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
}
