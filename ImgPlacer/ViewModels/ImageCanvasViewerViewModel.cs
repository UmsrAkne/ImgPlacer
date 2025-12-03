using System.Collections.ObjectModel;
using System.Linq;
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
        private ObservableCollection<ImageListViewModel> layers;
        private bool isInfoHighlighted;

        public ObservableCollection<ImageListViewModel> Layers
        {
            get => layers;
            set => SetProperty(ref layers, value);
        }

        public double OffsetX
        {
            get => offsetX;
            set
            {
                if (SetProperty(ref offsetX, value))
                {
                    RaisePropertyChanged(nameof(DisplayOffset));
                }
            }
        }

        public double OffsetY
        {
            get => offsetY;
            set
            {
                if (SetProperty(ref offsetY, value))
                {
                    RaisePropertyChanged(nameof(DisplayOffset));
                }
            }
        }

        public double Zoom
        {
            get => zoom;
            set
            {
                if (SetProperty(ref zoom, value))
                {
                    RaisePropertyChanged(nameof(DisplayOffset));
                }
            }
        }

        public double ImageWidth { get => imageWidth; set => SetProperty(ref imageWidth, value); }

        public double ImageHeight { get => imageHeight; set => SetProperty(ref imageHeight, value); }

        public double CanvasHeight { get => canvasHeight; set => SetProperty(ref canvasHeight, value); }

        public double CanvasWidth { get => canvasWidth; set => SetProperty(ref canvasWidth, value); }

        public bool IsInfoHighlighted
        {
            get => isInfoHighlighted;
            set => SetProperty(ref isInfoHighlighted, value);
        }

        public Point DisplayOffset => GetCenteredOffset();

        public Point GetCenteredOffset()
        {
            var canvasCenter = new Point(CanvasWidth / 2.0, CanvasHeight / 2.0);
            var imageCenter = new Point((ImageWidth * Zoom) / 2.0, (ImageHeight * Zoom) / 2.0);
            var offset = new Point(OffsetX, OffsetY);

            return new Point(
                offset.X + imageCenter.X - canvasCenter.X,
                offset.Y + imageCenter.Y - canvasCenter.Y);
        }

        public void SetCenteredOffset(double centeredX, double centeredY, double scale)
        {
            Zoom = scale;
            OffsetX = centeredX + (CanvasWidth / 2.0) - (ImageWidth * scale / 2.0);
            OffsetY = centeredY + (CanvasHeight / 2.0) - (ImageHeight * scale / 2.0);
        }

        public ImageCanvasViewerViewModel GetClone()
        {
            var cloneList = Layers.Select(i => i.GetClone()).ToList();
            var vm = new ImageCanvasViewerViewModel
            {
                OffsetX = OffsetX,
                OffsetY = OffsetY,
                Zoom = Zoom,
                ImageWidth = ImageWidth,
                ImageHeight = ImageHeight,
                CanvasHeight = CanvasHeight,
                CanvasWidth = CanvasWidth,
                Layers = new ObservableCollection<ImageListViewModel>(cloneList),
            };

            return vm;
        }
    }
}