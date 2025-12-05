using System.Windows;

namespace ImgPlacer.Views.Controls
{
    public partial class CursorPadContainer
    {
        public new readonly static DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(CursorPadContainer));

        public CursorPadContainer()
        {
            InitializeComponent();
        }

        public new object Content { get => GetValue(ContentProperty); set => SetValue(ContentProperty, value); }
    }
}