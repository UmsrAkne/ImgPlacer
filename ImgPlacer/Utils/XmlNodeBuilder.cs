using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using ImgPlacer.Enums;
using ImgPlacer.ViewModels.Xml;

namespace ImgPlacer.Utils
{
    public static class XmlNodeBuilder
    {
        public static ObservableCollection<IXmlNode> BuildChildren(XElement source, IXmlNode parent)
        {
            var list = new ObservableCollection<IXmlNode>();

            foreach (var child in source.Elements())
            {
                list.Add(new XmlChildNodeViewModel(child, parent));
            }

            return list;
        }

        public static ObservableCollection<IXmlNode> BuildScenarioChildren(XElement scenario, IXmlNode parent)
        {
            var list = new ObservableCollection<IXmlNode>();

            foreach (var child in scenario.Elements())
            {
                if (child.Name.LocalName == nameof(XmlTagName.Text).ToLower())
                {
                    continue;
                }

                if (child.Name.LocalName == nameof(AnimationName.AnimationChain).ToTopLower())
                {
                    list.Add(new AnimationChainNodeViewModel(child, parent));
                    continue;
                }

                list.Add(new XmlChildNodeViewModel(child, parent));
            }

            return list;
        }
    }
}