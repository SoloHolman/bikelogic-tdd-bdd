namespace Bikelogic.Negocio.Proveedores;

public interface IPurchaseOrderRepository
{
    bool HasActiveOrders(string nit);
}
