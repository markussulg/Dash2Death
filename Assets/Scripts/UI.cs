using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI MoneyText;
    private void Awake()
    {
        Events.OnChangeMoney += SetMoney;
    }
    private void OnDestroy()
    {
        Events.OnChangeMoney -= SetMoney;
    }
    private void Start()
    {
        Events.ChangeMoney(2);
    }
    void SetMoney(int value)
    {
        MoneyText.text = "Money: " + value.ToString();
    }
}
