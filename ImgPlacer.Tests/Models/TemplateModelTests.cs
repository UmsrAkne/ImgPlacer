using ImgPlacer.Models;
using ImgPlacer.ViewModels;

namespace ImgPlacer.Tests.Models
{
    [TestFixture]
    public class TemplateModelTests
    {
        [Test]
        public void ToTemplateModel_MapsLayerFileNamesAndViewerState()
        {
            var layerManager = new ImageLayerManagerViewModel
            {
                Layers =
                [
                    new () { SelectedImage = new ImageItem { FileName = "A.png", }, },
                    new () { SelectedImage = new ImageItem { FileName = "B.png", }, },
                    new () { SelectedImage = new ImageItem { FileName = "C.png", }, },
                    new () { SelectedImage = new ImageItem { FileName = "D.png", }, },
                ],
            };

            var viewer = new ImageCanvasViewerViewModel
            {
                CanvasWidth = 800,
                CanvasHeight = 600,
                ImageWidth = 200,
                ImageHeight = 100,
                Zoom = 2.0,
                OffsetX = 10.0,
                OffsetY = -20.0,
            };

            var context = new ToolPanelContext
            {
                ImageLayerManagerViewModel = layerManager,
                ImageCanvasViewerViewModel = viewer,
            };

            var model = TemplateModel.ToTemplateModel(context);

            var centered = viewer.GetCenteredOffset();
            var expectedX = (int)centered.X; // ToTemplateModel casts to int (truncate)
            var expectedY = (int)centered.Y;

            Assert.Multiple(() =>
            {
                Assert.That(model.A, Is.EqualTo("A.png"));
                Assert.That(model.B, Is.EqualTo("B.png"));
                Assert.That(model.C, Is.EqualTo("C.png"));
                Assert.That(model.D, Is.EqualTo("D.png"));
                Assert.That(model.X, Is.EqualTo(expectedX));
                Assert.That(model.Y, Is.EqualTo(expectedY));
                Assert.That(model.Scale, Is.EqualTo(viewer.Zoom));
            });
        }
    }
}