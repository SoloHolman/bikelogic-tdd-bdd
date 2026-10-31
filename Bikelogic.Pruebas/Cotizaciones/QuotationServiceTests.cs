using Bikelogic.Negocio.Cotizaciones;
using Xunit;

namespace Bikelogic.Pruebas.Cotizaciones;

public class QuotationServiceTests
{
    private const string Solicitud = "SC-001";
    private const string Proveedor = "900123456";

    private readonly InMemoryQuotationRequestRepository _solicitudes = new();
    private readonly SpyQuotationRepository _cotizaciones = new();
    private readonly QuotationService _servicio;

    public QuotationServiceTests()
    {
        _solicitudes.Add(new QuotationRequest(Solicitud, "ABIERTA"));
        _servicio = new QuotationService(_solicitudes, _cotizaciones);
    }

    [Theory]
    [InlineData(2500000, false)]
    [InlineData(0, true)]
    [InlineData(-1000, true)]
    public void DeberiaValidarElMontoDeLaCotizacion(int monto, bool lanzaExcepcion)
    {
        if (lanzaExcepcion)
        {
            var ex = Assert.Throws<InvalidQuotationException>(
                () => _servicio.RegisterQuotation(Solicitud, Proveedor, monto));

            Assert.Equal("Monto inválido", ex.Message);
            Assert.Equal(0, _cotizaciones.SaveCount);
        }
        else
        {
            var cotizacion = _servicio.RegisterQuotation(Solicitud, Proveedor, monto);

            Assert.Equal((decimal)monto, cotizacion.Monto);
            Assert.Equal(1, _cotizaciones.SaveCount);
        }
    }

    [Fact]
    public void NoDeberiaRegistrarSiLaSolicitudNoEstaAbierta()
    {
        _solicitudes.Add(new QuotationRequest("SC-002", "CERRADA"));

        var ex = Assert.Throws<InvalidQuotationException>(
            () => _servicio.RegisterQuotation("SC-002", Proveedor, 2500000));

        Assert.Equal("La solicitud no está abierta", ex.Message);
        Assert.Equal(0, _cotizaciones.SaveCount);
    }

    [Fact]
    public void NoDeberiaRegistrarSiLaSolicitudNoExiste()
    {
        var ex = Assert.Throws<InvalidQuotationException>(
            () => _servicio.RegisterQuotation("SC-999", Proveedor, 2500000));

        Assert.Equal("La solicitud no existe", ex.Message);
    }

    [Fact]
    public void NoDeberiaRegistrarCotizacionDuplicadaDelMismoProveedor()
    {
        _servicio.RegisterQuotation(Solicitud, Proveedor, 2500000);

        var ex = Assert.Throws<InvalidQuotationException>(
            () => _servicio.RegisterQuotation(Solicitud, Proveedor, 3000000));

        Assert.Equal("El proveedor ya cotizó esta solicitud", ex.Message);
        Assert.Equal(1, _cotizaciones.SaveCount);
    }

    [Fact]
    public void DeberiaPermitirCotizacionesDeProveedoresDistintosParaLaMismaSolicitud()
    {
        _servicio.RegisterQuotation(Solicitud, "900123456", 2500000);
        _servicio.RegisterQuotation(Solicitud, "800987654", 2700000);

        Assert.Equal(2, _cotizaciones.SaveCount);
    }
}
