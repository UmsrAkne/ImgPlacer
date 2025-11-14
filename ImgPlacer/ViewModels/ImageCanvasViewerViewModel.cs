using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageCanvasViewerViewModel : BindableBase
    {
        public ObservableCollection<ImageListViewModel> Layers { get; set; }
    }
}