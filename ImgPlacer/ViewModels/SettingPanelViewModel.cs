using ImgPlacer.Enums;
using ImgPlacer.Utils;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    public class SettingPanelViewModel : BindableBase, IToolPanelViewModel
    {
        private readonly ToolPanelContext toolPanelContext;
        private bool isExpanded;

        public SettingPanelViewModel(ToolPanelContext context)
        {
            toolPanelContext = context;
        }

        public AppSettings AppSettings { get; } = new AppSettings();

        public SideBarPanelKind PanelKind { get; } = SideBarPanelKind.Setting;

        public bool IsExpanded { get => isExpanded; set => SetProperty(ref isExpanded, value); }

        public DelegateCommand ToggleExpandedCommand => new(() =>
        {
            IsExpanded = !IsExpanded;
        });
    }
}