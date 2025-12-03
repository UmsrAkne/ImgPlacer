using ImgPlacer.Models;
using ImgPlacer.Utils;

namespace ImgPlacer.Tests.Utils
{
    [TestFixture]
    public class TagTemplateRendererTests
    {
        [Test]
        public void Render_Basic()
        {
            var model = new TemplateModel
            {
                A = "a.png",
                B = "b.png",
                C = "c.png",
                D = "d.png",
                X = 10,
                Y = 20,
                Scale = "2.00",
            };

            var template =
                """<image a="{{ a }}" b="{{ b }}" c="{{ c }}" d="{{ d }}" x="{{ x }}" y="{{ y }}" scale="{{ scale }}">""";

            var act = TagTemplateRenderer.Render(template, model);
            Assert.That(act,
                Is.EqualTo("""<image a="a.png" b="b.png" c="c.png" d="d.png" x="10" y="20" scale="2.00">"""));
        }
    }
}