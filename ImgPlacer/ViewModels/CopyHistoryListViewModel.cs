using System.Collections.ObjectModel;
using ImgPlacer.Enums;
using ImgPlacer.Utils;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels;

public class CopyHistoryListViewModel : BindableBase, IToolPanelViewModel
{
    private bool isExpanded;

    public CopyHistoryListViewModel(XElementInputPanelViewModel xElementInputPanelViewModel)
    {
        XElementInputPanelViewModel = xElementInputPanelViewModel;
    }

    public ObservableCollection<ImageCanvasViewerViewModel> CopyHistories { get; } = new ();

    public SideBarPanelKind PanelKind => SideBarPanelKind.CopyHistory;

    public bool IsExpanded { get => isExpanded; set => SetProperty(ref isExpanded, value); }

    public DelegateCommand<ImageCanvasViewerViewModel> RestoreHistoryCommand => new DelegateCommand<ImageCanvasViewerViewModel>((param) =>
    {
        if (param == null)
        {
            return;
        }

        var x = TagGenerator.GenerateAnimeTag(param);
        var temp = XElementInputPanelViewModel.InputText;
        XElementInputPanelViewModel.InputText = x.ToString();
        XElementInputPanelViewModel.ApplyImageInfoCommand.Execute();
        XElementInputPanelViewModel.InputText = temp;
    });

    private XElementInputPanelViewModel XElementInputPanelViewModel { get; }
}