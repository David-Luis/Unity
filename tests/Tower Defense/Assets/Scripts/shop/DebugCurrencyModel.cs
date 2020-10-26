using UnityEngine;
using System.Collections;

public class DebugCurrencyModel : CurrencyModel
{
    public DebugCurrencyModel(int coins) : base(coins)
    {
        Debug.Log("Starting coins: " + coins);
    }

    public override void SpendCoins(int coins)
    {
        Debug.Log("Spending coins: " + coins);
        SpendCoins(coins);
        Debug.Log("Remaining coins: " + coins);
    }
}
