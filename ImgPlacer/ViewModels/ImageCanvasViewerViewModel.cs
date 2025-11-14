using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageCanvasViewerViewModel : BindableBase
    {
        private double offsetX;
        private double offsetY;

        public ObservableCollection<ImageListViewModel> Layers { get; set; }

        public double OffsetX { get => offsetX; set => SetProperty(ref offsetX, value); }

        public double OffsetY { get => offsetY; set => SetProperty(ref offsetY, value); }
    }
}