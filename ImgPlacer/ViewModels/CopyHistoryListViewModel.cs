using System.Collections.ObjectModel;
using ImgPlacer.Utils;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels;

public class CopyHistoryListViewModel : BindableBase
{
    public CopyHistoryListViewModel(XElementInputPanelViewModel xElementInputPanelViewModel)
    {
        XElementInputPanelViewModel = xElementInputPanelViewModel;
    }

    public ObservableCollection<ImageCanvasViewerViewModel> CopyHistories { get; } = new ();

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