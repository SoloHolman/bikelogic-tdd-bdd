namespace Bikelogic.Negocio.Proveedores;

public class InMemorySupplierRepository : ISupplierRepository
{
    private readonly List<Supplier> _proveedores = new();
    private int _siguienteId = 1;

    public int Count => _proveedores.Count;

    public bool ExistsByNit(string nit) => _proveedores.Any(p => p.Nit == nit);

    public Supplier? GetByNit(string nit) => _proveedores.FirstOrDefault(p => p.Nit == nit);

    public void Add(Supplier proveedor)
    {
        proveedor.Id = _siguienteId++;
        _proveedores.Add(proveedor);
    }

    public void Update(Supplier proveedor)
    {
        // En memoria la referencia ya está actualizada; con Entity Framework aquí se guardarían los cambios.
    }
}
