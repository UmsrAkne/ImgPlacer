using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImgPlacer.Utils
{
    public static class ImageBoundsCalculator
    {
        private readonly static Dictionary<string, Int32Rect> CropCache = new ();

        public static async Task<Int32Rect> GetOpaquePixelBoundsAsync(BitmapImage bitmap, string keyPath)
        {
            var rect = GetOpaquePixelBounds(keyPath);
            if (rect != Int32Rect.Empty)
            {
                return rect;
            }

            // 画像を読み込む
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

                if (!foundOpaquePixel)
                {
                    AddOpaquePixelBounds(keyPath, Int32Rect.Empty);
                    return Int32Rect.Empty;
                }

                var result = new Int32Rect(minX, minY, maxX - minX + 1, maxY - minY + 1);
                AddOpaquePixelBounds(keyPath, result);
                return result;
            });
        }

        private static Int32Rect GetOpaquePixelBounds(string keyPath)
        {
            var lower = keyPath.ToLower();
            return CropCache.TryGetValue(lower, out var rect) ? rect : Int32Rect.Empty;
        }

        private static void AddOpaquePixelBounds(string keyPath, Int32Rect rect)
        {
            var lower = keyPath.ToLower();
            CropCache.TryAdd(lower, rect);
        }
    }
}