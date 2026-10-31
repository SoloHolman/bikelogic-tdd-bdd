namespace Bikelogic.Negocio.Cotizaciones;

public class QuotationService
{
    public const string MensajeRegistrada = "Cotización registrada";

    private readonly IQuotationRequestRepository _solicitudes;
    private readonly IQuotationRepository _cotizaciones;

    public QuotationService(IQuotationRequestRepository solicitudes, IQuotationRepository cotizaciones)
    {
        _solicitudes = solicitudes;
        _cotizaciones = cotizaciones;
    }

    public Quotation RegisterQuotation(string codigoSolicitud, string nitProveedor, decimal monto)
    {
        var solicitud = _solicitudes.GetByCode(codigoSolicitud)
            ?? throw new InvalidQuotationException("La solicitud no existe");

        if (solicitud.Estado != "ABIERTA")
            throw new InvalidQuotationException("La solicitud no está abierta");

        if (monto <= 0)
            throw new InvalidQuotationException("Monto inválido");

        if (_cotizaciones.ExistsByRequestAndSupplier(codigoSolicitud, nitProveedor))
            throw new InvalidQuotationException("El proveedor ya cotizó esta solicitud");

        var cotizacion = new Quotation(codigoSolicitud, nitProveedor, monto);
        _cotizaciones.Save(cotizacion);
        return cotizacion;
    }
}
