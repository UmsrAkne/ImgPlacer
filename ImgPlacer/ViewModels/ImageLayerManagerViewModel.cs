using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageLayerManagerViewModel : BindableBase
    {
        private ObservableCollection<ImageListViewModel> layers = new ();
        private string lastLoadedDirectory;
        private int columnsCount;
        private bool isFourthColumnVisible;

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

        /// <summary>
        /// 画像リストを横一列に並べる際の列数を表します。
        ///
        /// UniformGrid は要素を単純に非表示（Visibility.Collapsed）にしても
        /// レイアウト上のセルを保持してしまうため、スペースが詰まらず
        /// 「空の4番目の欄」が残ってしまいます。
        ///
        /// ColumnsCount はこの問題を回避するための “レイアウト制御用プロパティ”です。
        /// 列数は以下の通りにセットされます。
        ///
        /// - IsFourthColumnVisible が true の場合: 4列
        /// - false の場合: 3列
        ///
        /// これにより、4番目のリストを疑似的に非表示として扱いながらも、
        /// 全体のレイアウトは自然に詰まった状態を維持できます。
        /// </summary>
        public int ColumnsCount { get => columnsCount; private set => SetProperty(ref columnsCount, value); }

        public ToolPanelContext ToolPanelContext { private get; set; }

        public bool IsFourthColumnVisible
        {
            get => isFourthColumnVisible;
            set
            {
                SetProperty(ref isFourthColumnVisible, value);
                ColumnsCount = value ? 4 : 3;
            }
        }

        public AsyncRelayCommand<string> LoadImagesCommand => new (async (param) =>
        {
            lastLoadedDirectory = param;
            foreach (var layer in Layers)
            {
                await layer.LoadFromDirectory(param);
            }

            // 初期読み込み後にAの選択に応じてB~Dを再絞り込み
            await ApplyPrimarySelectionFilterToOtherLayers();

            // 想定しているアプリの横幅が [1280 - 1680] であるため、その範囲のサイズの画像を検索してそれを採用する。
            var leadingImage =
                Layers
                    .FirstOrDefault()?
                    .Images
                    .Where(img => img.LeadingLetter == "A")
                    .FirstOrDefault(img => img.Thumbnail.Width is >= 1280 and <= 1680);

            if (leadingImage != null)
            {
                ToolPanelContext.ImageCanvasViewerViewModel.CanvasWidth = leadingImage.Thumbnail.Width;
            }
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

        private async void OnLayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ImageListViewModel.SelectedImage))
            {
                await ApplyPrimarySelectionFilterToOtherLayers();
                ToolPanelContext.ImageCanvasViewerViewModel.ImageWidth
                    = PrimaryLayer.SelectedImage?.Thumbnail.Width ?? 0;

                ToolPanelContext.ImageCanvasViewerViewModel.ImageHeight
                    = PrimaryLayer.SelectedImage?.Thumbnail.Height ?? 0;
            }
        }

        private async Task ApplyPrimarySelectionFilterToOtherLayers()
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
                var l = Layers[i];
                l.SelectedImage = null;
            }

            for (var i = 1; i < Layers.Count; i++)
            {
                Layers[i].FilterNumberHead2 = head2; // null の場合は解除
                if (!string.IsNullOrEmpty(lastLoadedDirectory))
                {
                    await Layers[i].LoadFromDirectory(lastLoadedDirectory);
                }
            }
        }
    }
}