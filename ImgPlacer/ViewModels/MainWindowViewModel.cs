using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using ImgPlacer.Enums;
using ImgPlacer.Utils;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly ToolPanelContext context;
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

        context = new ToolPanelContext
        {
            ImageLayerManagerViewModel = ImageLayerManagerViewModel,
            ImageCanvasViewerViewModel = ImageCanvasViewerViewModel,
            CanvasSliderPanelViewModel = CanvasSliderPanelViewModel,
            XElementInputPanelViewModel = XElementInputPanelViewModel,
            CopyHistoryListViewModel = CopyHistoryListViewModel,
        };

        XmlEditorPanelViewModel = new XmlEditorPanelViewModel(context);
        SettingPanelViewModel = new SettingPanelViewModel(context);
        ImageLayerManagerViewModel.ToolPanelContext = context;

        context.XmlEditorPanelViewModel = XmlEditorPanelViewModel;
        context.SettingPanelViewModel = SettingPanelViewModel;

        ToolPanelViewModelCollection.Add(CanvasSliderPanelViewModel);
        ToolPanelViewModelCollection.Add(XElementInputPanelViewModel);
        ToolPanelViewModelCollection.Add(CopyHistoryListViewModel);
        ToolPanelViewModelCollection.Add(XmlEditorPanelViewModel);
        ToolPanelViewModelCollection.Add(SettingPanelViewModel);

        App.Settings.ToolPanelContext = context;

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

    public SettingPanelViewModel SettingPanelViewModel { get; private set; }

    public ObservableCollection<IToolPanelViewModel> ToolPanelViewModelCollection { get; set; } = new ();

    public DelegateCommand<string> CopyTagCommand => new ((param) =>
    {
        var toType = param switch
            {
                "animation-image" => TemplateType.Image,
                "animation-draw" => TemplateType.Draw,
                "animation-slide" => TemplateType.Slide,
                _ => TemplateType.Image,
            };

        var model = TagTemplateService.CreateModel(toType, context, SettingPanelViewModel.IsInvertY);
        var template = SettingPanelViewModel.TemplateTexts.FirstOrDefault(tpl => tpl.TemplateType == toType);
        var text = TagTemplateService.Render(template?.Text, model);

        if (text != null)
        {
            Clipboard.SetText(text);
            CopyHistoryListViewModel.CopyHistories.Add(ImageCanvasViewerViewModel.GetClone());
        }
    });

    [Conditional("DEBUG")]
    private void SetDebugData()
    {
    }
}