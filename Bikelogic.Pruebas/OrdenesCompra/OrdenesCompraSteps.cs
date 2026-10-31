using System.Globalization;
using Bikelogic.Negocio.OrdenesCompra;
using Reqnroll;
using Xunit;

namespace Bikelogic.Pruebas.OrdenesCompra;

[Binding]
[Scope(Feature = "Aprobación de órdenes de compra")]
public class OrdenesCompraSteps
{
    private readonly PurchaseOrderService _service = new();

    private PurchaseOrder? _orden;
    private string _proveedor = string.Empty;
    private string _numeroOrden = string.Empty;
    private Exception? _excepcion;

    // ---------- Dado ----------

    [Given(@"que el proveedor ""([^""]*)"" tiene presupuesto disponible de \$([\d\.]+)")]
    public void DadoQueElProveedorTienePresupuestoDisponible(string proveedor, string monto)
    {
        _proveedor = proveedor;
        _service.AddSupplier(proveedor, ConvertirMonto(monto));
    }

    [Given(@"existe una orden ""([^""]*)"" por \$([\d\.]+) en estado ""([^""]*)""")]
    public void DadoQueExisteUnaOrden(string numero, string monto, string estado)
    {
        _orden = CrearOrden(numero, _proveedor, ConvertirMonto(monto));
        _numeroOrden = numero;
        _service.AddOrder(_orden);

        if (estado == "APROBADA")
        {
            _service.ApproveOrder(numero);
        }
    }

    // ---------- Cuando ----------

    [When(@"el jefe de compras aprueba la orden")]
    public void CuandoElJefeDeComprasApruebaLaOrden()
    {
        try
        {
            _service.ApproveOrder(_numeroOrden);
        }
        catch (Exception e)
        {
            _excepcion = e;
        }
    }

    // ---------- Entonces ----------

    [Then(@"la orden cambia a ""([^""]*)""")]
    public void EntoncesLaOrdenCambiaA(string estado)
    {
        Assert.Null(_excepcion);
        Assert.NotNull(_orden);
        Assert.Equal(estado, _orden!.Status);
    }

    [Then(@"la orden permanece en estado ""([^""]*)""")]
    public void EntoncesLaOrdenPermaneceEnEstado(string estado)
    {
        Assert.NotNull(_orden);
        Assert.Equal(estado, _orden!.Status);
    }

    [Then(@"el presupuesto del proveedor (?:se reduce a|permanece en) \$([\d\.]+)")]
    public void EntoncesElPresupuestoDelProveedorEs(string monto)
    {
        Assert.Equal(ConvertirMonto(monto), _service.GetBudget(_proveedor));
    }

    [Then(@"el sistema muestra el mensaje ""([^""]*)""")]
    public void EntoncesElSistemaMuestraElMensaje(string mensaje)
    {
        Assert.NotNull(_excepcion);
        // Se ignora el punto final para que funcione con "Presupuesto insuficiente" y "Presupuesto insuficiente."
        Assert.Equal(mensaje, _excepcion!.Message.TrimEnd('.'));
    }

    // ---------- Apoyo ----------

    // SUPUESTO: el constructor es PurchaseOrder(numero, proveedor, monto).
    // Si el tuyo es distinto, cambia SOLO esta línea.
    private static PurchaseOrder CrearOrden(string numero, string proveedor, decimal monto) =>
        new PurchaseOrder(numero, proveedor, monto);

    // "10.000.000" -> 10000000
    private static decimal ConvertirMonto(string monto) =>
        decimal.Parse(monto.Replace(".", string.Empty), CultureInfo.InvariantCulture);
}
