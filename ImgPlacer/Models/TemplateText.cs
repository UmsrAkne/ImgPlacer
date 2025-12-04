using ImgPlacer.Enums;
using Prism.Mvvm;

namespace ImgPlacer.Models
{
    public class TemplateText : BindableBase
    {
        public string Text { get; set; } = string.Empty;

        public string ShortcutLabel { get; set; } = string.Empty;

        public TemplateType TemplateType { get; set; }
    }
}