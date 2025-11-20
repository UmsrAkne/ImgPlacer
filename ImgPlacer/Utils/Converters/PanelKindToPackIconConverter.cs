using ImgPlacer.Enums;

namespace ImgPlacer.Utils.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using MaterialDesignThemes.Wpf;

    public class PanelKindToPackIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not SideBarPanelKind kind)
            {
                return PackIconKind.Help; // フォールバック
            }

            return kind switch
            {
                SideBarPanelKind.CanvasSlider => PackIconKind.ZoomIn,
                SideBarPanelKind.XElementInput => PackIconKind.CodeBlockXml,
                SideBarPanelKind.CopyHistory => PackIconKind.History,
                SideBarPanelKind.XmlEditor => PackIconKind.FileDocumentEdit,

                _ => PackIconKind.Help,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}