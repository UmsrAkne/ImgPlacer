using System;
using System.Drawing;
using System.Globalization;
using System.Xml.Linq;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class XElementInputPanelViewModel : BindableBase
    {
        private string inputText;
        private readonly ImageCanvasViewerViewModel imageCanvasViewerViewModel;

        public XElementInputPanelViewModel(ImageCanvasViewerViewModel canvasVm)
        {
            imageCanvasViewerViewModel = canvasVm;
        }

        public string InputText { get => inputText; set => SetProperty(ref inputText, value); }

        public DelegateCommand ApplyImageInfoCommand => new DelegateCommand(() =>
        {
            if (string.IsNullOrWhiteSpace(InputText))
            {
                return;
            }

            try
            {
                var xElement = XElement.Parse(InputText);
                var point = new Point(
                    GetValueFromXElement(xElement, "x", 0, int.Parse),
                    GetValueFromXElement(xElement, "y", 0, int.Parse));

                var scale =
                    GetValueFromXElement(xElement, "scale", 1.0, s => double.Parse(s, CultureInfo.InvariantCulture));

                imageCanvasViewerViewModel.SetCenteredOffset(point.X, point.Y, scale);
            }
            catch (Exception e)
            {
                Console.WriteLine("XElementInputPanelViewModel: XElement への変換に失敗しました。");
                Console.WriteLine(e);
                throw;
            }
        });

        private T GetValueFromXElement<T>(XElement xElement, string attributeName, T defaultValue, Func<string, T> parser)
        {
            if (!xElement.HasAttributes)
            {
                return defaultValue;
            }

            var attr = xElement.Attribute(attributeName)?.Value;
            if (string.IsNullOrEmpty(attr))
            {
                return defaultValue;
            }

            try
            {
                return parser(attr);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}