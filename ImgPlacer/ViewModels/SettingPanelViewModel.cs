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
        private bool isInvertY;

        public SettingPanelViewModel(ToolPanelContext context)
        {
            TemplateTexts = new ObservableCollection<TemplateText>(AppSettings.LoadOrDefault().Templates);
            toolPanelContext = context;
            AddDefaultTemplatesIfEmpty();
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

        public bool IsInvertY { get => isInvertY; set => SetProperty(ref isInvertY, value); }

        public DelegateCommand ToggleExpandedCommand => new(() =>
        {
            IsExpanded = !IsExpanded;
        });

        public DelegateCommand ToggleFourthLayerVisibilityCommand => new (() =>
        {
            toolPanelContext.ImageLayerManagerViewModel.IsFourthColumnVisible = VisibleFourthLayer;
        });

        public DelegateCommand SetDefaultTemplatesCommand => new DelegateCommand(() =>
        {
            TemplateTexts.Clear();
            AddDefaultTemplatesIfEmpty();
        });

        public DelegateCommand SetInvertYFlagCommand => new DelegateCommand(() =>
        {
            AppSettings.IsInvertY = IsInvertY;
            AppSettings.Save();
        });

        private AppSettings AppSettings { get; } = new AppSettings();

        private void AddDefaultTemplatesIfEmpty()
        {
            if (TemplateTexts.Count != 0)
            {
                return;
            }

            TemplateTexts.Add(new TemplateText
            {
                ShortcutLabel = "Ctrl + I",
                Text = @"<image a=""{{ a }}"" b=""{{ b }}""  c=""{{ c }}"" d=""{{ d }}"" x=""{{ x }}"" y=""{{ y }}""  scale=""{{ scale }}"" />",
                TemplateType = TemplateType.Image,
            });
            TemplateTexts.Add(new TemplateText
            {
                ShortcutLabel = "Ctrl + D",
                Text = @"<draw a=""{{ a }}"" b=""{{ b }}""  c=""{{ c }}"" d=""{{ d }}"" />",
                TemplateType = TemplateType.Draw,
            });
            TemplateTexts.Add(new TemplateText
            {
                ShortcutLabel = "Ctrl + S",
                Text = @"<slide duration=""{{ duration }}"" distance=""{{ distance }}""  degree=""{{ degree }}"" repeatCount=""{{ repeatCount }}"" />",
                TemplateType = TemplateType.Slide,
            });
        }
    }
}