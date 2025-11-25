using System;
using System.Xml.Linq;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels.Xml
{
    public class XmlAttributeViewModel : BindableBase
    {
        private readonly XElement sourceXElement;
        private readonly XAttribute sourceXAttribute;
        private object val;

        public XmlAttributeViewModel(XElement xElem, XAttribute xAttr)
        {
            sourceXElement = xElem;
            sourceXAttribute = xAttr;
            Name = xAttr.Name.LocalName;

            // ここで型推論
            if (bool.TryParse(xAttr.Value, out var b))
            {
                ValueType = typeof(bool);
                Value = b;
            }
            else if (int.TryParse(xAttr.Value, out var i))
            {
                ValueType = typeof(int);
                Value = i;
            }
            else
            {
                ValueType = typeof(string);
                Value = xAttr.Value;
            }
        }

        public string Name { get; }

        public Type ValueType { get; }

        public object Value
        {
            get => val;
            set
            {
                if (SetProperty(ref val, value))
                {
                    // 属性値の書き戻し
                    sourceXElement.SetAttributeValue(sourceXAttribute.Name, value?.ToString());
                }
            }
        }
    }
}