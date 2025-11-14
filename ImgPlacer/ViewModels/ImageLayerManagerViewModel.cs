using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageLayerManagerViewModel : BindableBase
    {
        private ObservableCollection<ImageListViewModel> layers = new ();

        public ObservableCollection<ImageListViewModel> Layers
        {
            get => layers;
            set
            {
                if (SetProperty(ref layers, value))
                {
                    RaisePropertyChanged(nameof(PrimaryLayer));
                    RaisePropertyChanged(nameof(LoadImagesCommand));
                }
            }
        }

        public ImageListViewModel PrimaryLayer => Layers[0];

        public DelegateCommand<string> LoadImagesCommand => new DelegateCommand<string>((param) =>
        {
            foreach (var layer in Layers)
            {
                layer.LoadFromDirectory(param);
            }
        });
    }
}