using Bikelogic.Negocio.Cotizaciones;
using Reqnroll;
using Xunit;

namespace Bikelogic.Pruebas.Cotizaciones;

[Binding]
[Scope(Feature = "Solicitud de cotizaciones")]
public class QuotationSteps
{
    private const string ProveedorPorDefecto = "900123456";

    private readonly InMemoryQuotationRequestRepository _solicitudes = new();
    private readonly InMemoryQuotationRepository _cotizaciones = new();
    private readonly QuotationService _servicio;

    private string _codigoSolicitud = string.Empty;
    private string? _mensaje;

    public QuotationSteps()
    {
        _servicio = new QuotationService(_solicitudes, _cotizaciones);
    }

    [Given(@"que existe una solicitud ""([^""]*)"" en estado ""([^""]*)""")]
    public void DadoQueExisteUnaSolicitudEnEstado(string codigo, string estado)
    {
        _solicitudes.Add(new QuotationRequest(codigo, estado));
        _codigoSolicitud = codigo;
    }

    [Given(@"el proveedor ""([^""]*)"" ya cotizó la solicitud ""([^""]*)""")]
    public void DadoQueElProveedorYaCotizoLaSolicitud(string nit, string codigoSolicitud)
    {
        _servicio.RegisterQuotation(codigoSolicitud, nit, 1000000);
    }

    [When(@"registro una cotización por (-?\d+)")]
    public void CuandoRegistroUnaCotizacionPor(int monto)
    {
        Registrar(_codigoSolicitud, ProveedorPorDefecto, monto);
    }

    [When(@"el proveedor ""([^""]*)"" registra otra cotización por (-?\d+) para la solicitud ""([^""]*)""")]
    public void CuandoElProveedorRegistraOtraCotizacion(string nit, int monto, string codigoSolicitud)
    {
        Registrar(codigoSolicitud, nit, monto);
    }

    [Then(@"el sistema responde ""([^""]*)""")]
    public void EntoncesElSistemaResponde(string resultado)
    {
        Assert.Equal(resultado, _mensaje);
    }

    private void Registrar(string codigoSolicitud, string nit, decimal monto)
    {
        try
        {
            _servicio.RegisterQuotation(codigoSolicitud, nit, monto);
            _mensaje = QuotationService.MensajeRegistrada;
        }
        catch (InvalidQuotationException e)
        {
            _mensaje = e.Message;
        }
    }
}
