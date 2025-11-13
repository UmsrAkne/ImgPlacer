using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace ImgPlacer.Utils.Behaviors
{
    /// <summary>
    /// A behavior that enables drag-and-drop of folders on a Window.
    /// Only directory paths are accepted. On drop, it invokes the optional FolderDroppedCommand with a string[] of directory paths.
    /// </summary>
    public class FolderDropBehavior : Behavior<Window>
    {
        public readonly static DependencyProperty FolderDroppedCommandProperty = DependencyProperty.Register(
            nameof(FolderDroppedCommand), typeof(ICommand), typeof(FolderDropBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Command executed when valid folders are dropped.
        /// Parameter: string[] of directory paths.
        /// </summary>
        public ICommand FolderDroppedCommand
        {
            get => (ICommand)GetValue(FolderDroppedCommandProperty);
            set => SetValue(FolderDroppedCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject == null)
            {
                return;
            }

            AssociatedObject.AllowDrop = true;
            AssociatedObject.DragOver += OnDragOver;
            AssociatedObject.Drop += OnDrop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject == null)
            {
                return;
            }

            AssociatedObject.DragOver -= OnDragOver;
            AssociatedObject.Drop -= OnDrop;
        }

        private static bool HasOnlyDirectories(IDataObject data)
        {
            if (!data.GetDataPresent(DataFormats.FileDrop))
            {
                return false;
            }

            if (data.GetData(DataFormats.FileDrop) is not string[] paths || paths.Length == 0)
            {
                return false;
            }

            try
            {
                return paths.All(Directory.Exists);
            }
            catch
            {
                return false;
            }
        }

        private static string[] GetDirectories(IDataObject data)
        {
            if (!data.GetDataPresent(DataFormats.FileDrop))
            {
                return Array.Empty<string>();
            }

            if (data.GetData(DataFormats.FileDrop) is not string[] paths || paths.Length == 0)
            {
                return Array.Empty<string>();
            }

            return paths.Where(Directory.Exists).ToArray();
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (HasOnlyDirectories(e.Data))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("FolderDropBehavior: アイテムがドロップされました");
            if (!HasOnlyDirectories(e.Data))
            {
                e.Handled = true;
                return;
            }

            var dirs = GetDirectories(e.Data);
            if (dirs.Length == 0)
            {
                e.Handled = true;
                return;
            }

            Console.WriteLine("FolderDropBehavior: アイテムのバリデーションをパスしました。");
            foreach (var dir in dirs)
            {
                Console.WriteLine($"FolderDropBehavior: {dir}");
            }

            if (FolderDroppedCommand != null && FolderDroppedCommand.CanExecute(dirs))
            {
                FolderDroppedCommand.Execute(dirs);
            }

            e.Handled = true;
        }
    }
}