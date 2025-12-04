using ImgPlacer.ViewModels;

namespace ImgPlacer.Models.Templates
{
    public class SlideTemplateModel : ITemplateModel
    {
        public int Duration { get; set; }

        public int Distance { get; set; }

        public int Degree { get; set; }

        public int RepeatCount { get; set; }

        /// <summary>
        /// 表示仕様に従ってフォーマット済みのスケール値。
        /// 0.01 単位まで扱い、小数点以下 2 桁で固定した文字列として出力する。
        /// Scriban テンプレートへ直接渡すため double 型では保持しない。
        /// </summary>
        public string Scale { get; set; }

        public static SlideTemplateModel ToTemplateModel(ToolPanelContext panelContext)
        {
            var model = new SlideTemplateModel
            {
                Duration = panelContext.CanvasSliderPanelViewModel.Duration,
                Distance = panelContext.CanvasSliderPanelViewModel.Distance,
                Degree = panelContext.CanvasSliderPanelViewModel.Degree,
            };

            return model;
        }
    }
}