using System;
using System.Windows;
using System.Windows.Controls;
using ImgPlacer.ViewModels.Xml;

namespace ImgPlacer.Views.Selectors
{
    public class AttributeEditorTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextTemplate { get; set; }

        public DataTemplate BoolTemplate { get; set; }

        public DataTemplate IntTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var attr = (XmlAttributeViewModel)item;

            if (attr == null)
            {
                return TextTemplate;
            }

            if (attr.ValueType == typeof(bool))
            {
                return BoolTemplate;
            }

            if (attr.ValueType == typeof(int))
            {
                return IntTemplate;
            }

            return TextTemplate;
        }
    }
}