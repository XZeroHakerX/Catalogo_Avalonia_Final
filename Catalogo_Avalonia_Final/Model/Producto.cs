using System.IO;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Catalogo_Avalonia_Final.Model;

public class Producto
{
    
    // Atributos de la clase productos necesarios:
    public string Nombre {get; set;}
    public string Marca {get; set;}
    public string Descripcion {get; set;}
    public double Precio {get; set;}
    public int Stock {get; set;}
    public string Categoria {get; set;}
    public string OtraInformacion {get; set;}
    
    // Tenemos la foto en array de byte para la serializacion:
    private byte[] fotoBytes;

    // Aqui la variable con la que se accede o se cambia la foto
    public Bitmap Foto
    {
        get => fotoBytes != null ? new Bitmap(new MemoryStream(fotoBytes)) : null;
        set
        {
            if (value != null)
            {
                using (var stream = new MemoryStream())
                {
                    value.Save(stream);
                    fotoBytes = stream.ToArray();
                }
            }
            else
            {
                fotoBytes = null;
            }
        }
    }
    
    // Boolean para el panel de eliminacion:
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
    
    // Método para serializar un producto
    public void Serializar(BinaryWriter writer)
    {
        writer.Write(Nombre);
        writer.Write(Marca);
        writer.Write(Descripcion);
        writer.Write(Precio);
        writer.Write(Stock);
        writer.Write(Categoria);
        writer.Write(OtraInformacion);
        writer.Write(IsSelected);

        // Serializar la foto 
        if (fotoBytes != null)
        {
            writer.Write(fotoBytes.Length); // Escribir la longitud del array de bytes
            writer.Write(fotoBytes);       // Escribir los bytes de la imagen
        }
        else
        {
            writer.Write(0); // Indicar que no hay imagen
        }
    }

    // Método para deserializar un producto
    public static Producto Deserializar(BinaryReader reader)
    {
        string nombre = reader.ReadString();
        string marca = reader.ReadString();
        string descripcion = reader.ReadString();
        double precio = reader.ReadDouble();
        int stock = reader.ReadInt32();
        string categoria = reader.ReadString();
        string otraInformacion = reader.ReadString();
        bool isSelected = reader.ReadBoolean();

        // Leer la foto 
        int fotoLength = reader.ReadInt32();
        Bitmap foto = null;
        if (fotoLength > 0)
        {
            byte[] fotoBytes = reader.ReadBytes(fotoLength);
            foto = new Bitmap(new MemoryStream(fotoBytes));
        }

        return new Producto(nombre, marca, descripcion, precio, stock, categoria, otraInformacion, foto)
        {
            IsSelected = false
        };
    }
}