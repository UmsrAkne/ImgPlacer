using ImgPlacer.ViewModels;

namespace ImgPlacer.Tests.ViewModels
{
    [TestFixture]
    public class ImageCanvasViewerViewModelTests
    {
        [Test]
        public void GetCenteredOffset_Basic()
        {
            // Arrange
            var vm = new ImageCanvasViewerViewModel
            {
                CanvasWidth = 800,
                CanvasHeight = 600,
                ImageWidth = 400,
                ImageHeight = 200,
                Zoom = 2.0,
                OffsetX = 10,
                OffsetY = 20,
            };

            // Act
            var centered = vm.GetCenteredOffset();
            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(centered.X, Is.EqualTo(10).Within(1e-9));
                Assert.That(centered.Y, Is.EqualTo(-80).Within(1e-9));
            });
        }

        [Test]
        public void SetCenteredOffset_Basic_And_Invertibility()
        {
            // Arrange
            var vm = new ImageCanvasViewerViewModel
            {
                CanvasWidth = 800,
                CanvasHeight = 600,
                ImageWidth = 400,
                ImageHeight = 200,
            };

            double centeredX = 10;
            double centeredY = -80;
            var scale = 2.0;

            // Act
            vm.SetCenteredOffset(centeredX, centeredY, scale);
            Assert.Multiple(() =>
            {
                // Assert computed offsets and zoom
                Assert.That(vm.Zoom, Is.EqualTo(scale).Within(1e-9));
                Assert.That(vm.OffsetX, Is.EqualTo(10).Within(1e-9));
                Assert.That(vm.OffsetY, Is.EqualTo(20).Within(1e-9));
            });

            // And verify that GetCenteredOffset returns the original centered values
            var roundTrip = vm.GetCenteredOffset();
            Assert.Multiple(() =>
            {
                Assert.That(roundTrip.X, Is.EqualTo(centeredX).Within(1e-9));
                Assert.That(roundTrip.Y, Is.EqualTo(centeredY).Within(1e-9));
            });
        }

        private ImageCanvasViewerViewModel target;

        [SetUp]
        public void Setup()
        {
            target = new ImageCanvasViewerViewModel();
        }

        // -----------------------------
        // edge == currentPosition の場合
        // -----------------------------
        [TestCase(5, 10, 3, 10, ExpectedResult = (10 / 5 + 1) * 5 - 10)]
        [TestCase(4, 8, 99, 8, ExpectedResult = (8 / 4 + 1) * 4 - 8)]
        public int When_CurrentPositionEqualsEdge(int addition, int cp, int p1, int edge)
        {
            return target.CalculateNextSlideStep(addition, cp, p1, edge);
        }

        // -----------------------------
        // currentPosition が区間外 → addition を返す
        // -----------------------------
        [TestCase(5, 0, 10, 20, ExpectedResult = 5)]
        [TestCase(3, 50, 20, 10, ExpectedResult = 3)]
        public int When_CurrentPositionIsOutsideRange(int addition, int cp, int p1, int edge)
        {
            return target.CalculateNextSlideStep(addition, cp, p1, edge);
        }

        // -----------------------------
        // currentPosition が区間外だが、
        // addition を足すと区間内に入るケース
        // -----------------------------
        [TestCase(15, 0, 10, 20, ExpectedResult = 10 - 0)]
        [TestCase( -5, 25, 20, 10, ExpectedResult = -5)]
        public int When_OutsideRangeButNextStepEntersRange(int addition, int cp, int p1, int edge)
        {
            return target.CalculateNextSlideStep(addition, cp, p1, edge);
        }

        // -----------------------------
        // currentPosition は区間内だが、
        // currentPosition + addition が区間外
        // → currentPosition - edge を返す
        // -----------------------------
        [TestCase(2, 9, 5, 10, ExpectedResult = 10 - 9)]
        public int When_AdditionBreaksOutsideRange(int addition, int cp, int p1, int edge)
        {
            return target.CalculateNextSlideStep(addition, cp, p1, edge);
        }

        // -----------------------------
        // どちらも区間内 → addition
        // -----------------------------
        [TestCase(5, 15, 10, 20, ExpectedResult = 5)]
        [TestCase(3, 18, 25, 10, ExpectedResult = 3)]
        public int When_StayInsideRange(int addition, int cp, int p1, int edge)
        {
            return target.CalculateNextSlideStep(addition, cp, p1, edge);
        }
    }
}