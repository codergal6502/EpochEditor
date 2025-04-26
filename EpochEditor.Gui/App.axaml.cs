using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using EpochEditor.Gui.ViewModels;
using EpochEditor.Gui.Views;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using EpochEditor.SramUtilities;
using System.Collections.Generic;
using System.IO;
using System;

namespace EpochEditor.Gui;

public partial class App : Application
{
    private IStorageProvider? _storageProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private ApplicationViewModel? _applicationViewModel;
    private MainWindow? _mainWindow;

    public override void OnFrameworkInitializationCompleted()
    {
        this._applicationViewModel = new();
        
        this.DataContext = _applicationViewModel;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = _mainWindow = new MainWindow
            {
                DataContext = _applicationViewModel.MainWindowViewModel,
            };

            this._storageProvider = (TopLevel.GetTopLevel(desktop.MainWindow) ?? throw new EpochEditorAvaloniaSetupException()).StorageProvider;

            // var tl = TopLevel.GetTopLevel(desktop.MainWindow);
            // applicationViewModel.StorageProvider = (tl ?? throw new System.Exception()).StorageProvider;
        }

        base.OnFrameworkInitializationCompleted();
    }

    // private MainWindowViewModel MainWindowViewModel { get => (_applicationViewModel ?? throw new EpochEditorException("").MainWindowViewModel; }
    

    public void FileOpen_OnClick(object? sender, System.EventArgs args) {
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
                    MainWindowViewModel mainWindowViewModel = (this._applicationViewModel ?? throw new EpochEditorAvaloniaSetupException()).MainWindowViewModel;
                    mainWindowViewModel.Sram = sr.ReadBytes(File.ReadAllBytes(f.Path.LocalPath));
                    this._applicationViewModel.CurrentPath = f.Path.LocalPath;
                }
            }
        });
    }

    public void FileSave_OnClick(object? sender, System.EventArgs args) {
        if (null != this._applicationViewModel?.CurrentPath && null != this._applicationViewModel?.MainWindowViewModel?.Sram) {
            File.WriteAllBytes(
                this._applicationViewModel?.CurrentPath ?? throw new EpochEditorAvaloniaSetupException()
              , this._applicationViewModel?.MainWindowViewModel?.Sram.RawBytes ?? throw new EpochEditorAvaloniaSetupException()
            );
        }
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}