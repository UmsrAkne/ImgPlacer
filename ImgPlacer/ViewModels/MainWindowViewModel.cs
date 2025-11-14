using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private string title = "ImgPlacer";

    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    public ImageLayerManagerViewModel ImageLayerManagerViewModel { get; set; } = new ();
}