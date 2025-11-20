using System.Xml.Linq;

namespace ImgPlacer.ViewModels.Xml
{
    public interface IXmlNode
    {
        string Name { get; }

        string DisplayName { get; }

        XElement Source { get; }

        void LoadChildren();
    }
}