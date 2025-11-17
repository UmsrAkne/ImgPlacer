using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageListViewModel : BindableBase
    {
        private readonly static Regex NamingRegex = new Regex(@"^[A-Za-z]\d{4}\.png$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private ImageItem selectedImage;

        // 先頭文字でフィルタ（例: "A"/"B"/"C"/"D"）。null/空なら全件
        public string FilterPrefix { get; set; }

        // Aリストの選択に連動して絞り込む2桁（null/空なら無効）
        public string FilterNumberHead2 { get; set; }

        public ObservableCollection<ImageItem> Images { get; set; } = new();

        public ImageItem SelectedImage { get => selectedImage; set => SetProperty(ref selectedImage, value); }

        /// <summary>
        /// 指定したディレクトリから png 画像を読み込み、ImageItem として Images に追加します。
        /// さらに、先頭文字と命名規則の適合可否を各 ImageItem に記録し、FilterPrefix/FilterNumberHead2 に従って表示対象を制限します。
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
                    var fileNameOnly = Path.GetFileName(file);
                    var leading = !string.IsNullOrEmpty(fileNameOnly) && char.IsLetter(fileNameOnly[0])
                        ? char.ToUpperInvariant(fileNameOnly[0]).ToString()
                        : null;
                    var isValid = NamingRegex.IsMatch(fileNameOnly);

                    // 2桁を抽出（命名が有効な場合のみ）
                    string head2 = null;
                    if (isValid)
                    {
                        head2 = fileNameOnly.Substring(1, 2); // A0101.png の "01"
                    }

                    // フィルタ: FilterPrefix が設定されている場合は一致する先頭文字のみ表示
                    if (!string.IsNullOrEmpty(FilterPrefix))
                    {
                        if (!string.Equals(leading, FilterPrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }

                    // フィルタ: FilterNumberHead2 が設定されている場合は一致する2桁のみ
                    if (!string.IsNullOrEmpty(FilterNumberHead2))
                    {
                        if (!string.Equals(head2, FilterNumberHead2, StringComparison.Ordinal))
                        {
                            continue;
                        }
                    }

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
                        FileName = fileNameOnly,
                        ResolutionText = $"{bitmap.PixelWidth}x{bitmap.PixelHeight}",
                        LeadingLetter = leading,
                        IsNamingValid = isValid,
                        FirstTwoDigits = head2,
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

        public ImageListViewModel GetClone()
        {
            return new ImageListViewModel
            {
                FilterPrefix = FilterPrefix,
                FilterNumberHead2 = FilterNumberHead2,
                Images = new ObservableCollection<ImageItem>(Images),
                SelectedImage = SelectedImage,
            };
        }
    }
}