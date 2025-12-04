using ImgPlacer.ViewModels;

namespace ImgPlacer.Models.Templates
{
    public class DrawTemplateModel : ITemplateModel
    {
        public string A { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        public static DrawTemplateModel ToTemplateModel(ToolPanelContext panelContext)
        {
            var model = new DrawTemplateModel();
            var layers = panelContext.ImageLayerManagerViewModel.Layers;
            model.A = layers[0].SelectedImage?.FileName;
            model.B = layers[1].SelectedImage?.FileName;
            model.C = layers[2].SelectedImage?.FileName;
            model.D = layers[3].SelectedImage?.FileName;

            return model;
        }
    }
}