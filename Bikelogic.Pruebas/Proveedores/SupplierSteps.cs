using Bikelogic.Negocio.Proveedores;
using Reqnroll;
using Xunit;

namespace Bikelogic.Pruebas.Proveedores;

[Binding]
public class SupplierSteps
{
    private const string NitPorDefecto = "900123456";

    private readonly InMemorySupplierRepository _repositorio = new();
    private readonly FakePurchaseOrderRepository _ordenes = new();
    private readonly SupplierService _servicio;

    private Supplier? _proveedor;
    private Exception? _excepcion;
    private string? _mensaje;
    private string _nitExistente = NitPorDefecto;
    private bool _sesionIniciada;

    public SupplierSteps()
    {
        _servicio = new SupplierService(_repositorio, _ordenes);
    }

    // ---------- Dado ----------

    [Given(@"que el usuario administrador ha iniciado sesión")]
    public void DadoQueElUsuarioAdministradorHaIniciadoSesion()
    {
        // La autenticación real se probará en el módulo de usuarios.
        _sesionIniciada = true;
    }

    [Given(@"que no existe un proveedor con NIT ""([^""]*)""")]
    public void DadoQueNoExisteUnProveedorConNit(string nit)
    {
        Assert.False(_repositorio.ExistsByNit(nit));
    }

    [Given(@"que ya existe un proveedor con NIT ""([^""]*)""")]
    public void DadoQueYaExisteUnProveedorConNit(string nit)
    {
        _servicio.CreateSupplier(new Supplier(nit, "ACME S.A.S.", "contacto@acme.com"));
        _nitExistente = nit;
    }

    [Given(@"que existe un proveedor ""([^""]*)"" sin órdenes de compra activas")]
    public void DadoQueExisteUnProveedorSinOrdenesActivas(string nombre)
    {
        _servicio.CreateSupplier(new Supplier(NitPorDefecto, nombre, "contacto@acme.com"));
    }

    [Given(@"que existe un proveedor ""([^""]*)"" con órdenes de compra activas")]
    public void DadoQueExisteUnProveedorConOrdenesActivas(string nombre)
    {
        _servicio.CreateSupplier(new Supplier(NitPorDefecto, nombre, "contacto@acme.com"));
        _ordenes.AgregarOrdenActiva(NitPorDefecto);
    }

    // ---------- Cuando ----------

    [When(@"registro el proveedor con NIT ""([^""]*)"", nombre ""([^""]*)"" y correo ""([^""]*)""")]
    public void CuandoRegistroElProveedor(string nit, string nombre, string correo)
    {
        Registrar(new Supplier(nit, nombre, correo));
    }

    [When(@"intento registrar otro proveedor con el mismo NIT")]
    public void CuandoIntentoRegistrarOtroProveedorConElMismoNit()
    {
        Registrar(new Supplier(_nitExistente, "Otra empresa", "otra@empresa.com"));
    }

    [When(@"registro un proveedor con correo ""([^""]*)""")]
    public void CuandoRegistroUnProveedorConCorreo(string correo)
    {
        Registrar(new Supplier("900999999", "Proveedor de prueba", correo));
    }

    [When(@"solicito eliminar el proveedor")]
    public void CuandoSolicitoEliminarElProveedor()
    {
        Assert.True(_sesionIniciada);
        try
        {
            _servicio.DeleteSupplier(NitPorDefecto);
            _mensaje = SupplierService.MensajeDesactivado;
        }
        catch (Exception e)
        {
            _excepcion = e;
            _mensaje = e.Message;
        }
    }

    // ---------- Entonces ----------

    [Then(@"el proveedor se crea con estado ""([^""]*)""")]
    public void EntoncesElProveedorSeCreaConEstado(string estado)
    {
        Assert.NotNull(_proveedor);
        Assert.Equal(estado, _proveedor!.Estado);
    }

    [Then(@"el proveedor (?:cambia a|permanece en) estado ""([^""]*)""")]
    public void EntoncesElProveedorTieneEstado(string estado)
    {
        var guardado = _repositorio.GetByNit(NitPorDefecto);
        Assert.NotNull(guardado);
        Assert.Equal(estado, guardado!.Estado);
    }

    [Then(@"el sistema muestra el mensaje ""([^""]*)""")]
    [Then(@"el sistema muestra ""([^""]*)""")]
    [Then(@"el sistema confirma ""([^""]*)""")]
    public void EntoncesElSistemaMuestraElMensaje(string mensaje)
    {
        Assert.Equal(mensaje, _mensaje);
    }

    [Then(@"el sistema responde ""([^""]*)""")]
    public void EntoncesElSistemaResponde(string resultado)
    {
        var real = _excepcion is null ? "Correo válido" : _excepcion.Message;
        Assert.Equal(resultado, real);
    }

    [Then(@"no se crea un nuevo proveedor")]
    public void EntoncesNoSeCreaUnNuevoProveedor()
    {
        Assert.Equal(1, _repositorio.Count);
    }

    // ---------- Apoyo ----------

    private void Registrar(Supplier proveedor)
    {
        Assert.True(_sesionIniciada);
        try
        {
            _proveedor = _servicio.CreateSupplier(proveedor);
            _mensaje = SupplierService.MensajeRegistrado;
        }
        catch (Exception e)
        {
            _excepcion = e;
            _mensaje = e.Message;
        }
    }
}
