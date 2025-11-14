using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private string title = "ImgPlacer";

    public MainWindowViewModel()
    {
        ImageLayerManagerViewModel = new ImageLayerManagerViewModel();
        ImageCanvasViewerViewModel = new ImageCanvasViewerViewModel();
        var layers = new ObservableCollection<ImageListViewModel>();
        for (var i = 0; i < 4; i++)
        {
            layers.Add(new ImageListViewModel());
        }

        ImageLayerManagerViewModel.Layers = layers;
        ImageCanvasViewerViewModel.Layers = layers;
    }

    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    public ImageLayerManagerViewModel ImageLayerManagerViewModel { get; private set; }

    public ImageCanvasViewerViewModel ImageCanvasViewerViewModel { get; private set; }
}