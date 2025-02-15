using System;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Catalogo_Avalonia_Final.ViewModels;

public partial class CatalogoViewModel : ObservableObject
{

    [ObservableProperty] public string _nombre;
    [ObservableProperty] public string _marca;
    [ObservableProperty] public string _descripcion;
    [ObservableProperty] public double _precio;
    [ObservableProperty] public int _stock;
    [ObservableProperty] public string _categoria;
    [ObservableProperty] public string _otraInformacion;
    [ObservableProperty] public string _foto;

    public CatalogoViewModel()
    {
        Nombre = string.Empty;
        Marca = string.Empty;
        Descripcion = string.Empty;
        Precio = 0.0;
        Stock = 0;
        Categoria = string.Empty;
        OtraInformacion = string.Empty;
        Foto = string.Empty;
    }
    
    
}