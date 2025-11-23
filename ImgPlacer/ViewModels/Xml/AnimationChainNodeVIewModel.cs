using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Prism.Commands;

namespace ImgPlacer.ViewModels.Xml
{
    public class AnimationChainNodeViewModel : IXmlNode
    {
        public AnimationChainNodeViewModel(XElement element, IXmlNode parent)
        {
            Source = element;
            Parent = parent;

            foreach (var child in element.Elements())
            {
                Children.Add(new XmlChildNodeViewModel(child, this));
            }
        }

        public IXmlNode Parent { get; }

        public XElement Source { get; }

        public string Name => Source.Name.LocalName;

        public string DisplayName => "animationChain";

        public ObservableCollection<IXmlNode> Children { get; } = new ();

        public DelegateCommand<int?> MoveElementCommand => new ((moveDirection) =>
        {
            if (moveDirection.HasValue)
            {
                var i = Parent.Children.IndexOf(this);
                Parent.MoveChild(i, i + moveDirection.Value);
            }
        });

        public void LoadChildren()
        {
        }

        public void MoveChild(int oldIndex, int newIndex)
        {
            if (newIndex < 0 || newIndex >= Children.Count)
            {
                return;
            }

            // UI
            Children.Move(oldIndex, newIndex);

            // XML
            var xmlList = Source.Elements().ToList();
            var moving = xmlList[oldIndex];
            moving.Remove();

            if (newIndex >= xmlList.Count)
            {
                Source.Add(moving);
            }
            else
            {
                xmlList[newIndex].AddBeforeSelf(moving);
            }
        }
    }
}