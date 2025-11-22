using System.Collections.ObjectModel;
using System.Xml.Linq;
using ImgPlacer.Enums;
using ImgPlacer.Utils;

namespace ImgPlacer.ViewModels.Xml
{
    public class XmlChildNodeViewModel : IXmlNode
    {
        public XmlChildNodeViewModel(XElement element)
        {
            Source = element;

            // animationChain のみ内部に animation を持つので展開
            if (Name == nameof(AnimationName.AnimationChain).ToLower())
            {
                foreach (var child in element.Elements())
                {
                    Children.Add(new XmlChildNodeViewModel(child));
                }
            }
        }

        public XElement Source { get; }

        public string Name => Source.Name.LocalName;

        public string DisplayName
            => Name == nameof(XmlTagName.Text).ToLower()
                ? $"text: {Source.Attribute("string")?.Value}"
                : Name;

        public ObservableCollection<XmlChildNodeViewModel> Children { get; } = new ();

        public void LoadChildren()
        {
            Children.Clear();

            foreach (var childVm in XmlNodeBuilder.BuildChildren(Source))
            {
                Children.Add(childVm);
            }
        }
    }
}