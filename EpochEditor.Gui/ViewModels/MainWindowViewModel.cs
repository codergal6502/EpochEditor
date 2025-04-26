using System.Collections.Generic;
using EpochEditor.SramUtilities;

namespace EpochEditor.Gui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        SramViewModel = new SramReactiveViewModel();
    }

    public SramReactiveViewModel SramViewModel { get; }
    
    public Sram? Sram { get => SramViewModel.Sram; set => SramViewModel.Sram = value; }
}