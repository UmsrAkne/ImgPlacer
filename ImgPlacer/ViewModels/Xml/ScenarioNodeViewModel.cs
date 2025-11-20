using System.Collections.ObjectModel;
using System.Xml.Linq;
using ImgPlacer.Utils;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels.Xml
{
    /// <summary>
    /// scenario 要素を表す ViewModel（XMLエディタのメイン単位）
    /// </summary>
    public class ScenarioNodeViewModel : BindableBase, IXmlNode
    {
        public ScenarioNodeViewModel(XElement scenarioElement)
        {
            Source = scenarioElement;

            // 初期ロード
            LoadChildren();
        }

        /// <summary>
        /// 対応する XElement（scenario）
        /// </summary>
        public XElement Source { get; }

        /// <summary>
        /// 表示用の名前（例：scenario の text があればその文字列）
        /// </summary>
        public string DisplayName => Source.Element("text")?.Attribute("string")?.Value ?? "scenario";

        public string Name => Source.Name.ToString();

        /// <summary>
        /// scenario 直下の子ノード（text,  animation,  animationChain）
        /// </summary>
        public ObservableCollection<XmlChildNodeViewModel> Children { get; } = new ();

        /// <summary>
        /// XML の子要素 → ViewModel の Children に再投影
        /// （挿入・削除後はこれを呼べば UI と同期できる）
        /// </summary>
        public void LoadChildren()
        {
            Children.Clear();

            foreach (var childVm in XmlNodeBuilder.BuildScenarioChildren(Source))
            {
                Children.Add(childVm);
            }

            RaisePropertyChanged(nameof(DisplayName));
        }
    }
}