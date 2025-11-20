using ImgPlacer.Utils;
using ImgPlacer.ViewModels;

namespace ImgPlacer.Tests.Utils
{
    [TestFixture]
    public class TagGeneratorTests
    {
        [Test]
        public void GenerateAnimeTag_BasicWithFourLayers()
        {
            var vm = new ImageCanvasViewerViewModel
            {
                CanvasWidth = 800,
                CanvasHeight = 600,
                ImageWidth = 400,
                ImageHeight = 200,
                Zoom = 1.5,
                OffsetX = 5,
                OffsetY = -10,
                Layers = new System.Collections.ObjectModel.ObservableCollection<ImageListViewModel>
                {
                    new () { SelectedImage = new ImageItem { FileName = "a.png", }, },
                    new () { SelectedImage = new ImageItem { FileName = "b.png", }, },
                    new () { SelectedImage = new ImageItem { FileName = "c.png", }, },
                    new () { SelectedImage = new ImageItem { FileName = "d.png", }, },
                },
            };

            var el = TagGenerator.GenerateAnimeTag(vm);
            Assert.Multiple(() =>
            {
                Assert.That(el.Name.LocalName, Is.EqualTo("anime"));
                Assert.That(el.Attribute("name")?.Value, Is.EqualTo("image"));
                Assert.That(el.Attribute("a")?.Value, Is.EqualTo("a.png"));
                Assert.That(el.Attribute("b")?.Value, Is.EqualTo("b.png"));
                Assert.That(el.Attribute("c")?.Value, Is.EqualTo("c.png"));
                Assert.That(el.Attribute("d")?.Value, Is.EqualTo("d.png"));
            });

            // Verify x,y are integer strings rounded (四捨五入) using MidpointRounding.AwayFromZero
            var centered = vm.GetCenteredOffset();
            var expectedX = Math.Round(centered.X, 0, MidpointRounding.AwayFromZero);
            var expectedY = Math.Round(centered.Y, 0, MidpointRounding.AwayFromZero);

            var xAttr = el.Attribute("x")!.Value;
            var yAttr = el.Attribute("y")!.Value;
            Assert.Multiple(() =>
            {
                Assert.That(xAttr.Contains('.'), Is.False);
                Assert.That(yAttr.Contains('.'), Is.False);

                Assert.That(double.Parse(xAttr, System.Globalization.CultureInfo.InvariantCulture),
                    Is.EqualTo(expectedX).Within(1e-9));

                Assert.That(double.Parse(yAttr, System.Globalization.CultureInfo.InvariantCulture),
                    Is.EqualTo(expectedY).Within(1e-9));

                Assert.That(el.Attribute("scale")?.Value,
                    Is.EqualTo(vm.Zoom.ToString("0.0########", System.Globalization.CultureInfo.InvariantCulture)));
            });
        }

        [Test]
        public void GenerateAnimeTag_MissingLayersAndNulls()
        {
            var vm = new ImageCanvasViewerViewModel
            {
                CanvasWidth = 100,
                CanvasHeight = 100,
                ImageWidth = 10,
                ImageHeight = 10,
                Zoom = 1.0,
                OffsetX = 0,
                OffsetY = 0,
                Layers = new System.Collections.ObjectModel.ObservableCollection<ImageListViewModel>
                {
                    new () { SelectedImage = new ImageItem { FileName = "only.png", }, },
                    new (),
                },
            };

            var el = TagGenerator.GenerateAnimeTag(vm);
            Assert.Multiple(() =>
            {
                Assert.That(el.Attribute("a")?.Value, Is.EqualTo("only.png"));
                Assert.That(el.Attribute("b")?.Value, Is.EqualTo(""));
                Assert.That(el.Attribute("c")?.Value, Is.EqualTo(""));
                Assert.That(el.Attribute("d")?.Value, Is.EqualTo(""));
            });
        }

        [Test]
        public void GenerateDrawTag_UsesAnimeThenTrimsAndRenames()
        {
            var vm = new ImageCanvasViewerViewModel
            {
                Layers = new System.Collections.ObjectModel.ObservableCollection<ImageListViewModel>
                {
                    new () { SelectedImage = new ImageItem { FileName = "a.png", }, },
                    new () { SelectedImage = new ImageItem { FileName = "b.png", }, },
                    new () { SelectedImage = new ImageItem { FileName = "c.png", }, },
                    new () { SelectedImage = new ImageItem { FileName = "d.png", }, },
                },
            };

            var el = TagGenerator.GenerateDrawTag(vm);

            Assert.Multiple(() =>
            {
                Assert.That(el.Name.LocalName, Is.EqualTo("anime"));
                Assert.That(el.Attribute("name")?.Value, Is.EqualTo("draw"));

                // Only b, c, d remain (and name)
                Assert.That(el.Attribute("b")?.Value, Is.EqualTo("b.png"));
                Assert.That(el.Attribute("c")?.Value, Is.EqualTo("c.png"));
                Assert.That(el.Attribute("d")?.Value, Is.EqualTo("d.png"));

                Assert.That(el.Attribute("a"), Is.Null);
                Assert.That(el.Attribute("x"), Is.Null);
                Assert.That(el.Attribute("y"), Is.Null);
                Assert.That(el.Attribute("scale"), Is.Null);
            });
        }

        [Test]
        public void GenerateDrawTag_MissingLayers_YieldsEmptyStrings()
        {
            var vm = new ImageCanvasViewerViewModel
            {
                Layers = new System.Collections.ObjectModel.ObservableCollection<ImageListViewModel>
                {
                    new () { SelectedImage = new ImageItem { FileName = "onlyA.png", }, },
                    new (),
                },
            };

            var el = TagGenerator.GenerateDrawTag(vm);
            Assert.Multiple(() =>
            {
                Assert.That(el.Attribute("name")?.Value, Is.EqualTo("draw"));
                Assert.That(el.Attribute("b")?.Value, Is.EqualTo(""));
                Assert.That(el.Attribute("c")?.Value, Is.EqualTo(""));
                Assert.That(el.Attribute("d")?.Value, Is.EqualTo(""));
            });
        }
    }
}