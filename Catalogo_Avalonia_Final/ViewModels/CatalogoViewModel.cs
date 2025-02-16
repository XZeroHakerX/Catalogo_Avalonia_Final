using System;
using System.ComponentModel;
using Avalonia.Media.Imaging;
using Avalonia.Collections;
using Avalonia.Platform;
using Catalogo_Avalonia_Final.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using DialogHostAvalonia;

namespace Catalogo_Avalonia_Final.ViewModels;

public partial class CatalogoViewModel : ObservableObject
{

    [ObservableProperty] public string? _nombre;
    [ObservableProperty] public string? _marca;
    [ObservableProperty] public string? _descripcion;
    [ObservableProperty] public double? _precio;
    [ObservableProperty] public int? _stock;
    [ObservableProperty] public string? _categoria;
    [ObservableProperty] public string? _otraInformacion;
    [ObservableProperty] public Bitmap? _foto;
    
    [ObservableProperty] public bool _panelAniadir;
    [ObservableProperty] public bool _panelVer;
    [ObservableProperty] public bool _panelEliminar;
    [ObservableProperty] public bool _panelInfo;
    [ObservableProperty] public bool _panelDonar;
    [ObservableProperty] public bool _panelAyuda;
    
    [ObservableProperty] public string _textoContador;
    [ObservableProperty] public bool _siguiente;
    [ObservableProperty] public bool _anterior;
    
    [ObservableProperty] public AvaloniaList<Producto>? _productos;
    [ObservableProperty] public AvaloniaList<String>? _productosLista;
    
    private int contadorLista = 0;
    
    private Bitmap? ImagenPorDefecto { get; } = new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")));
    

    public CatalogoViewModel()
    {
        LimpiarCampos();
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = false;
        PanelAniadir = false;
        PanelAyuda = false;
        TextoContador = "Producto 0 de 0:";
        IniciaListaProductos();
        ComprobarBotonesIndividual();
    }

    private void IniciaListaProductos()
    {
        Productos = new AvaloniaList<Producto>
        {
            new Producto("SPHERE 200Hms","HP","asdfasdf",12.0,1,"Video","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("BOOM BlastX120","Razer","asdfasdf",12.0,1,"Video","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("ROX R5440","BQ","asdfasdf",12.0,1,"Raton","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("BASILIK AX20","Microsoft","asdfasdf",12.0,1,"Teclado","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("SPHERE 200Hms","HP","asdfasdf",12.0,1,"Video","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("BOOM BlastX120","Razer","asdfasdf",12.0,1,"Video","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("ROX R5440","BQ","asdfasdf",12.0,1,"Raton","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("BASILIK AX20","Microsoft","asdfasdf",12.0,1,"Teclado","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("SPHERE 200Hms","HP","asdfasdf",12.0,1,"Video","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("BOOM BlastX120","Razer","asdfasdf",12.0,1,"Video","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("ROX R5440","BQ","asdfasdf",12.0,1,"Raton","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")))),
            new Producto("BASILIK AX20","Microsoft","asdfasdf",12.0,1,"Teclado","qwerqwer", new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png"))))
        };
        ProductosLista = new AvaloniaList<String>();
    }

    private void ProductosALista()
    {
        foreach (var produto in Productos)
        {
            StringBuilder productoTexto = new StringBuilder();
            productoTexto.Append("NOMBRE: " + produto.Nombre.PadRight(30).PadLeft(5));
            productoTexto.Append("-- MARCA: " + produto.Marca.PadRight(20).PadLeft(5));
            productoTexto.Append("-- CATEGORIA: " + produto.Categoria.PadRight(20).PadLeft(5));
            productoTexto.Append("-- STOCK: " + produto.Stock.ToString().PadRight(20).PadLeft(5));
            ProductosLista.Add(productoTexto.ToString());
        } 
    }

    private void ComprobarBotonesIndividual()
    {
        if (contadorLista == Productos.Count - 1)
        {
            Siguiente = false;
        }
        else
        {
            Siguiente = true;
        }

        if (contadorLista == 0)
        {
            Anterior = false;
        }
        else
        {
            Anterior = true;
        }
    }

    [RelayCommand]
    private async Task EliminarProductos()
    {
        if (Productos == null || Productos.Count == 0)
        {
            return; 
        }
        
        // Crear una lista temporal con los productos seleccionados
        var productosSeleccionados = Productos.Where(p => p.IsSelected).ToList();
        
        if (productosSeleccionados.Count == 0)
        {
            // Mostrar mensaje si no hay productos seleccionados
            await DialogHost.Show(
                new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Spacing = 10,
                    Children =
                    {
                        new TextBlock { Text = "No has seleccionado ningún producto para eliminar.", FontWeight = FontWeight.Bold },
                        new Button
                        {
                            Content = "OK",
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            Command = new RelayCommand(() =>
                            {
                                // Cerrar el diálogo
                                DialogHost.GetDialogSession("MainDialogHost")?.Close();
                            })
                        }
                    }
                },
                "MainDialogHost"
            );
            return;
        }
        // Mostrar diálogo de confirmación
        var result = await DialogHost.Show(
            new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = $"¿Estás seguro de que quieres eliminar {productosSeleccionados.Count} producto(s)?", FontWeight = FontWeight.Bold },
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 10,
                        Children =
                        {
                            new Button
                            {
                                Content = "Aceptar",
                                Background = new SolidColorBrush(Colors.OrangeRed),
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                                Command = new RelayCommand(() =>
                                {
                                    // Cerrar el diálogo con "Yes" como resultado
                                    DialogHost.GetDialogSession("MainDialogHost")?.Close("Yes");
                                })
                            },
                            new Button
                            {
                                Content = "Cancelar",
                                Background = new SolidColorBrush(Colors.CadetBlue),
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                                Command = new RelayCommand(() =>
                                {
                                    // Cerrar el diálogo con "No" como resultado
                                    DialogHost.GetDialogSession("MainDialogHost")?.Close("No");
                                })
                            }
                        }
                    }
                }
            },
            "MainDialogHost"
        );

        // Proceder solo si el usuario hace clic en "Aceptar"
        if (result.ToString() != "Yes")
        {
            return;
        }
        
        // Eliminar los productos seleccionados
        foreach (var producto in productosSeleccionados)
        {
            Productos.Remove(producto);
        }
        
        // Actualizar la lista de productos en formato texto
        ProductosLista.Clear();
        ProductosALista();
        
        // Reiniciar el contador de lista si es necesario
        if (contadorLista >= Productos.Count)
        {
            contadorLista = Math.Max(0, Productos.Count - 1);
        }
        
    }
    
    [RelayCommand]
    private void ActivarPanelVer()
    {
        PanelAniadir = false;
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = true;
        PanelAyuda = false;
        contadorLista = 0;
        ProductosALista();
        ComprobarBotonesIndividual();
        IniciarCampos();
    }
    
    [RelayCommand]
    private void ActivarPanelAniadir()
    {
        PanelAniadir = true;
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = false;
        PanelAyuda = false;
        LimpiarCampos();
        Foto = ImagenPorDefecto;
    }
    
    [RelayCommand]
    private void ActivarPanelEliminar()
    {
        PanelAniadir = false;
        PanelDonar = false;
        PanelEliminar = true;
        PanelInfo = false;
        PanelVer = false;
        PanelAyuda = false;
        ProductosALista();
        IniciarCampos();
    }
    
    [RelayCommand]
    private void ActivarPanelInfo()
    {
        PanelAniadir = false;
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = true;
        PanelVer = false;
        PanelAyuda = false;
        LimpiarCampos();
        Foto = ImagenPorDefecto;
    }
    
    [RelayCommand]
    private void ActivarPanelDonar()
    {
        PanelAniadir = false;
        PanelDonar = true;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = false;
        PanelAyuda = false;
        LimpiarCampos();
        Foto = ImagenPorDefecto;
    }
    
    [RelayCommand]
    private void ActivarPanelAyuda()
    {
        PanelAniadir = false;
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = false;
        PanelAyuda = true;
        LimpiarCampos();
        Foto = ImagenPorDefecto;
    }

    [RelayCommand]
    private void SiguienteProducto()
    {
        contadorLista++;

        ComprobarBotonesIndividual();
        
        if (contadorLista < Productos.Count && Productos[contadorLista] != null)
        {
            IniciarCampos();
        }
        

    }
    
    [RelayCommand]
    private void AnteriorProducto()
    {
        
        contadorLista--;
        ComprobarBotonesIndividual();
        if (contadorLista >= 0 && Productos[contadorLista] != null)
        {
            IniciarCampos();    
        }
        
    }
    
    [RelayCommand]
    private void UltimoProducto()
    {
        
        contadorLista = Productos.Count - 1;
        ComprobarBotonesIndividual();
        if ( Productos[contadorLista] != null){
            IniciarCampos();    
        }
        
    }
    
    [RelayCommand]
    private void PrimerProducto()
    {
        
        contadorLista = 0;
        ComprobarBotonesIndividual();
        if ( Productos[contadorLista] != null){
            IniciarCampos();    
        }
        
    }

    private void LimpiarCampos()
    {
        Nombre = string.Empty;
        Marca = string.Empty;
        Descripcion = string.Empty;
        Precio = 0.0;
        Stock = 0;
        Categoria = string.Empty;
        OtraInformacion = string.Empty;
        Foto = ImagenPorDefecto;
        TextoContador = "Producto 0 de 0:";
    }

    private void IniciarCampos()
    {
        if (Productos != null && Productos.Count > 0 && contadorLista >= 0 && contadorLista < Productos.Count)
        { 
            TextoContador = "Producto " + (contadorLista + 1) + " de " + Productos.Count + ":";
            Nombre = Productos[contadorLista].Nombre;
            Marca = Productos[contadorLista].Marca;
            Descripcion = Productos[contadorLista].Descripcion;
            Precio = Productos[contadorLista].Precio; 
            Stock = Productos[contadorLista].Stock; 
            Categoria = Productos[contadorLista].Categoria; 
            OtraInformacion = Productos[contadorLista].OtraInformacion; 
            Foto = Productos[contadorLista].Foto;
        }
        else
        {
            LimpiarCampos();
        }
        
    }

    
    
    
    
    // Implementación de INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}