using System;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CanvasSliderPanelViewModel : BindableBase
    {
        private int distance;
        private int degree;
        private int duration;

        public CanvasSliderPanelViewModel(ImageCanvasViewerViewModel imageCanvasViewerViewModel)
        {
            ImageCanvasViewerViewModel = imageCanvasViewerViewModel;
        }

        public ImageCanvasViewerViewModel ImageCanvasViewerViewModel { get; }

        public int Distance { get => distance; set => SetProperty(ref distance, value); }

        public int Degree { get => degree; set => SetProperty(ref degree, value); }

        public int Duration { get => duration; set => SetProperty(ref duration, value); }

        public DelegateCommand SlideCommand => new DelegateCommand(() =>
        {
            // 安全確保：Distance, Degree が有効値かチェック（負でもOK）
            if (double.IsNaN(Distance) || double.IsNaN(Degree))
            {
                return;
            }

            // 角度をラジアンに変換
            var rad = Degree * Math.PI / 180.0;

            // 移動量
            var dx = Distance * Math.Cos(rad);
            var dy = Distance * Math.Sin(rad);

            // Viewer へ適用（相対移動）
            ImageCanvasViewerViewModel.OffsetX += dx;
            ImageCanvasViewerViewModel.OffsetY += dy;
        });

        public DelegateCommand SlideHoldCommand => new DelegateCommand(() =>
        {
            Console.WriteLine("Slide Command Executed");
        });
    }
}