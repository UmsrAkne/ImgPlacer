using System.Windows;
using ImgPlacer.ViewModels;

namespace ImgPlacer.Tests.ViewModels
{
    [TestFixture]
    public class CanvasSlidePanelViewModelTests
    {
        // 壁：100x100 の矩形
        private static Rect Wall => new (0, 0, 100, 100);

        [Test]
        public void Center_Right_HitsRightWall()
        {
            // 中心から右へ
            var origin = new Point(50, 50);
            var degree = 0; // 右

            var dist = CanvasSliderPanelViewModel.CalcDistance(origin, degree, Wall);

            Assert.That(dist, Is.EqualTo(50f).Within(0.001f));
        }

        [Test]
        public void Center_Left_HitsLeftWall()
        {
            var origin = new Point(50, 50);
            var degree = 180; // 左

            var dist = CanvasSliderPanelViewModel.CalcDistance(origin, degree, Wall);

            Assert.That(dist, Is.EqualTo(50f).Within(0.001f));
        }

        [Test]
        public void Center_Up_HitsTopWall()
        {
            var origin = new Point(50, 50);
            var degree = 90; // 上（※ Y反転補正込み）

            var dist = CanvasSliderPanelViewModel.CalcDistance(origin, degree, Wall);

            Assert.That(dist, Is.EqualTo(50f).Within(0.001f));
        }

        [Test]
        public void Center_Down_HitsBottomWall()
        {
            var origin = new Point(50, 50);
            var degree = -90; // 下

            var dist = CanvasSliderPanelViewModel.CalcDistance(origin, degree, Wall);

            Assert.That(dist, Is.EqualTo(50f).Within(0.001f));
        }

        [Test]
        public void RayAwayFromWall_ReturnsZero()
        {
            // 矩形の外から、さらに外へ
            var origin = new Point(-10, 50);
            var degree = 180; // 左

            var dist = CanvasSliderPanelViewModel.CalcDistance(origin, degree, Wall);

            Assert.That(dist, Is.EqualTo(0f));
        }

        [Test]
        public void Corner_45Degree_HitsOppositeCorner()
        {
            var wall = new Rect(0, 0, 100, 100);

            var origin = new Point(0, 0);
            var degree = -45; // 右下（Y反転補正込み）

            var dist = CanvasSliderPanelViewModel.CalcDistance(origin, degree, wall);

            var expected = MathF.Sqrt(100 * 100 + 100 * 100); // 100√2

            Assert.That(dist, Is.EqualTo(expected).Within(0.001f));
        }
    }
}