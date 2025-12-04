using System.Collections.ObjectModel;
using ImgPlacer.Enums;
using ImgPlacer.Models;
using ImgPlacer.Utils;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    public class SettingPanelViewModel : BindableBase, IToolPanelViewModel
    {
        private readonly ToolPanelContext toolPanelContext;
        private bool isExpanded;
        private bool visibleFourthLayer = true;
        private double canvasWidth;

        public SettingPanelViewModel(ToolPanelContext context)
        {
            toolPanelContext = context;
        }

        public double CanvasWidth
        {
            get => toolPanelContext.ImageCanvasViewerViewModel.CanvasWidth;
            set
            {
                if (SetProperty(ref canvasWidth, value))
                {
                    AppSettings.CanvasWidth = value;
                    toolPanelContext.ImageCanvasViewerViewModel.CanvasWidth = value;
                }
            }
        }

        public ObservableCollection<TemplateText> TemplateTexts { get; set; }

        public SideBarPanelKind PanelKind { get; } = SideBarPanelKind.Setting;

        public bool IsExpanded { get => isExpanded; set => SetProperty(ref isExpanded, value); }

        public bool VisibleFourthLayer
        {
            get => visibleFourthLayer;
            set => SetProperty(ref visibleFourthLayer, value);
        }

        public DelegateCommand ToggleExpandedCommand => new(() =>
        {
            IsExpanded = !IsExpanded;
        });

        public DelegateCommand ToggleFourthLayerVisibilityCommand => new (() =>
        {
            toolPanelContext.ImageLayerManagerViewModel.IsFourthColumnVisible = VisibleFourthLayer;
        });

        private AppSettings AppSettings { get; } = new AppSettings();
    }
}