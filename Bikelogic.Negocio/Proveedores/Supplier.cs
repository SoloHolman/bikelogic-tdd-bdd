namespace Bikelogic.Negocio.Proveedores;

public class Supplier
{
    public Supplier(string nit, string nombre, string correo)
    {
        Nit = nit;
        Nombre = nombre;
        Correo = correo;
    }

    public int Id { get; set; }
    public string Nit { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Estado { get; set; } = string.Empty;
}
