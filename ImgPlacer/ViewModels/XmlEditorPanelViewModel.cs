using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using ImgPlacer.Enums;
using ImgPlacer.ViewModels.Xml;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    public class XmlEditorPanelViewModel : BindableBase, IToolPanelViewModel
    {
        private bool isExpanded;
        private XDocument loadedDocument;

        public XDocument LoadedDocument
        {
            get => loadedDocument;
            set
            {
                SetProperty(ref loadedDocument, value);
                Scenarios.Clear();
                var scenarioList = value?.Root?.Elements("scenario").Select(el => new ScenarioNodeViewModel(el)).ToList();
                if (scenarioList != null)
                {
                    foreach (var scenario in scenarioList)
                    {
                        Scenarios.Add(scenario);
                    }
                }
            }
        }

        public ObservableCollection<ScenarioNodeViewModel> Scenarios { get; private set; } = new ();

        public SideBarPanelKind PanelKind => SideBarPanelKind.XmlEditor;

        public bool IsExpanded
        {
            get => isExpanded;
            set => SetProperty(ref isExpanded, value);
        }

        public DelegateCommand ToggleExpandedCommand => new(() =>
        {
            IsExpanded = !IsExpanded;
        });
    }
}