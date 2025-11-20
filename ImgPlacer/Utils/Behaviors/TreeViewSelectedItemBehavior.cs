using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace ImgPlacer.Utils.Behaviors
{
    /// <summary>
    /// Enables binding the SelectedItem of a TreeView via behavior.
    /// Usage:
    /// <i:Interaction.Behaviors>
    ///   <behaviors:TreeViewSelectedItemBehavior SelectedItem="{Binding SelectedNode, Mode=TwoWay}" />
    /// </i:Interaction.Behaviors>
    /// </summary>
    public class TreeViewSelectedItemBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem), typeof(object), typeof(TreeViewSelectedItemBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemChanged));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectedItemChanged += AssociatedObjectOnSelectedItemChanged;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectedItemChanged -= AssociatedObjectOnSelectedItemChanged;
            }

            base.OnDetaching();
        }

        private void AssociatedObjectOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Update bound property when a user changes selection in the TreeView
            SelectedItem = e.NewValue;
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Optional: handle programmatic selection from ViewModel in the future.
            // Keeping minimal implementation: one-way from TreeView -> VM.
        }
    }
}