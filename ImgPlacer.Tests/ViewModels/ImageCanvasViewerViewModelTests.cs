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
    }
}