using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class PlayerCanvas : NetworkBehaviour
{
    public TextMeshProUGUI playerText;
    public Image healthFill;

    private NetworkVariable<float> healthFillAmount = new NetworkVariable<float>();

    public int maxHealth;
    private int currentHealth;

    private void Start()
    {
        healthFillAmount.Value = maxHealth;
        currentHealth = maxHealth;

        if (!IsServer) {
            healthFillAmount.OnValueChanged += HandleValueChanged;
        }
    }

    private void HandleValueChanged(float previous, float current) {
        UpdateHealthClientRpc(current);
    }

    [ServerRpc]
    private void UpdateHealthServerRpc(float value) {
        healthFillAmount.Value = value;
        healthFill.fillAmount = value;
    }

    [ClientRpc]
    public void UpdateHealthClientRpc(float value) {
        healthFill.fillAmount = value;
    }

    public void SetPlayerName(string text)
    {
        playerText.text = text;
    }
    public bool DecreaseHealth()
    {
        currentHealth -= 1;
        print(currentHealth);
        healthFill.fillAmount = 1.0f * currentHealth / maxHealth;
        UpdateHealthServerRpc(healthFill.fillAmount);
        if (currentHealth<=0)
        {
            return true;
        }
        return false;
    }

}
