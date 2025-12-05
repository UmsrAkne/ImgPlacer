using System;
using System.Windows;
using System.Windows.Input;
using ImgPlacer.ViewModels;
using ImgPlacer.Views.Controls;
using Microsoft.Xaml.Behaviors;

namespace ImgPlacer.Utils.Behaviors
{
    public class CanvasDragBehavior : Behavior<ImageCanvasViewer>
    {
        private Point? dragStartPoint;
        private double startOffsetX;
        private double startOffsetY;
        private ImageCanvasViewerViewModel vm;
        private DateTime lastNotify = DateTime.MinValue;

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.DataContextChanged += OnDataContextChanged;
            AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
            vm = AssociatedObject.DataContext as ImageCanvasViewerViewModel;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            base.OnDetaching();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragStartPoint = e.GetPosition(AssociatedObject);
            if (vm != null)
            {
                startOffsetX = vm.OffsetX;
                startOffsetY = vm.OffsetY;
            }

            AssociatedObject.CaptureMouse();
            TryNotifyUserAction();
            Mouse.OverrideCursor = Cursors.Hand;
            e.Handled = true;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (dragStartPoint.HasValue && e.LeftButton == MouseButtonState.Pressed)
            {
                var current = e.GetPosition(AssociatedObject);
                var delta = current - dragStartPoint.Value;
                if (vm != null)
                {
                    vm.OffsetX = startOffsetX + delta.X;
                    vm.OffsetY = startOffsetY + delta.Y;
                }

                TryNotifyUserAction();
                e.Handled = true;
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragStartPoint = null;
            AssociatedObject.ReleaseMouseCapture();
            Mouse.OverrideCursor = null;
            e.Handled = true;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            vm = AssociatedObject.DataContext as ImageCanvasViewerViewModel;
        }

        private void TryNotifyUserAction()
        {
            var now = DateTime.Now;
            if ((now - lastNotify).TotalMilliseconds > 50)
            {
                vm.IsInfoHighlighted = true;
                lastNotify = now;
            }
        }
    }
}