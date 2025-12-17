using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ImgPlacer.Enums;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImageCanvasViewerViewModel : BindableBase
    {
        private double offsetX;
        private double offsetY;
        private double zoom = 1.0;
        private double imageWidth;
        private double imageHeight;
        private double canvasHeight = 720;
        private double canvasWidth;
        private ObservableCollection<ImageListViewModel> layers;
        private bool isInfoHighlighted;

        public DelegateCommand<ImageAnchor?> SetPositionCommand => new((param) =>
        {
            if (!param.HasValue)
            {
                throw new System.InvalidOperationException("param is null");
            }

            SetImagePosition(param.Value);
        });

        public ObservableCollection<ImageListViewModel> Layers
        {
            get => layers;
            set => SetProperty(ref layers, value);
        }

        public double OffsetX
        {
            get => offsetX;
            set
            {
                if (SetProperty(ref offsetX, value))
                {
                    RaisePropertyChanged(nameof(DisplayOffset));
                }
            }
        }

        public double OffsetY
        {
            get => offsetY;
            set
            {
                if (SetProperty(ref offsetY, value))
                {
                    RaisePropertyChanged(nameof(DisplayOffset));
                }
            }
        }

        public double Zoom
        {
            get => zoom;
            set
            {
                if (SetProperty(ref zoom, value))
                {
                    RaisePropertyChanged(nameof(DisplayOffset));
                }
            }
        }

        public double ImageWidth
        {
            get => imageWidth;
            set => SetProperty(ref imageWidth, value);
        }

        public double ImageHeight { get => imageHeight; set => SetProperty(ref imageHeight, value); }

        public double CanvasHeight { get => canvasHeight; set => SetProperty(ref canvasHeight, value); }

        public double CanvasWidth { get => canvasWidth; set => SetProperty(ref canvasWidth, value); }

        public bool IsInfoHighlighted
        {
            get => isInfoHighlighted;
            set => SetProperty(ref isInfoHighlighted, value);
        }

        public DelegateCommand ResetZoomCommand => new (() =>
        {
            Zoom = 1.0;
        });

        public DelegateCommand<ImageAnchor?> MoveImageCommand => new ((param) =>
        {
            if (!param.HasValue)
            {
                throw new InvalidOperationException("param is null");
            }

            int dx;
            int dy;
            const int step = 40;

            switch (param.Value)
            {
                case ImageAnchor.Top:
                    dx = 0;
                    dy = CalculateNextSlideStep(step, (int)OffsetY, (int)CanvasHeight * -1, 0);
                    break;

                case ImageAnchor.Left:
                    dx = CalculateNextSlideStep(step, (int)OffsetX, 0, (int)CanvasWidth);
                    dy = 0;
                    break;

                case ImageAnchor.Right:
                    dx = CalculateNextSlideStep(step * -1, (int)(OffsetX + (ImageWidth * Zoom)), (int)CanvasWidth, 0);
                    dy = 0;
                    break;

                case ImageAnchor.Bottom:
                    dx = 0;
                    dy = CalculateNextSlideStep(step * -1, (int)(OffsetY + (ImageHeight * Zoom)), 0, (int)CanvasHeight);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            OffsetX += dx;
            OffsetY += dy;
        });

        public Point DisplayOffset => GetCenteredOffset();

        public void SetImagePosition(ImageAnchor anchor)
        {
            if (anchor == ImageAnchor.Center)
            {
                SetCenteredOffset(0, 0, Zoom);
                return;
            }

            // 画像サイズ（ズーム後）
            var w = ImageWidth * Zoom;
            var h = ImageHeight * Zoom;

            // Anchor に対応する「画像中心位置」を決める
            var targetCenterX = anchor switch
            {
                ImageAnchor.Left or ImageAnchor.TopLeft or ImageAnchor.BottomLeft
                    => w * 0.5,                         // 左側
                ImageAnchor.Right or ImageAnchor.TopRight or ImageAnchor.BottomRight
                    => CanvasWidth - (w * 0.5),         // 右側
                _ => CanvasWidth / 2.0,
            };

            var targetCenterY = anchor switch
            {
                ImageAnchor.Top or ImageAnchor.TopLeft or ImageAnchor.TopRight
                    => h * 0.5,                         // 上側
                ImageAnchor.Left or ImageAnchor.Right
                    => CanvasHeight / 2.0,              // 中央
                ImageAnchor.Bottom or ImageAnchor.BottomLeft or ImageAnchor.BottomRight
                    => CanvasHeight - (h * 0.5),        // 下側
                _ => CanvasHeight / 2.0,
            };

            // 変更する前に現在の位置を取得
            var oldX = OffsetX;
            var oldY = OffsetY;

            // 既存メソッドで位置確定
            SetCenteredOffset(
                targetCenterX - (CanvasWidth / 2.0),
                targetCenterY - (CanvasHeight / 2.0),
                Zoom);

            // 上下左右が指定された場合、他軸は動かしたくないので復元する。
            if (anchor is ImageAnchor.Top or ImageAnchor.Bottom)
            {
                OffsetX = oldX;
            }

            if (anchor is ImageAnchor.Left or ImageAnchor.Right)
            {
                OffsetY = oldY;
            }
        }

        public Point GetCenteredOffset()
        {
            var canvasCenter = new Point(CanvasWidth / 2.0, CanvasHeight / 2.0);
            var imageCenter = new Point((ImageWidth * Zoom) / 2.0, (ImageHeight * Zoom) / 2.0);
            var offset = new Point(OffsetX, OffsetY);

            return new Point(
                offset.X + imageCenter.X - canvasCenter.X,
                offset.Y + imageCenter.Y - canvasCenter.Y);
        }

        public void SetCenteredOffset(double centeredX, double centeredY, double scale)
        {
            Zoom = scale;
            OffsetX = centeredX + (CanvasWidth / 2.0) - (ImageWidth * scale / 2.0);
            OffsetY = centeredY + (CanvasHeight / 2.0) - (ImageHeight * scale / 2.0);
        }

        public int CalculateNextSlideStep(int proposedStep, int currentPosition, int oppositeBoundary, int limitBoundary)
        {
            if (limitBoundary == currentPosition)
            {
                return (((currentPosition / proposedStep) + 1) * proposedStep) - currentPosition;
            }

            var left = Math.Max(oppositeBoundary, limitBoundary);
            var right = Math.Min(oppositeBoundary, limitBoundary);
            var destPos = currentPosition + proposedStep;

            if (!IsBetween(currentPosition, right, left))
            {
                // 区間外 → 区間内に突っ込む場合は
                // どちらの境界に近いかで吸着先を決定
                if (IsBetween(destPos, right, left))
                {
                    var distLimit = limitBoundary - currentPosition;
                    var distOpp = oppositeBoundary - currentPosition;

                    // 最近傍の境界へ吸着
                    return Math.Abs(distLimit) <= Math.Abs(distOpp)
                        ? distLimit
                        : distOpp;
                }

                return proposedStep;
            }

            if (!IsBetween(currentPosition + proposedStep, right, left))
            {
                return limitBoundary - currentPosition;
            }

            return proposedStep;

            bool IsBetween(int value, int min, int max) => value >= min && value <= max;
        }

        public ImageCanvasViewerViewModel GetClone()
        {
            var cloneList = Layers.Select(i => i.GetClone()).ToList();
            var vm = new ImageCanvasViewerViewModel
            {
                OffsetX = OffsetX,
                OffsetY = OffsetY,
                Zoom = Zoom,
                ImageWidth = ImageWidth,
                ImageHeight = ImageHeight,
                CanvasHeight = CanvasHeight,
                CanvasWidth = CanvasWidth,
                Layers = new ObservableCollection<ImageListViewModel>(cloneList),
            };

            return vm;
        }
    }
}