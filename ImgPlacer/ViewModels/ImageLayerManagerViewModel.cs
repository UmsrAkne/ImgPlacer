using System.Collections.ObjectModel;
using System.ComponentModel;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageLayerManagerViewModel : BindableBase
    {
        private ObservableCollection<ImageListViewModel> layers = new ();
        private string lastLoadedDirectory;

        public ObservableCollection<ImageListViewModel> Layers
        {
            get => layers;
            set
            {
                if (SetProperty(ref layers, value))
                {
                    RaisePropertyChanged(nameof(PrimaryLayer));
                    RaisePropertyChanged(nameof(LoadImagesCommand));
                    HookPrimarySelectionChanged();
                }
            }
        }

        public ImageListViewModel PrimaryLayer => Layers[0];

        public DelegateCommand<string> LoadImagesCommand => new DelegateCommand<string>((param) =>
        {
            lastLoadedDirectory = param;
            foreach (var layer in Layers)
            {
                layer.LoadFromDirectory(param);
            }

            // 初期読み込み後にAの選択に応じてB~Dを再絞り込み
            ApplyPrimarySelectionFilterToOtherLayers();
        });

        private void HookPrimarySelectionChanged()
        {
            if (Layers == null || Layers.Count == 0)
            {
                return;
            }

            // いったん既存の購読を解除（安全のため）
            foreach (var layer in Layers)
            {
                layer.PropertyChanged -= OnLayerPropertyChanged;
            }

            PrimaryLayer.PropertyChanged += OnLayerPropertyChanged;
        }

        private void OnLayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ImageListViewModel.SelectedImage))
            {
                ApplyPrimarySelectionFilterToOtherLayers();
            }
        }

        private void ApplyPrimarySelectionFilterToOtherLayers()
        {
            if (Layers == null || Layers.Count < 4)
            {
                return;
            }

            var head2 = PrimaryLayer?.SelectedImage?.IsNamingValid == true
                ? PrimaryLayer.SelectedImage.FirstTwoDigits
                : null;

            for (var i = 1; i < Layers.Count; i++)
            {
                Layers[i].FilterNumberHead2 = head2; // null の場合は解除
                if (!string.IsNullOrEmpty(lastLoadedDirectory))
                {
                    Layers[i].LoadFromDirectory(lastLoadedDirectory);
                }
            }
        }
    }
}