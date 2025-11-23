using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using ImgPlacer.Enums;
using ImgPlacer.ViewModels;

namespace ImgPlacer.Utils
{
    /// <summary>
    /// ユーティリティ: ImageCanvasViewerViewModel からアニメタグ(XElement)を生成します。
    /// </summary>
    public static class TagGenerator
    {
        /// <summary>
        /// 指定の ViewModel から次の形式の XElement を生成します。
        ///
        /// <anime name="image" a="name1" b="name2" c="name3" d="name4" x="0" y="0" scale="1.0" />
        ///
        /// a-d は Layers の各 SelectedImage の FileName（未選択は空文字）。
        /// x, y は vm.GetCenteredOffset() の値。
        /// scale は vm.Zoom。
        /// </summary>
        /// <param name="vm">
        /// タグ生成のソース。
        /// </param>
        /// <returns>
        /// 入力したオブジェクトから作った image タグ。
        /// </returns>
        public static XElement GenerateAnimeTag(ImageCanvasViewerViewModel vm)
        {
            if (vm == null)
            {
                throw new ArgumentNullException(nameof(vm));
            }

            // 4つ分を準備（レイヤーが不足している場合は空文字）
            var names = new string[4];
            for (var i = 0; i < names.Length; i++)
            {
                names[i] = string.Empty;
            }

            if (vm.Layers != null)
            {
                for (var i = 0; i < Math.Min(4, vm.Layers.Count); i++)
                {
                    var layer = vm.Layers[i];
                    var fileName = layer?.SelectedImage?.FileName ?? string.Empty;
                    names[i] = fileName;
                }
            }

            var centered = vm.GetCenteredOffset();

            // x, y は整数文字列。四捨五入（MidpointRounding.AwayFromZero）し、不変カルチャで出力。
            var xRounded = Math.Round(centered.X, 0, MidpointRounding.AwayFromZero);
            var yRounded = Math.Round(centered.Y, 0, MidpointRounding.AwayFromZero);
            var xStr = xRounded.ToString("0", CultureInfo.InvariantCulture);
            var yStr = yRounded.ToString("0", CultureInfo.InvariantCulture);

            // scale は最低 1 桁の小数を付与（例: 1.0）
            var scaleStr = vm.Zoom.ToString("0.0########", CultureInfo.InvariantCulture);

            var element = new XElement(
                nameof(XmlTagName.Anime).ToLower(),
                new XAttribute("name", nameof(AnimationName.Image).ToLower()),
                new XAttribute("a", names.ElementAtOrDefault(0) ?? string.Empty),
                new XAttribute("b", names.ElementAtOrDefault(1) ?? string.Empty),
                new XAttribute("c", names.ElementAtOrDefault(2) ?? string.Empty),
                new XAttribute("d", names.ElementAtOrDefault(3) ?? string.Empty),
                new XAttribute("x", xStr),
                new XAttribute("y", yStr),
                new XAttribute("scale", scaleStr));

            return element;
        }

        /// <summary>
        /// GenerateAnimeTag を内部で実行し、属性を削って name を "draw" にして返します。
        /// 返却形式の例:
        /// <anime name="draw" b="name2" c="name3" d="name4" />
        /// </summary>
        /// <param name="vm">
        /// タグの生成元の viewModel.
        /// </param>
        /// <returns>
        /// vm から生成した draw タグ.
        /// </returns>
        public static XElement GenerateDrawTag(ImageCanvasViewerViewModel vm)
        {
            var el = GenerateAnimeTag(vm);

            // name を draw に変更
            var nameAttr = el.Attribute("name");
            if (nameAttr != null)
            {
                nameAttr.Value = nameof(AnimationName.Draw).ToLower();
            }
            else
            {
                el.Add(new XAttribute("name", nameof(AnimationName.Draw).ToLower()));
            }

            // 残すのは b, c, d のみ。その他を削除
            var allowed = new[] { "name", "b", "c", "d", };
            var toRemove = el.Attributes().Where(a => !allowed.Contains(a.Name.LocalName)).ToList();
            foreach (var a in toRemove)
            {
                a.Remove();
            }

            return el;
        }
    }
}