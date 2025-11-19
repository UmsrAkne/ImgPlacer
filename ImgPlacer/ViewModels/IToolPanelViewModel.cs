using ImgPlacer.Enums;

namespace ImgPlacer.ViewModels
{
    public interface IToolPanelViewModel
    {
        SideBarPanelKind PanelKind { get; }

        bool IsExpanded { get; set; }
    }
}