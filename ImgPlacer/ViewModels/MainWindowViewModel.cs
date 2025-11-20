using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Xml.Linq;
using ImgPlacer.Utils;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private string title = "ImgPlacer";

    public MainWindowViewModel()
    {
        ImageLayerManagerViewModel = new ImageLayerManagerViewModel();
        ImageCanvasViewerViewModel = new ImageCanvasViewerViewModel();
        CanvasSliderPanelViewModel = new CanvasSliderPanelViewModel(ImageCanvasViewerViewModel);
        var layers = new ObservableCollection<ImageListViewModel>
        {
            // A, B, C, D の4リストを作成し、各レイヤーに先頭文字フィルタを設定
            new () { FilterPrefix = "A", },
            new () { FilterPrefix = "B", },
            new () { FilterPrefix = "C", },
            new () { FilterPrefix = "D", },
        };

        ImageLayerManagerViewModel.Layers = layers;
        ImageCanvasViewerViewModel.Layers = layers;
        XElementInputPanelViewModel = new XElementInputPanelViewModel(ImageCanvasViewerViewModel);
        CopyHistoryListViewModel = new CopyHistoryListViewModel(XElementInputPanelViewModel);
        XmlEditorPanelViewModel = new XmlEditorPanelViewModel();

        ToolPanelViewModelCollection.Add(CanvasSliderPanelViewModel);
        ToolPanelViewModelCollection.Add(XElementInputPanelViewModel);
        ToolPanelViewModelCollection.Add(CopyHistoryListViewModel);
        ToolPanelViewModelCollection.Add(XmlEditorPanelViewModel);

        SetDebugData();
    }

    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    public ImageLayerManagerViewModel ImageLayerManagerViewModel { get; private set; }

    public ImageCanvasViewerViewModel ImageCanvasViewerViewModel { get; private set; }

    public CanvasSliderPanelViewModel CanvasSliderPanelViewModel { get; private set; }

    public XElementInputPanelViewModel XElementInputPanelViewModel { get; private set; }

    public CopyHistoryListViewModel CopyHistoryListViewModel { get; private set; }

    public XmlEditorPanelViewModel XmlEditorPanelViewModel { get; private set; }

    public ObservableCollection<IToolPanelViewModel> ToolPanelViewModelCollection { get; set; } = new ();

    public DelegateCommand<string> CopyTagCommand => new ((param) =>
    {
        XElement text = null;
        switch (param)
        {
            case "animation-image":
                text = TagGenerator.GenerateAnimeTag(ImageCanvasViewerViewModel);
                break;
            case "animation-draw":
                text = TagGenerator.GenerateDrawTag(ImageCanvasViewerViewModel);
                break;
        }

        if (text != null)
        {
            Clipboard.SetText(text.ToString());
            CopyHistoryListViewModel.CopyHistories.Add(ImageCanvasViewerViewModel.GetClone());
        }
    });

    [Conditional("DEBUG")]
    private void SetDebugData()
    {
    }
}