using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

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

    public readonly static DependencyProperty IsHudActiveProperty =
        DependencyProperty.Register(
            nameof(IsHudActive),
            typeof(bool),
            typeof(ImageCanvasViewer),
            new PropertyMetadata(false));

    public readonly static DependencyProperty ImageSourceProperty = DependencyProperty.Register(
        nameof(ImageSource), typeof(ImageSource), typeof(ImageCanvasViewer), new PropertyMetadata(null));

    private readonly DispatcherTimer idleTimer;

    private DateTime lastActionTime;

    public ImageCanvasViewer()
    {
        InitializeComponent();
        UpdateSizeBindings();

        idleTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(200),
        };

        idleTimer.Tick += IdleTimer_Tick;
        idleTimer.Start();

        lastActionTime = DateTime.Now;
    }

    public bool IsHudActive
    {
        get => (bool)GetValue(IsHudActiveProperty);
        set => SetValue(IsHudActiveProperty, value);
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

    public void NotifyUserAction()
    {
        IsHudActive = true;
        lastActionTime = DateTime.Now;
    }

    private static void OnFrameSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ImageCanvasViewer viewer)
        {
            viewer.UpdateSizeBindings();
        }
    }

    private void IdleTimer_Tick(object sender, EventArgs e)
    {
        if ((DateTime.Now - lastActionTime).TotalSeconds > 1.2)
        {
            IsHudActive = false;
        }
    }

    private void UpdateSizeBindings()
    {
    }
}