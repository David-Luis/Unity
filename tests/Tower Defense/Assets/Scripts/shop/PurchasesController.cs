using UnityEngine;

public class PurchasesController : IGameSystem
{
    private Products products;

    public PurchasesController()
    {
        products = new Products();
    }

    public void Destroy()
    {
    }

    public int GetProductValue(string productId)
    {
        Product product = products.GetProduct(productId);
        if (product != null)
        {
            return product.Value;
        }

        Debug.LogError("Product not found: " + productId);
        return 0;
    }


    public bool Purchase(string productId)
    {
        Product product = products.GetProduct(productId);
        if (product != null)
        {
            if (product.Value <= Systems.currencyModel.GetCoins())
            {
                Systems.currencyModel.SpendCoins(product.Value);
                return true;
            }
            else
            {
                Debug.LogError("Not enough coins: " + productId);
                return false;
            }
        }
        else
        {
            Debug.LogError("Product not found: " + productId);
            return false;
        }
    }

    public void Update(float deltaTime) { }
}
