using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageListViewModel : BindableBase
    {
        private ImageItem selectedImage;

        public ObservableCollection<ImageItem> Images { get; } = new();

        public ImageItem SelectedImage { get => selectedImage; set => SetProperty(ref selectedImage, value); }
    }
}