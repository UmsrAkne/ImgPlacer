using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace ImgPlacer.ViewModels.Xml
{
    public class XmlChildNodeViewModel
    {
        public XmlChildNodeViewModel(XElement element)
        {
            Source = element;

            // animationChain のみ内部に animation を持つので展開
            if (Name == "animationChain")
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
            => Name == "text"
                ? $"text: {Source.Attribute("string")?.Value}"
                : Name;

        public ObservableCollection<XmlChildNodeViewModel> Children { get; } = new ();
    }
}