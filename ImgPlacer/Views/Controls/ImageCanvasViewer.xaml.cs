using System;
using System.Windows;
using System.Windows.Input;
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

    public readonly static DependencyProperty ZoomProperty = DependencyProperty.Register(
        nameof(Zoom), typeof(double), typeof(ImageCanvasViewer), new PropertyMetadata(1.0d));

    private const double MinZoom = 0.05;
    private const double MaxZoom = 20.0;

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

    public double Zoom
    {
        get => (double)GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
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

    private void OnCanvasPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        // Zoom only when Ctrl is pressed
        if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
        {
            return;
        }

        var canvas = (IInputElement)sender;
        var mousePos = e.GetPosition(canvas);

        var oldZoom = Zoom;

        var ticks = e.Delta / 120;
        var factor = Math.Pow(1.05, ticks);
        var newZoom = Math.Clamp(oldZoom * factor, MinZoom, MaxZoom);

        if (Math.Abs(newZoom - oldZoom) < 0.0001)
        {
            e.Handled = true;
            return;
        }

        // Keep the point under the mouse stationary: x = cX * z + offX => offX' = x - cX * z'
        // var contentX = (mousePos.X - OffsetX) / oldZoom;
        // var contentY = (mousePos.Y - OffsetY) / oldZoom;

        Zoom = newZoom;

        // OffsetX = mousePos.X - (contentX * newZoom);
        // OffsetY = mousePos.Y - (contentY * newZoom);

        e.Handled = true;
    }
}