using System;
using ImgPlacer.Enums;
using Prism.Commands;
using Prism.Mvvm;

namespace ImgPlacer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CursorPadContainerViewModel : BindableBase
    {
        public ToolPanelContext ToolPanelContext { private get; set; }

        public DelegateCommand<ImageAnchor?> SetPositionCommand => new ((param) =>
        {
            if (!param.HasValue)
            {
                throw new InvalidOperationException("param is null");
            }

            ToolPanelContext.ImageCanvasViewerViewModel.SetImagePosition(param.Value);
        });
    }
}