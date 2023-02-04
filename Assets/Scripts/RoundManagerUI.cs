using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RoundManagerUI : NetworkBehaviour {

    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI roundTimer;

    public override void OnNetworkSpawn() {
        NetworkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkVariable<int> currentRound = RoundManager.Instance.GetCurrentRoundNetworkVariable();

        currentRound.OnValueChanged += HandleRoundChanged;
    }

    private void Start() {

    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj) {
        StartCoroutine(StartChangingNetworkVariable());

        NetworkManager.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
    }

    private void Update() {
        if (!IsOwner) return;

        DisplayTime();
    }
    void DisplayTime() {
        float timeToDisplay = RoundManager.Instance.GetRoundTime();
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        roundTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void HandleRoundChanged(int previous, int current) {
        if (previous == current) return;

        roundText.text = "Round " + RoundManager.Instance.GetCurrentRound();
    }

    private IEnumerator StartChangingNetworkVariable() {
        roundText.text = "Round " + RoundManager.Instance.GetCurrentRound();

        while (RoundManager.Instance.GetRoundTime() > 0) {
            DisplayTime();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
