using Avalonia.Media.Imaging;

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
    public Bitmap Foto {get; set;}
    
    public bool IsSelected {get; set;}
  
    
    // Constructor: (Inicializacion de las propiedades)
    public Producto(string nombre, string marca, 
        string descripcion, double precio, int stock, 
        string categoria, string otraInformacion, 
        Bitmap foto)
    {
        Nombre = nombre;
        Marca = marca;
        Descripcion = descripcion;
        Precio = precio;
        Stock = stock;
        Categoria = categoria;
        OtraInformacion = otraInformacion;
        Foto = foto;
        IsSelected = false;
    }
}