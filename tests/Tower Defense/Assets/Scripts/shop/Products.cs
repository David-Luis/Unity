using System.Collections.Generic;

public class Products
{
    public const string TURRET_1 = "TURRET_1";
    public const string TURRET_2 = "TURRET_2";
    public const string TURRET_3 = "TURRET_3";

    private Dictionary<string, Product> products = new Dictionary<string, Product>();

    public Products()
    {
        products.Add(TURRET_1, new Product(1));
        products.Add(TURRET_2, new Product(10));
        products.Add(TURRET_3, new Product(50));
    }

    public Product GetProduct(string product)
    {
        if (products.ContainsKey(product))
        {
            return products[product];
        }

        return null;
    }
}