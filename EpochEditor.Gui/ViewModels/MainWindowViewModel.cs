using System;
using System.Collections.Generic;
using EpochEditor.SramUtilities;
using ReactiveUI;

namespace EpochEditor.Gui.ViewModels;

public partial class MainWindowViewModel : ReactiveObject
{
    private string _currentPath = String.Empty;

    public MainWindowViewModel()
    {
        SramViewModel = new SramReactiveViewModel();
    }

    public SramReactiveViewModel SramViewModel { get; }
    
    public Sram? Sram { get => SramViewModel.Sram; set => SramViewModel.Sram = value; }
    public String CurrentPath { get => _currentPath; set { this.RaiseAndSetIfChanged(ref _currentPath, value); this.RaisePropertyChanged(nameof(HasFileBeenLoaded)); } }
    public bool HasFileBeenLoaded { get => !String.IsNullOrWhiteSpace(CurrentPath); } 
}