public class CurrencyModel : ICurrencyModel
{
    private int coins;

    public CurrencyModel(int coins)
    {
        this.coins = coins;
    }

    public virtual void SpendCoins(int value)
    {
        coins -= value;
    }

    public virtual int GetCoins()
    {
        return coins;
    }

    public virtual void AddCoins(int value)
    {
        coins += value;
    }

    public void Update(float dt) {}
}