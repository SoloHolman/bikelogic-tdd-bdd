using System.Text.RegularExpressions;

namespace Bikelogic.Negocio.Proveedores;

public class SupplierService
{
    public const string MensajeRegistrado = "Proveedor registrado exitosamente";
    public const string MensajeDesactivado = "Proveedor desactivado";

    private readonly ISupplierRepository _proveedores;
    private readonly IPurchaseOrderRepository _ordenes;

    public SupplierService(ISupplierRepository proveedores, IPurchaseOrderRepository ordenes)
    {
        _proveedores = proveedores;
        _ordenes = ordenes;
    }

    public Supplier CreateSupplier(Supplier proveedor)
    {
        if (string.IsNullOrWhiteSpace(proveedor.Nombre))
            throw new InvalidSupplierException("El nombre es obligatorio");

        if (!NitEsValido(proveedor.Nit))
            throw new InvalidSupplierException("NIT inválido");

        if (!CorreoEsValido(proveedor.Correo))
            throw new InvalidSupplierException("Formato de correo inválido");

        if (_proveedores.ExistsByNit(proveedor.Nit))
            throw new InvalidSupplierException("El NIT ya está registrado");

        proveedor.Estado = "ACTIVO";
        _proveedores.Add(proveedor);
        return proveedor;
    }

    public Supplier DeleteSupplier(string nit)
    {
        var proveedor = _proveedores.GetByNit(nit)
            ?? throw new InvalidSupplierException("El proveedor no existe");

        if (_ordenes.HasActiveOrders(nit))
            throw new SupplierHasActiveOrdersException();

        proveedor.Estado = "INACTIVO";
        _proveedores.Update(proveedor);
        return proveedor;
    }

    // Supuesto: NIT válido = solo dígitos, entre 9 y 10 caracteres.
    public static bool NitEsValido(string nit) =>
        !string.IsNullOrWhiteSpace(nit) && Regex.IsMatch(nit, @"^\d{9,10}$");

    public static bool CorreoEsValido(string correo) =>
        !string.IsNullOrWhiteSpace(correo) && Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
}
