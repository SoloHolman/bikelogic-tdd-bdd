using Bikelogic.Negocio.Proveedores;

namespace Bikelogic.Pruebas.Proveedores;

public class FakePurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly HashSet<string> _nitsConOrdenesActivas = new();

    public void AgregarOrdenActiva(string nit) => _nitsConOrdenesActivas.Add(nit);

    public bool HasActiveOrders(string nit) => _nitsConOrdenesActivas.Contains(nit);
}

public class SpySupplierRepository : ISupplierRepository
{
    private readonly InMemorySupplierRepository _interno = new();

    public Supplier? UltimoAgregado { get; private set; }
    public int VecesQueSeLlamoAdd { get; private set; }

    public bool ExistsByNit(string nit) => _interno.ExistsByNit(nit);

    public Supplier? GetByNit(string nit) => _interno.GetByNit(nit);

    public void Add(Supplier proveedor)
    {
        VecesQueSeLlamoAdd++;
        UltimoAgregado = proveedor;
        _interno.Add(proveedor);
    }

    public void Update(Supplier proveedor) => _interno.Update(proveedor);
}
