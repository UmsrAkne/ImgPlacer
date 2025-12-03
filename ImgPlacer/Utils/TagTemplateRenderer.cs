using ImgPlacer.Models;
using Scriban;

namespace ImgPlacer.Utils
{
    public static class TagTemplateRenderer
    {
        public static string Render(string template, TemplateModel model)
        {
            var tpl = Template.Parse(template);
            return tpl.Render(model);
        }
    }
}