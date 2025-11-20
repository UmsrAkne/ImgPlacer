using ImgPlacer.Enums;
using Prism.Commands;

namespace ImgPlacer.ViewModels
{
    public interface IToolPanelViewModel
    {
        SideBarPanelKind PanelKind { get; }

        bool IsExpanded { get; set; }

        DelegateCommand ToggleExpandedCommand { get; }
    }
}