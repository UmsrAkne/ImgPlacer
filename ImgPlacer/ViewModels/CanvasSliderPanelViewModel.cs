using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
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
        private bool isExpanded;
        private string preset;
        private bool isRepeatCustom;

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

        public DelegateCommand<int?> AddDegreeCommand => new ((param) =>
        {
            if (!param.HasValue)
            {
                throw new InvalidOperationException("param is null");
            }

            Degree = (Degree + param.Value) % 360;
        });

        public SideBarPanelKind PanelKind => SideBarPanelKind.CanvasSlider;

        public bool IsExpanded { get => isExpanded; set => SetProperty(ref isExpanded, value); }

        public bool IsRepeatCustom { get => isRepeatCustom; set => SetProperty(ref isRepeatCustom, value); }

        public string RepeatPreset
        {
            get => preset;
            set
            {
                preset = value;
                RaisePropertyChanged();

                IsRepeatCustom = value.EndsWith("Custom");
                var cnt = value.Substring(value.Length - 1, 1);

                RepeatCount = cnt switch
                {
                    "0" => 0,
                    "1" => 1,
                    "∞" => 999,   // or int.MaxValue
                    _ => RepeatCount,
                };
            }
        }

        public DelegateCommand ToggleExpandedCommand => new (() =>
        {
            IsExpanded = !IsExpanded;
        });

        public static float CalcDistance(Point originPoint, int degree, Rect wallRect)
        {
            var distances = new float[4];
            var walls = new List<(Point start, Point end)>
            {
                (new Point(wallRect.Left,  wallRect.Top),    new Point(wallRect.Right, wallRect.Top)),    // 上
                (new Point(wallRect.Right, wallRect.Top),    new Point(wallRect.Right, wallRect.Bottom)), // 右
                (new Point(wallRect.Right, wallRect.Bottom), new Point(wallRect.Left,  wallRect.Bottom)), // 下
                (new Point(wallRect.Left,  wallRect.Bottom), new Point(wallRect.Left,  wallRect.Top)),     // 左
            };

            var vec = new Vector2((float)originPoint.X, (float)originPoint.Y);
            var rad = -degree * MathF.PI / 180f;

            for (var i = 0; i < walls.Count; i++)
            {
                var wall = walls[i];
                distances[i] = RaycastToSegment(vec, rad, ToVec(wall.start), ToVec(wall.end));
            }

            if (distances.All(d => Math.Abs(d) <= 1e-6f))
            {
                return 0;
            }

            return distances.Where(d => Math.Abs(d) >= 1e-6f).Min();

            Vector2 ToVec(Point p) => new((float)p.X, (float)p.Y);
        }

        public static float RaycastToSegment(Vector2 rayOrigin, float rayAngleRad, Vector2 wallA, Vector2 wallB)
        {
            // レイ方向（正規化）
            var rayDir = new Vector2(MathF.Cos(rayAngleRad), MathF.Sin(rayAngleRad));

            // 壁の方向
            var wallDir = wallB - wallA;

            // A - P
            var ap = wallA - rayOrigin;

            var determinant = Cross(rayDir, wallDir);

            // determinant は rayDir から見た wallDir の傾きを表す。
            // determinant == 0 の場合、 レイから見て壁が傾いていない(平行)
            if (MathF.Abs(determinant) < 1e-6f)
            {
                return 0f; // 平行の場合は距離は出ないので 0
            }

            var t = Cross(ap, wallDir) / determinant;
            var u = Cross(ap, rayDir) / determinant;

            // レイ前方 & 壁の線分内
            if (t >= 0f && u is >= 0f and <= 1f)
            {
                return t; // rayDir は長さ1なので t = 距離
            }

            return 0f;
        }

        private static float Cross(Vector2 a, Vector2 b)
        {
            return (a.X * b.Y) - (a.Y * b.X); // 2D クロス積（Z成分）
        }
    }
}