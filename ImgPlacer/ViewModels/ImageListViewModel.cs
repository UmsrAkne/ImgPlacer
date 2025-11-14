using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageListViewModel : BindableBase
    {
        private ImageItem selectedImage;

        public ObservableCollection<ImageItem> Images { get; } = new();

        public ImageItem SelectedImage { get => selectedImage; set => SetProperty(ref selectedImage, value); }

        /// <summary>
        /// 指定したディレクトリから png 画像を読み込み、ImageItem として Images に追加します。
        /// </summary>
        /// <param name="dirPath">ディレクトリのフルパス</param>
        public void LoadFromDirectory(string dirPath)
        {
            // 既存をクリア
            Images.Clear();

            if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
            {
                SelectedImage = null;
                return;
            }

            string[] files;
            try
            {
                files = Directory.GetFiles(dirPath, "*.png", SearchOption.TopDirectoryOnly);
            }
            catch
            {
                SelectedImage = null;
                return;
            }

            foreach (var file in files.OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // ファイルロック回避
                    bitmap.UriSource = new Uri(file);
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    var item = new ImageItem
                    {
                        Thumbnail = bitmap,
                        FileName = Path.GetFileName(file),
                        ResolutionText = $"{bitmap.PixelWidth}x{bitmap.PixelHeight}",
                    };

                    Images.Add(item);
                }
                catch
                {
                    // 個別ファイルの読み込み失敗はメッセージを出すのみに留める。
                    Console.WriteLine($"{file} の読み込みに失敗しました。");
                }
            }

            SelectedImage = Images.FirstOrDefault();
        }
    }
}