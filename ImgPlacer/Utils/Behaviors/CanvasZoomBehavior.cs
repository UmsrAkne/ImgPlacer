using System;
using System.Windows.Input;
using ImgPlacer.ViewModels;
using ImgPlacer.Views.Controls;
using Microsoft.Xaml.Behaviors;

namespace ImgPlacer.Utils.Behaviors
{
    public class CanvasZoomBehavior : Behavior<ImageCanvasViewer>
    {
        private const double MinZoom = 0.05;
        private const double MaxZoom = 20.0;
        private ImageCanvasViewerViewModel vm;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
            vm = AssociatedObject.DataContext as ImageCanvasViewerViewModel;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
            base.OnDetaching();
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Zoom only when Ctrl is pressed
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
            {
                return;
            }

            if (vm == null)
            {
                return;
            }

            var mousePos = e.GetPosition(AssociatedObject);
            var oldZoom = vm.Zoom;

            var ticks = e.Delta / 120;

            const ModifierKeys ctl = ModifierKeys.Control;
            const ModifierKeys shift = ModifierKeys.Shift;

            var zoomStep = (Keyboard.Modifiers & (ctl | shift)) == (ctl | shift)
                ? 0.25
                : MinZoom;

            var newZoom = Math.Clamp(oldZoom + (zoomStep * ticks), MinZoom, MaxZoom);

            if (Math.Abs(newZoom - oldZoom) < 0.0001)
            {
                e.Handled = true;
                return;
            }

            // Keep the point under the mouse stationary
            var contentX = (mousePos.X - vm.OffsetX) / oldZoom;
            var contentY = (mousePos.Y - vm.OffsetY) / oldZoom;

            vm.Zoom = newZoom;
            vm.OffsetX = mousePos.X - (contentX * newZoom);
            vm.OffsetY = mousePos.Y - (contentY * newZoom);

            e.Handled = true;

            AssociatedObject.NotifyUserAction();
        }
    }
}