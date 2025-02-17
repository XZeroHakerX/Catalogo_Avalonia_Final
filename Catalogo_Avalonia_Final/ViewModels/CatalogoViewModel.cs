using System;
using System.ComponentModel;
using Avalonia.Media.Imaging;
using Avalonia.Collections;
using Avalonia.Platform;
using Catalogo_Avalonia_Final.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using DialogHostAvalonia;

namespace Catalogo_Avalonia_Final.ViewModels;

public partial class CatalogoViewModel : ObservableObject
{

    // Declaracion de propiedades del objeto Producto:
    [ObservableProperty] public string? _nombre;
    [ObservableProperty] public string? _marca;
    [ObservableProperty] public string? _descripcion;
    [ObservableProperty] public double? _precio;
    [ObservableProperty] public int? _stock;
    [ObservableProperty] public string? _categoria;
    [ObservableProperty] public string? _otraInformacion;
    [ObservableProperty] public Bitmap? _foto;

    // Declaracion de propiedades de activacion de paneles:
    [ObservableProperty] public bool _panelInicio;
    [ObservableProperty] public bool _panelAniadir;
    [ObservableProperty] public bool _panelVer;
    [ObservableProperty] public bool _panelEliminar;
    [ObservableProperty] public bool _panelInfo;
    [ObservableProperty] public bool _panelDonar;
    [ObservableProperty] public bool _panelAyuda;
    [ObservableProperty] public bool _activarPanelDonacionUno;
    [ObservableProperty] public bool _activarPanelDonacionDos;
    [ObservableProperty] public bool _activarPanelDonacionTres;
        
    // Declaracion del panel donacion:
    [ObservableProperty] public double _cantidadDonacion;
    [ObservableProperty] public Bitmap _imagenDonacion;
    [ObservableProperty] public Bitmap _imagenEmpresa;
    [ObservableProperty] public Bitmap _imagenEmpresaFondo;
    
    // Declaracion de las variables para los botones:
    [ObservableProperty] public bool _siguiente;
    [ObservableProperty] public bool _anterior;
    
    // Declaracion de las listas y el texto de contador de las mismas:
    [ObservableProperty] public string _textoContador;
    [ObservableProperty] public AvaloniaList<Producto>? _productos;
    [ObservableProperty] public AvaloniaList<String>? _productosLista;
    
    // Comando para cerrar la aplicación
    public ReactiveCommand<Unit, Unit> ExitApplicationCommand { get; }
    
    // Declaracion e inicializacion de las imagenes y contador:
    private int contadorLista = 0;
    private Bitmap? ImagenPorDefecto { get; } = new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/avatar.png")));
    private Bitmap? ImagenCaraUno { get; } = new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/cara1.png")));
    private Bitmap? ImagenCaraDos { get; } = new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/cara2.png")));
    private Bitmap? ImagenCaraTres { get; } = new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/cara3.png")));
   
    

    // Constructor:
    public CatalogoViewModel()
    {
        LimpiarCampos();
        PanelInicio = true;
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = false;
        PanelAniadir = false;
        PanelAyuda = false;
        ActivarPanelDonacionUno = false;
        ActivarPanelDonacionDos = false;
        ActivarPanelDonacionTres = false;
        ImagenDonacion = null;
        ImagenEmpresa = new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/empresa.png")));
        ImagenEmpresaFondo = new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/empresa25.png")));
        CantidadDonacion = 0.0;
        TextoContador = "Producto 0 de 0:";
        IniciaListaProductos();
        ComprobarBotonesIndividual();
        
        // Define el comando para cerrar la aplicación
        ExitApplicationCommand = ReactiveCommand.Create(() =>
        {
            // Cierra la aplicación
            Environment.Exit(0);
        });
    }

    // Metodo que inicia las listas y los campos:
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
    
    // Metodo para pasar la lista de productos a una lista de string:
    private void ProductosALista()
    {
        ProductosLista.Clear();
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

    // Metodos para la comprobacion necesaria de botones e imagenes:
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

    private void ComprobarImagenDonacion()
    {
        if (CantidadDonacion == 0.0)
        {
            ImagenDonacion = null;
            ActivarPanelDonacionUno = false;
            ActivarPanelDonacionDos = false;
            ActivarPanelDonacionTres = false;
        }
        if (CantidadDonacion > 0.0 && CantidadDonacion <= 49.0)
        {
            ImagenDonacion = ImagenCaraUno;
            ActivarPanelDonacionUno = true;
        }
        if(CantidadDonacion >= 50.0 && CantidadDonacion <= 99.0)
        {
            ImagenDonacion = ImagenCaraDos;
            ActivarPanelDonacionDos = true;
        }

        if (CantidadDonacion >= 100.0)
        {
            ImagenDonacion = ImagenCaraTres;
            ActivarPanelDonacionTres = true;
        }
    }
    
    // Metodos para limpiar los componentes:
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
    
    private void LimpiarPanelesDonacion()
    {
        ActivarPanelDonacionUno = false;
        ActivarPanelDonacionDos = false;
        ActivarPanelDonacionTres = false;
    }
    
    
    // Metodos para la pantalla de donacion:
    [RelayCommand]
    private void DonarCinco()
    {
        CantidadDonacion += 5.0;
        ComprobarImagenDonacion();
    }
    
    [RelayCommand]
    private void DonarDiez()
    {
        CantidadDonacion += 10.0;
        ComprobarImagenDonacion();
    }
    
    [RelayCommand]
    private void DonarVeinte()
    {
        CantidadDonacion += 20.0;
        ComprobarImagenDonacion();
    }
    
    [RelayCommand]
    private async Task BotonDonar()
    {
        CantidadDonacion = 0.0;
        ComprobarImagenDonacion();
        
        // Mostrar mensaje cuando haga la donacion:
        await DialogHost.Show(
            new StackPanel
            {
                Orientation = Orientation.Vertical,
                Width = 380,
                Height = 100,
                VerticalAlignment =  VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Spacing = 10,
                Children =
                {
                    new TextBlock { Text = "GRACIAS por tu Apoyo!", FontSize = 25,FontWeight = FontWeight.Bold, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center },
                    new Button
                    {
                        Content = "Te queremos!",
                        Height = 60,
                        Width = 200,
                        FontSize = 25,
                        Foreground = new SolidColorBrush(Colors.Azure),
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
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
    
    
    // Metodo para gestion de los productos:
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
                    Background = new SolidColorBrush(Color.FromArgb(240,250,100,100)),
                    Children =
                    {
                        new TextBlock { Text = "No has seleccionado ningún producto para eliminar.",Foreground = new SolidColorBrush(Colors.Azure), FontWeight = FontWeight.Bold },
                        new Button
                        {
                            Content = "OK",
                            Foreground = new SolidColorBrush(Colors.Azure),
                            Background = new SolidColorBrush(Colors.CadetBlue),
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
                    new TextBlock { Text = $"¿Estás seguro de que quieres eliminar {productosSeleccionados.Count} producto(s)?", 
                        Foreground = new SolidColorBrush(Colors.Azure), FontWeight = FontWeight.Bold },
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 10,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Background = new SolidColorBrush(Color.FromArgb(240,250,100,100)),
                        Children =
                        {
                            new Button
                            {
                                Content = "Aceptar",
                                Foreground = new SolidColorBrush(Colors.Azure),
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
                                Foreground = new SolidColorBrush(Colors.Azure),
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
    
    
    // Metodos activadores de paneles:
    [RelayCommand]
    private void ActivarPanelVer()
    {
        PanelInicio = false;
        PanelAniadir = false;
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = true;
        PanelAyuda = false;
        contadorLista = 0;
        TextoContador = "Producto " + (contadorLista + 1) + " de " + Productos.Count + ":";
        LimpiarPanelesDonacion();
        ProductosALista();
        ComprobarBotonesIndividual();
        IniciarCampos();
    }
    
    [RelayCommand]
    private void ActivarPanelAniadir()
    {
        PanelInicio = false;
        PanelAniadir = true;
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = false;
        PanelAyuda = false;
        LimpiarCampos();
        LimpiarPanelesDonacion();
        Foto = ImagenPorDefecto;
    }
    
    [RelayCommand]
    private void ActivarPanelEliminar()
    {
        PanelInicio = false;
        PanelAniadir = false;
        PanelDonar = false;
        PanelEliminar = true;
        PanelInfo = false;
        PanelVer = false;
        PanelAyuda = false;
        LimpiarPanelesDonacion();
        ProductosALista();
        IniciarCampos();
    }
    
    [RelayCommand]
    private void ActivarPanelInfo()
    {
        PanelInicio = false;
        PanelAniadir = false;
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = true;
        PanelVer = false;
        PanelAyuda = false;
        LimpiarCampos();
        LimpiarPanelesDonacion();
        Foto = ImagenPorDefecto;
    }
    
    [RelayCommand]
    private void ActivarPanelDonar()
    {
        PanelInicio = false;
        CantidadDonacion = 0.0;
        ComprobarImagenDonacion();
        PanelAniadir = false;
        PanelDonar = true;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = false;
        PanelAyuda = false;
        LimpiarCampos();
        LimpiarPanelesDonacion();
        Foto = ImagenPorDefecto;
    }
    
    [RelayCommand]
    private void ActivarPanelAyuda()
    {
        PanelInicio = false;
        PanelAniadir = false;
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = false;
        PanelAyuda = true;
        LimpiarCampos();
        LimpiarPanelesDonacion();
        Foto = ImagenPorDefecto;
    }

    
    // Metodos para el control de los articulos en la vista individual:
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
    
    
    // Implementación de INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}


