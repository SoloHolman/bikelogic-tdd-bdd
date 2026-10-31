namespace Bikelogic.Negocio.Cotizaciones;

public class QuotationRequest
{
    public QuotationRequest(string codigo, string estado)
    {
        Codigo = codigo;
        Estado = estado;
    }

    public string Codigo { get; set; }
    public string Estado { get; set; }
}
