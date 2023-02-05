using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCanvas : MonoBehaviour
{
    public TextMeshProUGUI playerText;
    public Image healthFill;
    public int maxHealth;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
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
        if (currentHealth<=0)
        {
            return true;
        }
        return false;
    }

}
