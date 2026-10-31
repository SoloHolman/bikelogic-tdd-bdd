namespace Bikelogic.Negocio.Proveedores;

public class InvalidSupplierException : Exception
{
    public InvalidSupplierException(string mensaje) : base(mensaje)
    {
    }
}

public class SupplierHasActiveOrdersException : Exception
{
    public SupplierHasActiveOrdersException()
        : base("No se puede eliminar un proveedor con órdenes activas")
    {
    }
}
