
namespace Bikelogic.Negocio.Inventario;

public class Product
{
    public string Code { get; }
    public string Category { get; }
    public int Stock { get; private set; }

    public Product(string code, string category, int initialStock)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("El código del producto es obligatorio.");

        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("La categoría es obligatoria.");

        if (initialStock < 0)
            throw new ArgumentException("El stock inicial no puede ser negativo.");

        Code = code;
        Category = category;
        Stock = initialStock;
    }

    public string Withdraw(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que cero.");

        if (quantity > Stock)
            return "Stock insuficiente";

        Stock -= quantity;
        return "Retiro exitoso";
    }
}
