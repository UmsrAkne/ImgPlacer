using System;
using ImgPlacer.Enums;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CanvasSliderPanelViewModel : BindableBase, IToolPanelViewModel
    {
        private int distance;
        private int degree;
        private int duration;
        private int repeatCount;
        private int delay;
        private int interval;

        private bool isHolding;
        private double originalOffsetX;
        private double originalOffsetY;

        public CanvasSliderPanelViewModel(ImageCanvasViewerViewModel imageCanvasViewerViewModel)
        {
            ImageCanvasViewerViewModel = imageCanvasViewerViewModel;
        }

        public ImageCanvasViewerViewModel ImageCanvasViewerViewModel { get; }

        public int Distance { get => distance; set => SetProperty(ref distance, value); }

        public int Degree { get => degree; set => SetProperty(ref degree, value); }

        public int Duration { get => duration; set => SetProperty(ref duration, value); }

        public int RepeatCount { get => repeatCount; set => SetProperty(ref repeatCount, value); }

        public int Delay { get => delay; set => SetProperty(ref delay, value); }

        public int Interval { get => interval; set => SetProperty(ref interval, value); }

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

        // 押している間だけ移動させ、離したら元の座標に戻す
        public DelegateCommand SlideHoldPressCommand => new DelegateCommand(() =>
        {
            if (isHolding)
            {
                return;
            }

            // 安全確保：Distance, Degree が有効値かチェック
            if (double.IsNaN(Distance) || double.IsNaN(Degree))
            {
                return;
            }

            isHolding = true;

            // 現在位置を保存
            originalOffsetX = ImageCanvasViewerViewModel.OffsetX;
            originalOffsetY = ImageCanvasViewerViewModel.OffsetY;

            // 角度をラジアンに変換
            var rad = Degree * Math.PI / 180.0;

            // 移動量（1回分）
            var dx = Distance * Math.Cos(rad);
            var dy = Distance * Math.Sin(rad);

            // 一時的な位置へ移動
            ImageCanvasViewerViewModel.OffsetX = originalOffsetX + dx;
            ImageCanvasViewerViewModel.OffsetY = originalOffsetY + dy;
        });

        public DelegateCommand SlideHoldReleaseCommand => new DelegateCommand(() =>
        {
            if (!isHolding)
            {
                return;
            }

            // 元の位置へ戻す
            ImageCanvasViewerViewModel.OffsetX = originalOffsetX;
            ImageCanvasViewerViewModel.OffsetY = originalOffsetY;
            isHolding = false;
        });

        public SideBarPanelKind PanelKind => SideBarPanelKind.CanvasSlider;
    }
}