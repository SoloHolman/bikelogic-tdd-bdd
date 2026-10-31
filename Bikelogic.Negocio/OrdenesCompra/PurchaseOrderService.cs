
namespace Bikelogic.Negocio.OrdenesCompra;

public class PurchaseOrderService
{
    private readonly Dictionary<string, decimal> _budgets = new();

    private readonly Dictionary<string, PurchaseOrder> _orders = new();

    public void AddSupplier(string supplier, decimal budget)
    {
        if (string.IsNullOrWhiteSpace(supplier))
            throw new ArgumentException("El proveedor es obligatorio.");

        if (budget < 0)
            throw new ArgumentException("El presupuesto no puede ser negativo.");

        _budgets[supplier] = budget;
    }

    public void AddOrder(PurchaseOrder order)
    {
        if (!_budgets.ContainsKey(order.Supplier))
            throw new InvalidOperationException(
                "El proveedor no existe.");

        if (!_orders.TryAdd(order.Number, order))
            throw new InvalidOperationException(
                "El número de orden ya está registrado.");
    }

    public void ApproveOrder(string number)
    {
        if (!_orders.TryGetValue(number, out var order))
            throw new InvalidOperationException(
                "La orden no existe.");

        if (order.Status == "APROBADA")
            throw new InvalidOperationException(
                "La orden ya está aprobada.");

        decimal budget = _budgets[order.Supplier];

        if (order.Amount > budget)
            throw new InvalidOperationException(
                "Presupuesto insuficiente.");

        order.Approve();
        _budgets[order.Supplier] = budget - order.Amount;
    }

    public decimal GetBudget(string supplier)
    {
        if (!_budgets.TryGetValue(supplier, out var budget))
            throw new InvalidOperationException(
                "El proveedor no existe.");

        return budget;
    }
}
