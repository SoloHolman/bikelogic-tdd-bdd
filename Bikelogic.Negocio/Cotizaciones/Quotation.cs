namespace Bikelogic.Negocio.Cotizaciones;

public class Quotation
{
    public Quotation(string codigoSolicitud, string nitProveedor, decimal monto)
    {
        CodigoSolicitud = codigoSolicitud;
        NitProveedor = nitProveedor;
        Monto = monto;
    }

    public int Id { get; set; }
    public string CodigoSolicitud { get; set; }
    public string NitProveedor { get; set; }
    public decimal Monto { get; set; }
}
