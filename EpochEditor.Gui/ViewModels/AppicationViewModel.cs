using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using EpochEditor.SramUtilities;

namespace EpochEditor.Gui.ViewModels {
    public class ApplicationViewModel : ObservableObject {
        private String? _currentPath;

        public ApplicationViewModel() {
            this.MainWindowViewModel = new MainWindowViewModel();
        }

        public MainWindowViewModel MainWindowViewModel { get; set; }
        
        public String? CurrentPath { get => this._currentPath; set => this._currentPath = value; }
    }
}