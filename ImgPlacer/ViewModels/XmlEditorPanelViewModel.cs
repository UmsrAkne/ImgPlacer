using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
        private IXmlNode selectedItem;

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

        /// <summary>
        /// TreeView の選択アイテムを保持（ScenarioNodeViewModel または XmlChildNodeViewModel）
        /// </summary>
        public IXmlNode SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public SideBarPanelKind PanelKind => SideBarPanelKind.XmlEditor;

        public bool IsExpanded
        {
            get => isExpanded;
            set => SetProperty(ref isExpanded, value);
        }

        public DelegateCommand PasteCommand => new DelegateCommand(() =>
        {
            if (SelectedItem == null || !CanInsertTo(SelectedItem))
            {
                return;
            }

            ExecutePaste();
            XElement TryGetClipboardXElement()
            {
                if (!Clipboard.ContainsText())
                {
                    return null;
                }

                var text = Clipboard.GetText();

                try
                {
                    return XElement.Parse(text);
                }
                catch
                {
                    // XML として解釈できなかった
                    return null;
                }
            }

            bool CanInsertTo(IXmlNode node)
            {
                var name = node.Name;

                return name is "scenario" or "animationChain";
            }

            void ExecutePaste()
            {
                var insertTarget = SelectedItem.Source;
                var targetElementName = insertTarget.Name.LocalName;
                var newElement = TryGetClipboardXElement();

                if (targetElementName == "scenario")
                {
                    // <scenario>の子として追加（末尾）
                    insertTarget.Add(newElement);
                }
                else if (targetElementName == "animationChain")
                {
                    // animationChain の最後に追加
                    insertTarget.Add(newElement);
                }
                else
                {
                    Console.WriteLine("このノードには挿入できません。");
                }
            }
        });

        public DelegateCommand ToggleExpandedCommand => new(() =>
        {
            IsExpanded = !IsExpanded;
        });
    }
}