using Bikelogic.Negocio.Inventario;
using Reqnroll;
using Xunit;

namespace Bikelogic.Pruebas.Inventario;

[Binding]
[Scope(Feature = "Inventario de productos, materia prima e insumos")]
public class InventarioProductosSteps
{
    private readonly InventoryService _service = new();

    private string _codigo = string.Empty;
    private string? _respuesta;
    private Exception? _excepcion;

    // ---------- Dado ----------

    [Given(@"que el producto ""([^""]*)"" tiene stock (\d+)")]
    public void DadoQueElProductoTieneStock(string codigo, int stock)
    {
        _service.RegisterProduct(new Product(codigo, "Repuestos", stock));
        _codigo = codigo;
    }

    [Given(@"que ya existe el producto ""([^""]*)"" con stock (\d+)")]
    public void DadoQueYaExisteElProducto(string codigo, int stock)
    {
        _service.RegisterProduct(new Product(codigo, "Repuestos", stock));
    }

    // ---------- Cuando ----------

    [When(@"se retiran (\d+) unidades")]
    public void CuandoSeRetiranUnidades(int cantidad)
    {
        _respuesta = _service.WithdrawStock(_codigo, cantidad);
    }

    [When(@"registro el producto ""([^""]*)"" de la categoría ""([^""]*)"" con stock inicial (-?\d+)")]
    [When(@"intento registrar el producto ""([^""]*)"" de la categoría ""([^""]*)"" con stock inicial (-?\d+)")]
    public void CuandoRegistroElProducto(string codigo, string categoria, int stock)
    {
        IntentarRegistrar(codigo, categoria, stock);
    }

    [When(@"intento registrar otro producto con código ""([^""]*)""")]
    public void CuandoIntentoRegistrarOtroProducto(string codigo)
    {
        IntentarRegistrar(codigo, "Lubricantes", 5);
    }

    // ---------- Entonces ----------

    [Then(@"el stock final es (\d+)")]
    public void EntoncesElStockFinalEs(int esperado)
    {
        Assert.Equal(esperado, _service.GetProduct(_codigo).Stock);
    }

    [Then(@"el sistema responde ""([^""]*)""")]
    public void EntoncesElSistemaResponde(string mensaje)
    {
        Assert.Equal(mensaje, _respuesta);
    }

    [Then(@"el producto ""([^""]*)"" queda registrado con stock (\d+)")]
    public void EntoncesElProductoQuedaRegistrado(string codigo, int stock)
    {
        Assert.Null(_excepcion);
        Assert.Equal(stock, _service.GetProduct(codigo).Stock);
    }

    [Then(@"el sistema rechaza el registro")]
    public void EntoncesElSistemaRechazaElRegistro()
    {
        Assert.NotNull(_excepcion);
    }

    [Then(@"el stock del producto ""([^""]*)"" permanece en (\d+)")]
    public void EntoncesElStockPermaneceEn(string codigo, int esperado)
    {
        Assert.Equal(esperado, _service.GetProduct(codigo).Stock);
    }

    // ---------- Apoyo ----------

    private void IntentarRegistrar(string codigo, string categoria, int stock)
    {
        try
        {
            _service.RegisterProduct(new Product(codigo, categoria, stock));
        }
        catch (Exception e)
        {
            _excepcion = e;
        }
    }
}
