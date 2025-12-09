using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ImgPlacer.Utils.Converters
{
    public class CropConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmap = values[0] as BitmapSource;
            var range = (Int32Rect)values[1];

            return bitmap != null
                ? new CroppedBitmap(bitmap, range)
                : DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}