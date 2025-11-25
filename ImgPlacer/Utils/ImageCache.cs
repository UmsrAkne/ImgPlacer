using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ImgPlacer.Utils
{
    public static class ImageCache
    {
        private readonly static Dictionary<string, BitmapImage> Cache = new ();

        public static BitmapImage Get(string fullPath)
        {
            if (Cache.TryGetValue(fullPath, out var bmp))
            {
                return bmp;
            }

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(fullPath);
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.EndInit();
            bitmap.Freeze();

            Cache[fullPath] = bitmap;
            return bitmap;
        }
    }
}