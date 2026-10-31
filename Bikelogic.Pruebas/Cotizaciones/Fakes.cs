using Bikelogic.Negocio.Cotizaciones;

namespace Bikelogic.Pruebas.Cotizaciones;

public class SpyQuotationRepository : IQuotationRepository
{
    private readonly InMemoryQuotationRepository _interno = new();

    public int SaveCount { get; private set; }

    public bool ExistsByRequestAndSupplier(string codigoSolicitud, string nitProveedor) =>
        _interno.ExistsByRequestAndSupplier(codigoSolicitud, nitProveedor);

    public void Save(Quotation cotizacion)
    {
        SaveCount++;
        _interno.Save(cotizacion);
    }
}
