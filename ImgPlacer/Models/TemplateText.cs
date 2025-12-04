using ImgPlacer.Enums;
using Prism.Mvvm;

namespace ImgPlacer.Models
{
    public class TemplateText : BindableBase
    {
        private string text = string.Empty;
        private string shortcutLabel = string.Empty;
        private TemplateType templateType;

        public string Text { get => text; set => SetProperty(ref text, value); }

        public string ShortcutLabel { get => shortcutLabel; set => SetProperty(ref shortcutLabel, value); }

        public TemplateType TemplateType { get => templateType; set => SetProperty(ref templateType, value); }
    }
}