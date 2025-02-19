using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Catalogo_Avalonia_Final.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using ReactiveUI;

namespace Catalogo_Avalonia_Final.ViewModels;

public partial class CatalogoViewModel : ObservableObject
{

    // Declaracion de propiedades del objeto Producto:
    [ObservableProperty] public string? _nombre;
    [ObservableProperty] public string? _marca;
    [ObservableProperty] public string? _descripcion;
    [ObservableProperty] public double? _precio;
    [ObservableProperty] public string? _precioTexto;
    [ObservableProperty] public string? _errorPrecio;
    [ObservableProperty] public int? _stock;
    [ObservableProperty] public string? _stockTexto;
    [ObservableProperty] public string? _errorStock;
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
    [ObservableProperty] public string _nombreListaActual;
    [ObservableProperty] public string _textoContador;
    [ObservableProperty] public AvaloniaList<Producto>? _productos;
    [ObservableProperty] public AvaloniaList<String>? _productosLista;
    [ObservableProperty] public AvaloniaList<String>? _categorias; 
    
    
    // Metodos para el control de la vista pestaña del panel ver:
    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (_selectedIndex != value)
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }
    }
    
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
        NombreListaActual = "Ejemplo.bin";
        Categorias = new AvaloniaList<String>{"Sonido","Video","Raton","Teclado"};
        ImagenDonacion = null;
        ImagenEmpresa = new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/empresa.png")));
        ImagenEmpresaFondo = new Bitmap(AssetLoader.Open(new Uri("avares://Catalogo_Avalonia_Final/Assets/empresafinal.png")));
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
        Productos = new AvaloniaList<Producto>();
        ProductosLista = new AvaloniaList<String>();
        CargarProductos();
    }

    private void IniciarCampos()
    {
        if (Productos != null && Productos.Count > 0 && contadorLista >= 0 && contadorLista < Productos.Count)
        {
            if (Productos.Count == 0)
            {
                TextoContador = "Producto 0 de " + Productos.Count + ":";
                
            }
            else
            {
                TextoContador = "Producto " + (contadorLista + 1) + " de " + Productos.Count + ":";
            }
            Nombre = Productos[contadorLista].Nombre;
            Marca = Productos[contadorLista].Marca;
            Descripcion = Productos[contadorLista].Descripcion;
            Precio = Productos[contadorLista].Precio; 
            Stock = Productos[contadorLista].Stock; 
            Categoria = Productos[contadorLista].Categoria; 
            OtraInformacion = Productos[contadorLista].OtraInformacion; 
            Foto = Productos[contadorLista].Foto;
        }
        
        
    }
    
    // Convertidores y errores de precio y stock para añadir
    public double? PrecioComoNumero
    {
        get
        {
            // Parsea a double
            if (double.TryParse(PrecioTexto, out double resultado))
            {
                return resultado;
            }
            return null;
        }
    }

    partial void OnPrecioTextoChanged(string? newValue)
    {
        // Control para poder borrar el 0 inicial del texto:
        if (string.IsNullOrEmpty(newValue) || double.TryParse(newValue, out _))
        {
            ErrorPrecio = null; 
        }
        else
        {
            ErrorPrecio = "El precio debe ser un número entero o decimal válido.";
        }
    }
    
    public int? StockComoNumero
    {
        get
        {
            // Parsea a int
            if (int.TryParse(StockTexto, out int resultado))
            {
                return resultado;
            }
            return null;
        }
    }

    partial void OnStockTextoChanged(string? newValue)
    {
        // Control para poder borrar el 0 inicial del texto:
        if (string.IsNullOrEmpty(newValue) || int.TryParse(newValue, out _))
        {
            ErrorPrecio = null;
        }
        else
        {
            ErrorPrecio = "El stock debe ser un número entero válido.";
        }
    }
    
    
    // Metodo para pasar la lista de productos a una lista de string:
    private void ProductosALista()
    {
        ProductosLista.Clear();
        foreach (var produto in Productos)
        {
            StringBuilder productoTexto = new StringBuilder();
            productoTexto.Append("NOMBRE: " + produto.Nombre.PadRight(5).PadLeft(5));
            productoTexto.Append("  -- MARCA: " + produto.Marca.PadRight(5).PadLeft(5));
            productoTexto.Append("  -- CATEGORIA: " + produto.Categoria.PadRight(5).PadLeft(5));
            productoTexto.Append("  -- PRECIO: " + produto.Precio.ToString().PadRight(5).PadLeft(5) + "€/u.");
            productoTexto.Append("  -- STOCK: " + produto.Stock.ToString().PadLeft(5)+ "u.");
            ProductosLista.Add(productoTexto.ToString());
        } 
    }

    // Metodos para la comprobacion necesaria de botones e imagenes:
    private void ComprobarBotonesIndividual()
    {
        if (contadorLista == Productos.Count - 1 || Productos.Count == 0)
        {
            Siguiente = false;
        }
        else
        {
            Siguiente = true;
        }

        if (contadorLista == 0 || Productos.Count == 0)
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
        
        // Cambia la foto del emoji y los stack panel de colores:
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
    
    [RelayCommand]
    private void LimpiarCampos()
    {
        contadorLista = 0;
        Nombre = string.Empty;
        Marca = string.Empty;
        Descripcion = string.Empty;
        PrecioTexto = "0.0";
        Precio = 0.0;
        StockTexto = "0";
        Stock = 0;
        Categoria = string.Empty;
        OtraInformacion = string.Empty;
        Foto = ImagenPorDefecto;
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
    
    // Metodos para gestion de las listas:
    [RelayCommand]
    private async Task GuardarProductos()
    {
        // Dialogo para confirmar la accion:
        var result = await DialogHost.Show(
            new StackPanel
            {
                Width = 450,
                Height = 70,
                Spacing = 10,
                Background = new SolidColorBrush(Color.FromArgb(255,50,50,50)),
                Children =
                {
                    new TextBlock { Text = $"¿Estás seguro de que quieres guardar los cambios del catálogo?", 
                        Foreground = new SolidColorBrush(Colors.Azure), FontWeight = FontWeight.Bold, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, 
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new Thickness(5)},
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 10,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Children =
                        {
                            new Button
                            {
                                Content = "Aceptar",
                                Foreground = new SolidColorBrush(Colors.Azure),
                                Background = new SolidColorBrush(Colors.CadetBlue),
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                                Command = new RelayCommand(() =>
                                {
                                    // Cerrar el diálogo con "Yes" como resultado
                                    DialogHost.GetDialogSession("DosDialogHost")?.Close("Yes");
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
                                    DialogHost.GetDialogSession("DosDialogHost")?.Close("No");
                                })
                            }
                        }
                    }
                }
            },
            "DosDialogHost"
        );
        
        // Proceder solo si el usuario hace clic en "Aceptar"
        if (result.ToString() != "Yes")
        {
            return;
        }
        
        using (var stream = new FileStream(NombreListaActual, FileMode.Create, FileAccess.Write)) 
        using (var writer = new BinaryWriter(stream))
        {
            writer.Write(Productos.Count);
            foreach (var vProducto in Productos)
            {
                vProducto.Serializar(writer);
            }
        }
        
        
        // Dialogo para terminar la accion:
        await DialogHost.Show(
            new StackPanel
            {
                Orientation = Orientation.Vertical,
                Width = 300,
                Height = 70,
                Spacing = 10,
                Background = new SolidColorBrush(Color.FromArgb(255,50,50,50)),
                Children =
                {
                    new TextBlock { Text = "Catálogo guardado con exito.",Foreground = new SolidColorBrush(Colors.Azure), 
                        FontWeight = FontWeight.Bold, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, 
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new Thickness(5)},
                    new Button
                    {
                        Content = "Gracias!",
                        Foreground = new SolidColorBrush(Colors.Azure),
                        Background = new SolidColorBrush(Colors.CadetBlue),
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Command = new RelayCommand(() =>
                        {
                            // Cerrar el diálogo
                            DialogHost.GetDialogSession("DosDialogHost")?.Close();
                        })
                    }
                }
            },
            "DosDialogHost"
        );
    }

    
    // Metodo para futuras actualizaciones:
    [RelayCommand]
    private void NuevosProductos()
    {
            
    }

    // Metodo para cargar los datos del fichero:
    [RelayCommand]
    private void CargarProductos()
    {
        Productos.Clear();
        ProductosLista.Clear();

        // Comprueba si el archivo existe o esta vacio para inicializarlo con el numero de objetos:
        if (!File.Exists(NombreListaActual) || new FileInfo(NombreListaActual).Length == 0)
        {
            using (var stream = new FileStream(NombreListaActual, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(0);
            }
        }

        using (var stream = new FileStream(NombreListaActual, FileMode.Open, FileAccess.Read))
        using (var reader = new BinaryReader(stream))
        {

            int cantidad = reader.ReadInt32(); // Leer la cantidad de productos


            for (int i = 0; i < cantidad; i++)
            {
                Productos.Add(Producto.Deserializar(reader)); // Deserializar cada producto
            }


        }
        ProductosALista();
    }
    
        
    

    
    // Metodos para gestion de los productos:
    [RelayCommand]
    private async Task AgregarProducto()
    {
        if (!string.IsNullOrEmpty(Nombre) && !string.IsNullOrEmpty(Marca) && !string.IsNullOrEmpty(Categoria))
        {
             
            var nuevoProducto =
                new Producto(Nombre, Marca, Descripcion??"", PrecioComoNumero??0.0, StockComoNumero??0, Categoria  , OtraInformacion??"", 
                    Foto!);
            
            Productos.Add(nuevoProducto); 
            LimpiarCampos(); /
            
            // Actualizar la lista de productos en formato texto
            ProductosLista.Clear();
            ProductosALista();
            
            // Mostrar mensaje si se agrego correctamente el usuario
            await DialogHost.Show(
                new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Width = 400,
                    Height = 70,
                    Spacing = 10,
                    Background = new SolidColorBrush(Color.FromArgb(255,50,70,50)),
                    Children =
                    {
                        new TextBlock { Text = "Producto agregado correctamente!",Foreground = new SolidColorBrush(Colors.Azure), 
                            FontWeight = FontWeight.Bold, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, 
                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new Thickness(5)},
                        new Button
                        {
                            Content = "PERFECTO",
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
        else
        {
            // Mostrar mensaje si faltan datos minimos necesarios
            await DialogHost.Show(
                new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Width = 400,
                    Height = 70,
                    Spacing = 10,
                    Background = new SolidColorBrush(Color.FromArgb(255,50,50,50)),
                    Children =
                    {
                        new TextBlock { Text = "Nombre, Marca y Categoria son OBLIGATORIOS.",Foreground = new SolidColorBrush(Colors.Azure), 
                            FontWeight = FontWeight.Bold, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, 
                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new Thickness(5)},
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
    }

    [RelayCommand]
    private async void SubirFoto(Window ventanaPadre)
    {
        try
        {
            var dlg = new OpenFileDialog();
            dlg.Filters.Add(new FileDialogFilter() { Name = "Imágenes JPEG", Extensions = { "jpg" } });
            dlg.Filters.Add(new FileDialogFilter() { Name = "Imágenes PNG", Extensions = { "png" } });
            dlg.Filters.Add(new FileDialogFilter() { Name = "Todos los archivos", Extensions = { "*" } });
            dlg.AllowMultiple = false;

            var result = await dlg.ShowAsync(ventanaPadre);
            if (result != null)
            {
                string rutaFoto = result[0];
                Foto = new Bitmap(rutaFoto);
            }
        }
        catch (Exception ex)
        {
            return;
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
                    Width = 400,
                    Height = 70,
                    Spacing = 10,
                    Background = new SolidColorBrush(Color.FromArgb(255,50,50,50)),
                    Children =
                    {
                        new TextBlock { Text = "No has seleccionado ningún producto para eliminar.",Foreground = new SolidColorBrush(Colors.Azure), 
                            FontWeight = FontWeight.Bold, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, 
                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new Thickness(5)},
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
                Width = 400,
                Height = 70,
                Spacing = 10,
                Background = new SolidColorBrush(Color.FromArgb(255,50,50,50)),
                Children =
                {
                    new TextBlock { Text = $"¿Estás seguro de que quieres eliminar {productosSeleccionados.Count} producto(s)?", 
                        Foreground = new SolidColorBrush(Colors.Azure), FontWeight = FontWeight.Bold, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, 
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new Thickness(5)},
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 10,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Background = new SolidColorBrush(Color.Parse("#600000")),
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
            contadorLista = 0;
        }
        
    }
    
    
    // Metodos activadores de paneles:
    [RelayCommand]
    private void ActivarPanelVer()
    {
        LimpiarPanelesDonacion();
        
        PanelInicio = false;
        PanelAniadir = false;
        PanelDonar = false;
        PanelEliminar = false;
        PanelInfo = false;
        PanelVer = true;
        PanelAyuda = false;
        TextoContador = "Producto 0 de " + Productos.Count + ":";
        LimpiarCampos();
        IniciarCampos();
        ComprobarBotonesIndividual();
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
        LimpiarPanelesDonacion();
        
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
        LimpiarPanelesDonacion();
        
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
        LimpiarPanelesDonacion();
        
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


