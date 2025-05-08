using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using EpochEditor.Gui.ViewModels;
using EpochEditor.SramUtilities;

namespace EpochEditor.Gui.Views;

public partial class MainWindow : Window
{
    private IStorageProvider _storageProvider;

    public MainWindow()
    {
        InitializeComponent();
        this._storageProvider = (TopLevel.GetTopLevel(this) ?? throw new EpochEditorAvaloniaSetupException()).StorageProvider;
    }

    public void FileOpen_OnClick(object? sender, System.EventArgs args) {
        if (this.DataContext is MainWindowViewModel mainWindowViewModel) {
            var ofp = (this._storageProvider ?? throw new EpochEditorAvaloniaSetupException()).OpenFilePickerAsync(new FilePickerOpenOptions {
                AllowMultiple = false
              , FileTypeFilter = [ new FilePickerFileType("SNES Save RAM File") { Patterns = [ "*.srm" ] }]
              , Title = "Open SNES Save RAM File…"
            });

            ofp.ContinueWith(afpTask => {
                if (afpTask.IsCompletedSuccessfully) {
                    IReadOnlyList<IStorageFile> r = afpTask.Result;
                    
                    if (r.Count > 1) { throw new EpochEditorException($"Expected at most one file but got {r.Count} files."); }

                    var f = r.SingleOrDefault();
                    if (null != f) {
                        mainWindowViewModel.LoadSram(f.Path.LocalPath);
                    }
                }
            });
        }
    }

    public void FileSave_OnClick(object? sender, System.EventArgs args) {
        if (this.DataContext is MainWindowViewModel mainWindowViewModel) {
            mainWindowViewModel.SaveToCurrentPath();
        }
    }

    public void FileSaveAs_OnClick(Object? sender, System.EventArgs args) {
        if (this.DataContext is MainWindowViewModel mainWindowViewModel) {
            var sfp = (this._storageProvider ?? throw new EpochEditorAvaloniaSetupException()).SaveFilePickerAsync(new FilePickerSaveOptions {
                FileTypeChoices = [ new FilePickerFileType("SNES Save RAM File") { Patterns = [ "*.srm" ] }]
              , Title = "Save SNES Save RAM File…"
            });

            sfp.ContinueWith(afpTask => {
                if (afpTask.IsCompletedSuccessfully) {
                    if (afpTask.Result is IStorageFile file) {
                        
                        mainWindowViewModel.SaveToNewPathPath(file.Path.LocalPath);
                    }
                }
            });
        }
    }

    private void FileAbout_OnClick(Object? sender, EventArgs args) {
        var dialog = new AboutEpochEditorWindow();
        dialog.DataContext = new AboutEpochEditorViewModel();
	
        dialog.ShowDialog(this);
    }
    private void FilePreferences_OnClick(Object? sender, EventArgs args) {
        
    }

    public void FileExit_OnClick(Object? sender, System.EventArgs args) {
        if (Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktopApplication) {
            desktopApplication.Shutdown();
        }
    }
}
