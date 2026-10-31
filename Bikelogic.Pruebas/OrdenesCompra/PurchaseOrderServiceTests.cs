
using Bikelogic.Negocio.OrdenesCompra;

namespace Bikelogic.Pruebas.OrdenesCompra;

public class PurchaseOrderServiceTests
{
    [Fact]
    public void AprobarOrden_ConPresupuestoSuficiente_DescuentaElMonto()
    {
        var service = new PurchaseOrderService();
        service.AddSupplier("ACME", 10_000_000m);

        var order = new PurchaseOrder("OC-100", "ACME", 5_000_000m);
        service.AddOrder(order);

        service.ApproveOrder("OC-100");

        Assert.Equal("APROBADA", order.Status);
        Assert.Equal(5_000_000m, service.GetBudget("ACME"));
    }

    [Fact]
    public void AprobarOrden_SinPresupuestoSuficiente_LanzaExcepcion()
    {
        var service = new PurchaseOrderService();
        service.AddSupplier("ACME", 4_000_000m);

        var order = new PurchaseOrder("OC-101", "ACME", 5_000_000m);
        service.AddOrder(order);

        Assert.Throws<InvalidOperationException>(
            () => service.ApproveOrder("OC-101"));

        Assert.Equal("PENDIENTE", order.Status);
        Assert.Equal(4_000_000m, service.GetBudget("ACME"));
    }

    [Fact]
    public void AprobarOrden_DosVeces_LanzaExcepcion()
    {
        var service = new PurchaseOrderService();
        service.AddSupplier("ACME", 10_000_000m);

        var order = new PurchaseOrder("OC-102", "ACME", 5_000_000m);
        service.AddOrder(order);

        service.ApproveOrder("OC-102");

        Assert.Throws<InvalidOperationException>(
            () => service.ApproveOrder("OC-102"));

        Assert.Equal(5_000_000m, service.GetBudget("ACME"));
    }
}
