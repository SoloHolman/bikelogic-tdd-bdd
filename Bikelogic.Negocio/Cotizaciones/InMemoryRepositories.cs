namespace Bikelogic.Negocio.Cotizaciones;

public class InMemoryQuotationRequestRepository : IQuotationRequestRepository
{
    private readonly List<QuotationRequest> _solicitudes = new();

    public void Add(QuotationRequest solicitud) => _solicitudes.Add(solicitud);

    public QuotationRequest? GetByCode(string codigo) =>
        _solicitudes.FirstOrDefault(s => s.Codigo == codigo);
}

public class InMemoryQuotationRepository : IQuotationRepository
{
    private readonly List<Quotation> _cotizaciones = new();
    private int _siguienteId = 1;

    public int Count => _cotizaciones.Count;

    public bool ExistsByRequestAndSupplier(string codigoSolicitud, string nitProveedor) =>
        _cotizaciones.Any(c => c.CodigoSolicitud == codigoSolicitud && c.NitProveedor == nitProveedor);

    public void Save(Quotation cotizacion)
    {
        cotizacion.Id = _siguienteId++;
        _cotizaciones.Add(cotizacion);
    }
}
