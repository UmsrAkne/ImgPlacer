using System.Windows;
using System.Windows.Media;

namespace ImgPlacer.Views.Controls;

public partial class ImageCanvasViewer
{
    public readonly static DependencyProperty FrameWidthProperty = DependencyProperty.Register(
        nameof(FrameWidth),
        typeof(double),
        typeof(ImageCanvasViewer),
        new FrameworkPropertyMetadata(
            300d,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnFrameSizeChanged));

    public readonly static DependencyProperty FrameHeightProperty = DependencyProperty.Register(
        nameof(FrameHeight),
        typeof(double),
        typeof(ImageCanvasViewer),
        new FrameworkPropertyMetadata(
            200d,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnFrameSizeChanged));

    public readonly static DependencyProperty ImageSourceProperty = DependencyProperty.Register(
        nameof(ImageSource), typeof(ImageSource), typeof(ImageCanvasViewer), new PropertyMetadata(null));

    public ImageCanvasViewer()
    {
        InitializeComponent();
        UpdateSizeBindings();
    }

    public double FrameWidth
    {
        get => (double)GetValue(FrameWidthProperty);
        set => SetValue(FrameWidthProperty, value);
    }

    public double FrameHeight
    {
        get => (double)GetValue(FrameHeightProperty);
        set => SetValue(FrameHeightProperty, value);
    }

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    private static void OnFrameSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ImageCanvasViewer viewer)
        {
            viewer.UpdateSizeBindings();
        }
    }

    private void UpdateSizeBindings()
    {
    }
}