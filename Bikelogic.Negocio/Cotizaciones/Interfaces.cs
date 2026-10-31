namespace Bikelogic.Negocio.Cotizaciones;

public interface IQuotationRequestRepository
{
    QuotationRequest? GetByCode(string codigo);
}

public interface IQuotationRepository
{
    bool ExistsByRequestAndSupplier(string codigoSolicitud, string nitProveedor);
    void Save(Quotation cotizacion);
}
