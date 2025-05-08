using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EpochEditor.Gui.Views;

public partial class AboutEpochEditorWindow : Window {

    public AboutEpochEditorWindow() {
        this.InitializeComponent();
    }

    public void UrlButton_OnClick(Object? sender, RoutedEventArgs args) {
        var url = "https://github.com/codergal6502/EpochEditor";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
    }
}