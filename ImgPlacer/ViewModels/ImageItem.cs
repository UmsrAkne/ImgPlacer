using System.Windows.Media;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    public class ImageItem : BindableBase
    {
        private ImageSource thumbnail;
        private string fileName;
        private string resolutionText;

        public ImageSource Thumbnail { get => thumbnail; set => SetProperty(ref thumbnail, value); }

        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }

        public string ResolutionText
        {
            get => resolutionText;
            set => SetProperty(ref resolutionText, value);
        } // "1920x1080"
    }
}