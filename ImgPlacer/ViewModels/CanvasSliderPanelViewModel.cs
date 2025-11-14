using System;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CanvasSliderPanelViewModel : BindableBase
    {
        private int distance;
        private int degree;
        private int duration;

        public CanvasSliderPanelViewModel(ImageCanvasViewerViewModel imageCanvasViewerViewModel)
        {
            ImageCanvasViewerViewModel = imageCanvasViewerViewModel;
        }

        public ImageCanvasViewerViewModel ImageCanvasViewerViewModel { get; }

        public int Distance { get => distance; set => SetProperty(ref distance, value); }

        public int Degree { get => degree; set => SetProperty(ref degree, value); }

        public int Duration { get => duration; set => SetProperty(ref duration, value); }

        public DelegateCommand SlideCommand => new DelegateCommand(() =>
        {
            Console.WriteLine("Slide Command Executed");
        });

        public DelegateCommand SlideHoldCommand => new DelegateCommand(() =>
        {
            Console.WriteLine("Slide Command Executed");
        });
    }
}