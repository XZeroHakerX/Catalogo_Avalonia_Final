namespace Catalogo_Avalonia_Final.Model;

public class Producto
{
    public string Nombre {get; set;}
    public string Marca {get; set;}
    public string Descripcion {get; set;}
    public double Precio {get; set;}
    public int Stock {get; set;}
    public string Categoria {get; set;}
    public string OtraInformacion {get; set;}
    public string Foto {get; set;}
    
    
    // Constructor: (Inicializacion de las propiedades)
    public Producto(string nombre, string marca, double precio)
    {
        Nombre = nombre;
        Marca = marca;
        Descripcion = "N/S";
        Precio = precio;
        Stock = 0;
        Categoria = "N/S";
        OtraInformacion = "N/S";
        Foto = "";
    }
    
    public Producto(string nombre, string marca, 
        string descripcion, double precio, int stock, 
        string categoria, string otraInformacion, 
        string foto)
    {
        Nombre = nombre;
        Marca = marca;
        Descripcion = descripcion;
        Precio = precio;
        Stock = stock;
        Categoria = categoria;
        OtraInformacion = otraInformacion;
        Foto = foto;
    }
}