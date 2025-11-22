using System.Collections.ObjectModel;
using System.Xml.Linq;
using ImgPlacer.Enums;
using ImgPlacer.ViewModels.Xml;

namespace ImgPlacer.Utils
{
    public static class XmlNodeBuilder
    {
        public static ObservableCollection<XmlChildNodeViewModel> BuildChildren(XElement source)
        {
            var list = new ObservableCollection<XmlChildNodeViewModel>();

            foreach (var child in source.Elements())
            {
                list.Add(new XmlChildNodeViewModel(child));
            }

            return list;
        }

        public static ObservableCollection<XmlChildNodeViewModel> BuildScenarioChildren(XElement scenario)
        {
            var list = new ObservableCollection<XmlChildNodeViewModel>();

            foreach (var child in scenario.Elements())
            {
                if (child.Name.LocalName == nameof(XmlTagName.Text).ToLower())
                {
                    continue;
                }

                list.Add(new XmlChildNodeViewModel(child));
            }

            return list;
        }
    }
}