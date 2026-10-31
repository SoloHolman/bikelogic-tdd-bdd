
namespace Bikelogic.Negocio.OrdenesCompra;

public class PurchaseOrder
{
    public string Number { get; }
    public string Supplier { get; }
    public decimal Amount { get; }
    public string Status { get; private set; }

    public PurchaseOrder(string number, string supplier, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("El numero de orden es obligatorio.");

        if (string.IsNullOrWhiteSpace(supplier))
            throw new ArgumentException("El proveedor es obligatorio.");

        if (amount <= 0)
            throw new ArgumentException("El monto debe ser mayor que cero.");

        Number = number;
        Supplier = supplier;
        Amount = amount;
        Status = "PENDIENTE";
    }

    public void Approve()
    {
        if (Status == "APROBADA")
            throw new InvalidOperationException(
                "La orden ya esta aprobada.");

        Status = "APROBADA";
    }
}
