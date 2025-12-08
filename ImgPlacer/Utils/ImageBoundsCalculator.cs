using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImgPlacer.Utils
{
    public static class ImageBoundsCalculator
    {
        public static async Task<Int32Rect> GetOpaquePixelBoundsAsync(string imagePath)
        {
            // 画像を読み込む
            var bitmap = new BitmapImage(new Uri(imagePath));
            var convertedBitmap = new FormatConvertedBitmap(bitmap, System.Windows.Media.PixelFormats.Bgra32, null, 0);

            // ピクセルデータを取得
            var width = convertedBitmap.PixelWidth;
            var height = convertedBitmap.PixelHeight;
            var stride = width * 4;
            var pixels = new byte[height * stride];
            convertedBitmap.CopyPixels(pixels, stride, 0);

            return await Task.Run(() =>
            {
                // 不透明ピクセルの範囲を計算する
                var minX = width;
                var minY = height;
                var maxX = 0;
                var maxY = 0;

                var foundOpaquePixel = false;

                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var index = (y * stride) + (x * 4);
                        var alpha = pixels[index + 3];

                        // 不透明ピクセル
                        if (alpha == 0)
                        {
                            continue;
                        }

                        foundOpaquePixel = true;
                        if (x < minX)
                        {
                            minX = x;
                        }

                        if (y < minY)
                        {
                            minY = y;
                        }

                        if (x > maxX)
                        {
                            maxX = x;
                        }

                        if (y > maxY)
                        {
                            maxY = y;
                        }
                    }
                }

                return !foundOpaquePixel
                    ? Int32Rect.Empty
                    : new Int32Rect(minX, minY, maxX - minX + 1, maxY - minY + 1);
            });
        }
    }
}