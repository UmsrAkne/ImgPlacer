using System.Xml.Linq;
using ImgPlacer.Enums;
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
            set => SetProperty(ref loadedDocument, value);
        }

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