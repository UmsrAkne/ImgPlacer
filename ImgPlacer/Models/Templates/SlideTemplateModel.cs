using ImgPlacer.ViewModels;

namespace ImgPlacer.Models.Templates
{
    public class SlideTemplateModel : ITemplateModel
    {
        public int Duration { get; set; }

        public int Distance { get; set; }

        public int Degree { get; set; }

        public int RepeatCount { get; set; }

        public static SlideTemplateModel ToTemplateModel(ToolPanelContext panelContext)
        {
            var model = new SlideTemplateModel
            {
                Duration = panelContext.CanvasSliderPanelViewModel.Duration,
                Distance = panelContext.CanvasSliderPanelViewModel.Distance,
                Degree = panelContext.CanvasSliderPanelViewModel.Degree,
                RepeatCount = panelContext.CanvasSliderPanelViewModel.RepeatCount,
            };

            return model;
        }
    }
}