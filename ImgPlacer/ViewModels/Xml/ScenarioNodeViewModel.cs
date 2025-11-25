using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using ImgPlacer.Enums;
using ImgPlacer.Utils;
using Prism.Commands;
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

        public IXmlNode Parent { get; }

        /// <summary>
        /// 対応する XElement（scenario）
        /// </summary>
        public XElement Source { get; }

        /// <summary>
        /// 表示用の名前（例：scenario の text があればその文字列）
        /// </summary>
        public string DisplayName => Source.Element(nameof(XmlTagName.Text).ToLower())?.Attribute("string")?.Value ?? nameof(XmlTagName.Scenario).ToLower();

        public string Name => Source.Name.ToString();

        /// <summary>
        /// scenario 直下の子ノード（text,  animation,  animationChain）
        /// </summary>
        public ObservableCollection<IXmlNode> Children { get; } = new ();

        public DelegateCommand<int?> MoveElementCommand => new ((moveDirection) => { });

        /// <summary>
        /// XML の子要素 → ViewModel の Children に再投影
        /// （挿入・削除後はこれを呼べば UI と同期できる）
        /// </summary>
        public void LoadChildren()
        {
            Children.Clear();

            foreach (var childVm in XmlNodeBuilder.BuildScenarioChildren(Source, this))
            {
                Children.Add(childVm);
            }

            RaisePropertyChanged(nameof(DisplayName));
        }

        public void MoveChild(int oldIndex, int moveCount)
        {
            if (oldIndex == moveCount || oldIndex < 0 || oldIndex >= Children.Count || moveCount < 0 || moveCount >= Children.Count)
            {
                return;
            }

            // 1. UI の Children を並び替え
            Children.Move(oldIndex, moveCount);

            // 2. XML（Source）の子要素も並び替え
            var elements = Source.Elements().ToList();
            var moving = elements[oldIndex];

            // 一度 XML ツリーから外す
            moving.Remove();

            if (moveCount >= elements.Count)
            {
                // Index がサイズを超える場合は最後に追加
                Source.Add(moving);
            }
            else
            {
                var target = elements[moveCount];
                target.AddBeforeSelf(moving);
            }
        }

        public void PerformAction(ToolPanelContext context)
        {
            Console.WriteLine("ScenarioNodeViewModel.PerformAction() execute");
        }
    }
}