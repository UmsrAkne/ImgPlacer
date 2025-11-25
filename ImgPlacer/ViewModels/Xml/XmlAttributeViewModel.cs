using System;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels.Xml
{
    public class XmlAttributeViewModel : BindableBase
    {
        private object val;

        public XmlAttributeViewModel(string name, string rawValue)
        {
            Name = name;

            // ここで型推論
            if (bool.TryParse(rawValue, out var b))
            {
                ValueType = typeof(bool);
                Value = b;
            }
            else if (int.TryParse(rawValue, out var i))
            {
                ValueType = typeof(int);
                Value = i;
            }
            else
            {
                ValueType = typeof(string);
                Value = rawValue;
            }
        }

        public string Name { get; }

        public Type ValueType { get; }

        public object Value { get => val; set => SetProperty(ref val, value); }
    }
}