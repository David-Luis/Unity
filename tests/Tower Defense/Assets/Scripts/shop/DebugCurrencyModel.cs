using UnityEngine;
using System.Collections;

public class DebugCurrencyModel : CurrencyModel
{
    public DebugCurrencyModel(int coins) : base(coins)
    {
        Debug.Log("Starting coins: " + coins);
    }

    public override void SpendCoins(int value)
    {
        Debug.Log("Spending coins: " + value);
        base.SpendCoins(value);
        Debug.Log("Remaining coins: " + GetCoins());
    }

    public override void AddCoins(int value)
    {
        Debug.Log("Adding coins: " + value);
        base.AddCoins(value);
        Debug.Log("currentCoins coins: " + GetCoins());
    }
}
