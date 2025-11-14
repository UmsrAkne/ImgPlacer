using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ImgPlacer.Views.Controls;

public partial class ImageCanvasViewer
{
    public static readonly DependencyProperty FrameWidthProperty = DependencyProperty.Register(
        nameof(FrameWidth), typeof(double), typeof(ImageCanvasViewer),
        new FrameworkPropertyMetadata(300d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnFrameSizeChanged));

    public static readonly DependencyProperty FrameHeightProperty = DependencyProperty.Register(
        nameof(FrameHeight), typeof(double), typeof(ImageCanvasViewer),
        new FrameworkPropertyMetadata(200d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnFrameSizeChanged));

    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
        nameof(ImageSource), typeof(ImageSource), typeof(ImageCanvasViewer), new PropertyMetadata(null));

    public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register(
        nameof(OffsetX), typeof(double), typeof(ImageCanvasViewer), new PropertyMetadata(0d));

    public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register(
        nameof(OffsetY), typeof(double), typeof(ImageCanvasViewer), new PropertyMetadata(0d));

    private Point? dragStartPoint;
    private double startOffsetX;
    private double startOffsetY;

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

    public double OffsetX
    {
        get => (double)GetValue(OffsetXProperty);
        set => SetValue(OffsetXProperty, value);
    }

    public double OffsetY
    {
        get => (double)GetValue(OffsetYProperty);
        set => SetValue(OffsetYProperty, value);
    }

    public ImageCanvasViewer()
    {
        InitializeComponent();
        UpdateSizeBindings();
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
        // Keep the control's size equal to the frame size
        Width = FrameWidth;
        Height = FrameHeight;
    }

    private void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var canvas = (IInputElement)sender;
        dragStartPoint = e.GetPosition(canvas);
        startOffsetX = OffsetX;
        startOffsetY = OffsetY;
        ((UIElement)sender).CaptureMouse();
        Mouse.OverrideCursor = Cursors.Hand;
        e.Handled = true;
    }

    private void OnCanvasMouseMove(object sender, MouseEventArgs e)
    {
        if (dragStartPoint.HasValue && e.LeftButton == MouseButtonState.Pressed)
        {
            var canvas = (IInputElement)sender;
            var current = e.GetPosition(canvas);
            var delta = current - dragStartPoint.Value;
            OffsetX = startOffsetX + delta.X;
            OffsetY = startOffsetY + delta.Y;
            e.Handled = true;
        }
    }

    private void OnCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        dragStartPoint = null;
        ((UIElement)sender).ReleaseMouseCapture();
        Mouse.OverrideCursor = null;
        e.Handled = true;
    }
}