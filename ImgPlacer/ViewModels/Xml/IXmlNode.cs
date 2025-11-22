using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace ImgPlacer.ViewModels.Xml
{
    public interface IXmlNode
    {
        string Name { get; }

        string DisplayName { get; }

        public IXmlNode Parent { get; }

        XElement Source { get; }

        ObservableCollection<IXmlNode> Children { get; }

        void LoadChildren();

        void MoveChild(int oldIndex, int moveCount);
    }
}