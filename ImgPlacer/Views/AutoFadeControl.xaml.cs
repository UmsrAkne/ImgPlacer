using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ImgPlacer.Views
{
    public partial class AutoFadeControl
    {
        public readonly static DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register(
                nameof(IsHighlighted),
                typeof(bool),
                typeof(AutoFadeControl),
                new PropertyMetadata(false, OnIsHighlightedChanged));

        private const double BaseOpacity = 0.4;

        private const double HighlightOpacity = 0.8;

        private bool isHighlighting;

        public AutoFadeControl()
        {
            InitializeComponent();
            Opacity = BaseOpacity;
        }

        public bool IsHighlighted
        {
            get => (bool)GetValue(IsHighlightedProperty);
            set => SetValue(IsHighlightedProperty, value);
        }

        private static void OnIsHighlightedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (AutoFadeControl)d;
            control.OnHighlightChanged((bool)e.NewValue);
        }

        private void OnHighlightChanged(bool isHighlighted)
        {
            if (isHighlighted)
            {
                FadeTo(HighlightOpacity);

                if (isHighlighting)
                {
                    return;
                }

                isHighlighting = true;

                // 数秒後に元に戻す
                var timer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(2),
                };

                timer.Tick += (_, _) =>
                {
                    timer.Stop();
                    FadeTo(BaseOpacity);
                    isHighlighting = false;
                    SetValue(IsHighlightedProperty, false);
                };

                timer.Start();
            }
        }

        // ============================
        //   汎用フェードアニメーション
        // ============================
        private void FadeTo(double target)
        {
            var anim = new DoubleAnimation
            {
                To = target,
                Duration = TimeSpan.FromMilliseconds(200),
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.2,
            };

            BeginAnimation(OpacityProperty, anim);
        }
    }
}