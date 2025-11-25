using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Microsoft.Xaml.Behaviors;

namespace ImgPlacer.Utils.Behaviors
{
    public class BubbleMouseWheelBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var parent = AssociatedObject.GetParentObject() as UIElement;
            parent?.RaiseEvent(
                new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender,
                });
        }
    }
}