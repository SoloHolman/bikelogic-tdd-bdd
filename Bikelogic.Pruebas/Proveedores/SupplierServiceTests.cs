using Bikelogic.Negocio.Proveedores;
using Xunit;

namespace Bikelogic.Pruebas.Proveedores;

public class SupplierServiceTests
{
    private readonly InMemorySupplierRepository _repositorio = new();
    private readonly FakePurchaseOrderRepository _ordenes = new();
    private readonly SupplierService _servicio;

    public SupplierServiceTests()
    {
        _servicio = new SupplierService(_repositorio, _ordenes);
    }

    [Fact]
    public void DeberiaCrearProveedorConNitValido()
    {
        var proveedor = new Supplier("900123456", "ACME S.A.S.", "contacto@acme.com");

        var creado = _servicio.CreateSupplier(proveedor);

        Assert.True(creado.Id > 0);
        Assert.Equal("ACTIVO", creado.Estado);
    }

    [Fact]
    public void NoDeberiaCrearProveedorConNitDuplicado()
    {
        _servicio.CreateSupplier(new Supplier("900123456", "ACME", "a@a.com"));
        var duplicado = new Supplier("900123456", "Otra", "b@b.com");

        var ex = Assert.Throws<InvalidSupplierException>(() => _servicio.CreateSupplier(duplicado));

        Assert.Equal("El NIT ya está registrado", ex.Message);
        Assert.Equal(1, _repositorio.Count);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("sin-arroba.com")]
    [InlineData("usuario@")]
    public void NoDeberiaAceptarCorreosInvalidos(string correo)
    {
        var proveedor = new Supplier("900999999", "Test", correo);

        var ex = Assert.Throws<InvalidSupplierException>(() => _servicio.CreateSupplier(proveedor));

        Assert.Equal("Formato de correo inválido", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("ABC123456")]
    [InlineData("12345")]
    public void NoDeberiaAceptarNitInvalido(string nit)
    {
        var proveedor = new Supplier(nit, "Test", "test@test.com");

        var ex = Assert.Throws<InvalidSupplierException>(() => _servicio.CreateSupplier(proveedor));

        Assert.Equal("NIT inválido", ex.Message);
    }

    [Fact]
    public void NoDeberiaCrearProveedorSinNombre()
    {
        var proveedor = new Supplier("900123456", " ", "contacto@acme.com");

        var ex = Assert.Throws<InvalidSupplierException>(() => _servicio.CreateSupplier(proveedor));

        Assert.Equal("El nombre es obligatorio", ex.Message);
    }

    [Fact]
    public void DeberiaCrearProveedorConCorreoValido()
    {
        var repositorioEspia = new SpySupplierRepository();
        var servicio = new SupplierService(repositorioEspia, _ordenes);

        servicio.CreateSupplier(new Supplier("900123456", "ACME S.A.S.", "contacto@acme.com"));

        Assert.Equal(1, repositorioEspia.VecesQueSeLlamoAdd);
        Assert.NotNull(repositorioEspia.UltimoAgregado);
        Assert.Equal("ACTIVO", repositorioEspia.UltimoAgregado!.Estado);
    }

    [Fact]
    public void DeberiaDesactivarProveedorSinOrdenesActivas()
    {
        _servicio.CreateSupplier(new Supplier("900123456", "ACME", "contacto@acme.com"));

        var desactivado = _servicio.DeleteSupplier("900123456");

        Assert.Equal("INACTIVO", desactivado.Estado);
    }

    [Fact]
    public void NoDeberiaEliminarProveedorConOrdenesActivas()
    {
        _servicio.CreateSupplier(new Supplier("900123456", "ACME", "contacto@acme.com"));
        _ordenes.AgregarOrdenActiva("900123456");

        Assert.Throws<SupplierHasActiveOrdersException>(() => _servicio.DeleteSupplier("900123456"));

        Assert.Equal("ACTIVO", _repositorio.GetByNit("900123456")!.Estado);
    }
}
