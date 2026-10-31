
namespace Bikelogic.Negocio.Inventario;

public class InventoryService
{
    private readonly Dictionary<string, Product> _products = new();

    public void RegisterProduct(Product product)
    {
        if (!_products.TryAdd(product.Code, product))
        {
            throw new InvalidOperationException(
                "El código del producto ya está registrado.");
        }
    }

    public Product GetProduct(string code)
    {
        if (!_products.TryGetValue(code, out var product))
        {
            throw new InvalidOperationException(
                "El producto no existe.");
        }

        return product;
    }

    public string WithdrawStock(string code, int quantity)
    {
        var product = GetProduct(code);
        return product.Withdraw(quantity);
    }
}
