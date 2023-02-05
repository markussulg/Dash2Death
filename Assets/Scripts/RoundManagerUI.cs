using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RoundManagerUI : NetworkBehaviour {

    public static RoundManagerUI Instance;

    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI roundTimer;
    [SerializeField] private TextMeshProUGUI countdownText;

    private int roundCountdownStart = 3;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {
        NetworkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkVariable<int> currentRound = RoundManager.Instance.GetCurrentRoundNetworkVariable();

        currentRound.OnValueChanged += HandleRoundChanged;
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
    public void StartCountdown(Action callback) {
        StartCoroutine(StartCountdownTimer(callback));
        StartCoroutine(StartChangingNetworkVariable());
    }

    private IEnumerator StartCountdownTimer(Action callback) {
        countdownText.gameObject.SetActive(true);
        roundText.gameObject.SetActive(false);
        roundTimer.gameObject.SetActive(false);
        int currentTime = roundCountdownStart;

        while (currentTime > 0) {
            countdownText.text = currentTime.ToString();
            currentTime--;
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        countdownText.gameObject.SetActive(false);
        roundText.gameObject.SetActive(true);
        roundTimer.gameObject.SetActive(true);
        callback.Invoke();
    }

    private IEnumerator StartChangingNetworkVariable() {
        roundText.text = "Round " + RoundManager.Instance.GetCurrentRound();

        while (RoundManager.Instance.GetRoundTime() > 0) {
            DisplayTime();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
