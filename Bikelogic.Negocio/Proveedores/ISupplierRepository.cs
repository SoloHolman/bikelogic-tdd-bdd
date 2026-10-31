namespace Bikelogic.Negocio.Proveedores;

public interface ISupplierRepository
{
    bool ExistsByNit(string nit);
    Supplier? GetByNit(string nit);
    void Add(Supplier proveedor);
    void Update(Supplier proveedor);
}
