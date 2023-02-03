using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static event Action<int> OnChangeMoney;
    public static void ChangeMoney(int amount) => OnChangeMoney?.Invoke(amount);

    public static event Func<int> OnRequestMoney;
    public static int RequestMoney() => OnRequestMoney?.Invoke() ?? 0;
}
