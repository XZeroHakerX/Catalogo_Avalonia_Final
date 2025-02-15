using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Catalogo_Avalonia_Final.ViewModels;

namespace Catalogo_Avalonia_Final;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new Views.CatalogoView();
            desktop.MainWindow.DataContext = new CatalogoViewModel();
        }

        base.OnFrameworkInitializationCompleted();
    }
}