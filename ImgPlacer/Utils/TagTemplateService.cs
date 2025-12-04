using System;
using ImgPlacer.Enums;
using ImgPlacer.Models;
using ImgPlacer.Models.Templates;
using ImgPlacer.ViewModels;
using Scriban;

namespace ImgPlacer.Utils
{
    public static class TagTemplateService
    {
        public static string Render(string template, ITemplateModel model)
        {
            var tpl = Template.Parse(template);
            return tpl.Render(model, m => string.Concat(m.Name[..1].ToLower(), m.Name.AsSpan(1)));
        }

        public static ITemplateModel CreateModel(TemplateType type, ToolPanelContext context)
        {
            return type switch
            {
                TemplateType.Image => TemplateModel.ToTemplateModel(context),
                TemplateType.Draw => DrawTemplateModel.ToTemplateModel(context),
                TemplateType.Slide => SlideTemplateModel.ToTemplateModel(context),
                _ => throw new ArgumentException("未定義の TemplateType が入力されました。"),
            };
        }
    }
}