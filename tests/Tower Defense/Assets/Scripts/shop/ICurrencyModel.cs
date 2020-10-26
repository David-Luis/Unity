public interface ICurrencyModel : IGameSystem
{
    void SpendCoins(int value);
    int GetCoins();
    void AddCoins(int value);
}
