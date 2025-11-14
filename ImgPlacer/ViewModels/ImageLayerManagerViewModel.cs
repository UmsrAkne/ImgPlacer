using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageLayerManagerViewModel : BindableBase
    {
        public ObservableCollection<ImageListViewModel> Layers { get; set; } = new ();

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