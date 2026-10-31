using Bikelogic.Negocio.Inventario;
using Xunit;

namespace Bikelogic.Pruebas.Inventario;

public class InventoryServiceTests
{
    private readonly InventoryService _service = new();

    // ---------- Registro de productos ----------

    [Fact]
    public void RegistrarProducto_CodigoUnico_RegistroExitoso()
    {
        _service.RegisterProduct(new Product("PROD-001", "Repuestos", 10));

        var producto = _service.GetProduct("PROD-001");
        Assert.Equal("PROD-001", producto.Code);
        Assert.Equal(10, producto.Stock);
    }

    [Fact]
    public void RegistrarProducto_CodigoDuplicado_LanzaExcepcionYConservaElProductoOriginal()
    {
        _service.RegisterProduct(new Product("PROD-001", "Repuestos", 10));

        Assert.Throws<InvalidOperationException>(() =>
            _service.RegisterProduct(new Product("PROD-001", "Lubricantes", 5)));

        Assert.Equal(10, _service.GetProduct("PROD-001").Stock);
    }

    [Fact]
    public void RegistrarProducto_StockInicialCero_RegistroExitoso()
    {
        _service.RegisterProduct(new Product("PROD-004", "Insumos", 0));

        Assert.Equal(0, _service.GetProduct("PROD-004").Stock);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void CrearProducto_StockInicialNegativo_LanzaExcepcion(int stockInicial)
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("PROD-002", "Repuestos", stockInicial));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void CrearProducto_CategoriaVaciaOEnBlanco_LanzaExcepcion(string categoria)
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("PROD-003", categoria, 5));
    }

    // ---------- Retiro de inventario (esquema del Taller 4) ----------

    [Theory]
    [InlineData("PROD-001", 10, 3, 7, "Retiro exitoso")]
    [InlineData("PROD-002", 5, 5, 0, "Retiro exitoso")]
    [InlineData("PROD-003", 2, 5, 2, "Stock insuficiente")]
    public void RetirarStock_SegunEsquemaDelTaller_ActualizaElStockYResponde(
        string codigo, int stockInicial, int retiro, int stockFinal, string mensaje)
    {
        _service.RegisterProduct(new Product(codigo, "Repuestos", stockInicial));

        var resultado = _service.WithdrawStock(codigo, retiro);

        Assert.Equal(mensaje, resultado);
        Assert.Equal(stockFinal, _service.GetProduct(codigo).Stock);
    }
}
