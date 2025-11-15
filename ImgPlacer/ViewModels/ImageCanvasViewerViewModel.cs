using System.Collections.ObjectModel;
using System.Windows;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageCanvasViewerViewModel : BindableBase
    {
        private double offsetX;
        private double offsetY;
        private double zoom = 1.0;
        private double imageWidth;
        private double imageHeight;
        private double canvasHeight;
        private double canvasWidth;

        public ObservableCollection<ImageListViewModel> Layers { get; set; }

        public double OffsetX { get => offsetX; set => SetProperty(ref offsetX, value); }

        public double OffsetY { get => offsetY; set => SetProperty(ref offsetY, value); }

        public double Zoom { get => zoom; set => SetProperty(ref zoom, value); }

        public double ImageWidth { get => imageWidth; set => SetProperty(ref imageWidth, value); }

        public double ImageHeight { get => imageHeight; set => SetProperty(ref imageHeight, value); }

        public double CanvasHeight { get => canvasHeight; set => SetProperty(ref canvasHeight, value); }

        public double CanvasWidth { get => canvasWidth; set => SetProperty(ref canvasWidth, value); }

        public Point GetCenteredOffset()
        {
            var canvasCenter = new Point(CanvasWidth / 2.0, CanvasHeight / 2.0);
            var imageCenter = new Point((ImageWidth * Zoom) / 2.0, (ImageHeight * Zoom) / 2.0);
            var offset = new Point(OffsetX, OffsetY);

            return new Point(
                offset.X + imageCenter.X - canvasCenter.X,
                offset.Y + imageCenter.Y - canvasCenter.Y);
        }
    }
}