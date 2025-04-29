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
                        SramReader sr = new SramReader();
                        mainWindowViewModel.Sram = sr.ReadBytes(File.ReadAllBytes(f.Path.LocalPath));
                        mainWindowViewModel.CurrentPath = f.Path.LocalPath;
                    }
                }
            });
        }
    }

    public void FileSave_OnClick(object? sender, System.EventArgs args) {
        if (this.DataContext is MainWindowViewModel mainWindowViewModel) {
            if (null != mainWindowViewModel?.CurrentPath && null != mainWindowViewModel.Sram) {
                File.WriteAllBytes(
                    mainWindowViewModel.CurrentPath ?? throw new EpochEditorAvaloniaSetupException($"Cannot save with null {nameof(mainWindowViewModel.CurrentPath)}")
                  , mainWindowViewModel.Sram?.RawBytes ?? throw new EpochEditorAvaloniaSetupException($"Cannot save with null {nameof(mainWindowViewModel.Sram.RawBytes)} or null {nameof(mainWindowViewModel.Sram.RawBytes)}.")
                );
            }
        }
    }
}