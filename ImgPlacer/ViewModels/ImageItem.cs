using System.Windows.Media;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    public class ImageItem : BindableBase
    {
        private ImageSource thumbnail;
        private string fileName;
        private string resolutionText;
        private string leadingLetter;
        private bool isNamingValid;

        public ImageSource Thumbnail { get => thumbnail; set => SetProperty(ref thumbnail, value); }

        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }

        public string ResolutionText
        {
            get => resolutionText;
            set => SetProperty(ref resolutionText, value);
        } // "1920x1080"

        // 先頭の文字（例: "A" / "B" / ...）。命名規則チェック用に保持
        public string LeadingLetter { get => leadingLetter; set => SetProperty(ref leadingLetter, value); }

        // ファイル名が "Axxxx.png" 形式（先頭英字 + 4 桁）に合致しているか
        public bool IsNamingValid { get => isNamingValid; set => SetProperty(ref isNamingValid, value); }
    }
}