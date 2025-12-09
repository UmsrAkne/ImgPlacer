using System.Windows;
using System.Windows.Media;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    public class ImageItem : BindableBase
    {
        private readonly bool isNamingValid;
        private readonly string firstTwoDigits;
        private ImageSource thumbnail;
        private string fileName;
        private string resolutionText;
        private string leadingLetter;
        private Int32Rect opaqueRange;

        public ImageSource Thumbnail { get => thumbnail; set => SetProperty(ref thumbnail, value); }

        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }

        public string ResolutionText
        {
            get => resolutionText;
            set => SetProperty(ref resolutionText, value);
        } // "1920x1080"

        // 先頭の文字（例: "A" / "B" / ...）。命名規則チェック用に保持
        public string LeadingLetter { get => leadingLetter; set => SetProperty(ref leadingLetter, value); }

        // ファイル名が "A0000.png" 形式（先頭英字 + 4 桁）に合致しているか
        public bool IsNamingValid { get => isNamingValid; init => SetProperty(ref isNamingValid, value); }

        // 先頭文字の直後の2桁（例: A0101 -> "01"）。Aリスト選択に連動した絞り込み用
        public string FirstTwoDigits { get => firstTwoDigits; init => SetProperty(ref firstTwoDigits, value); }

        public Int32Rect OpaqueRange { get => opaqueRange; set => SetProperty(ref opaqueRange, value); }
    }
}