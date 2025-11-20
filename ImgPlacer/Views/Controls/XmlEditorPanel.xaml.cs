using System;
using System.IO;
using System.Linq;
using System.Windows;
using ImgPlacer.Utils;
using ImgPlacer.ViewModels;

namespace ImgPlacer.Views.Controls
{
    public partial class XmlEditorPanel
    {
        public XmlEditorPanel()
        {
            InitializeComponent();
        }

        private static void UpdateDragEffect(DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            UpdateDragEffect(e);
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            UpdateDragEffect(e);
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    return;
                }

                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var file = files?.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(file) || !File.Exists(file))
                {
                    return;
                }

                // XML を読み込む
                var xDoc = XmlFileLoader.LoadXml(file);

                if (DataContext is XmlEditorPanelViewModel vm)
                {
                    vm.LoadedDocument = xDoc;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"XML の読み込みに失敗しました:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                e.Handled = true;
            }
        }
    }
}