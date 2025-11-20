using System.Collections.ObjectModel;
using System.Xml.Linq;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels.Xml
{
    /// <summary>
    /// scenario 要素を表す ViewModel（XMLエディタのメイン単位）
    /// </summary>
    public class ScenarioNodeViewModel : BindableBase
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

            foreach (var child in Source.Elements())
            {
                // text は scenario のヘッダーに値が表示されているため不要
                if (child.Name.LocalName == "text")
                {
                    continue;
                }

                Children.Add(new XmlChildNodeViewModel(child));
            }

            RaisePropertyChanged(nameof(DisplayName));
        }
    }
}