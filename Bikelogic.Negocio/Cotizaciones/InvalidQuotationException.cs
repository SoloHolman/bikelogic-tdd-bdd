namespace Bikelogic.Negocio.Cotizaciones;

public class InvalidQuotationException : Exception
{
    public InvalidQuotationException(string mensaje) : base(mensaje)
    {
    }
}
