using ImgPlacer.ViewModels;

namespace ImgPlacer.Models
{
    public class TemplateModel
    {
        public string A { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public double Scale { get; set; }

        public static TemplateModel ToTemplateModel(ToolPanelContext panelContext)
        {
            var model = new TemplateModel();
            var layers = panelContext.ImageLayerManagerViewModel.Layers;
            model.A = layers[0].SelectedImage?.FileName;
            model.B = layers[1].SelectedImage?.FileName;
            model.C = layers[2].SelectedImage?.FileName;
            model.D = layers[3].SelectedImage?.FileName;
            model.X = (int)panelContext.ImageCanvasViewerViewModel.GetCenteredOffset().X;
            model.Y = (int)panelContext.ImageCanvasViewerViewModel.GetCenteredOffset().Y;
            model.Scale = panelContext.ImageCanvasViewerViewModel.Zoom;

            return model;
        }
    }
}