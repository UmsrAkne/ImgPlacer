using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ImgPlacer.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace ImgPlacer.Utils.Behaviors
{
    /// <summary>
    /// Attach to the Distance slider to temporarily move the ImageCanvasViewer while dragging.
    /// Behavior:
    /// - On drag start: remember ImageCanvasViewer original offsets.
    /// - While dragging: set offsets to original + vector (Distance, Degree).
    /// - On drag complete: restore original offsets.
    /// </summary>
    public class DistanceSliderDragPreviewBehavior : Behavior<Slider>
    {
        private bool isDragging;
        private double originalOffsetX;
        private double originalOffsetY;
        private CanvasSliderPanelViewModel panelVm;

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject == null)
            {
                return;
            }

            AssociatedObject.DataContextChanged += OnDataContextChanged;
            AssociatedObject.ValueChanged += OnValueChanged;
            AssociatedObject.AddHandler(Thumb.DragStartedEvent, new DragStartedEventHandler(OnDragStarted));
            AssociatedObject.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(OnDragCompleted));

            panelVm = AssociatedObject.DataContext as CanvasSliderPanelViewModel;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.DataContextChanged -= OnDataContextChanged;
                AssociatedObject.ValueChanged -= OnValueChanged;
                AssociatedObject.RemoveHandler(Thumb.DragStartedEvent, new DragStartedEventHandler(OnDragStarted));
                AssociatedObject.RemoveHandler(
                    Thumb.DragCompletedEvent,
                    new DragCompletedEventHandler(OnDragCompleted));
            }

            base.OnDetaching();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            panelVm = AssociatedObject?.DataContext as CanvasSliderPanelViewModel;
        }

        private void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            if (panelVm?.ImageCanvasViewerViewModel == null)
            {
                return;
            }

            var vm = panelVm.ImageCanvasViewerViewModel;
            originalOffsetX = vm.OffsetX;
            originalOffsetY = vm.OffsetY;
            isDragging = true;

            // Apply initial preview immediately with current value
            ApplyPreview();
        }

        private void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!isDragging)
            {
                return;
            }

            ApplyPreview();
        }

        private void OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (panelVm?.ImageCanvasViewerViewModel == null)
            {
                isDragging = false;
                return;
            }

            var vm = panelVm.ImageCanvasViewerViewModel;

            // Restore original offsets when thumb is released
            vm.OffsetX = originalOffsetX;
            vm.OffsetY = originalOffsetY;
            isDragging = false;
        }

        private void ApplyPreview()
        {
            if (panelVm?.ImageCanvasViewerViewModel == null)
            {
                return;
            }

            var vm = panelVm.ImageCanvasViewerViewModel;

            // Use the slider's current value as Distance; Degree comes from the panel VM
            var distance = AssociatedObject?.Value ?? panelVm.Distance;
            var degree = panelVm.Degree;

            // Convert to radians and compute delta
            var rad = degree * Math.PI / 180.0;
            var dx = distance * Math.Cos(rad);
            var dy = distance * Math.Sin(rad);

            vm.OffsetX = originalOffsetX + dx;
            vm.OffsetY = originalOffsetY + dy;
        }
    }
}