using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace EpochEditor.Gui.ViewModels;

public class AboutEpochEditorViewModel : ReactiveObject {
    private static Version? _version;
    private static String? _fileVersion;
    private static String? _informationalVersion;

    static AboutEpochEditorViewModel() {
        // See https://stackoverflow.com/a/65062/1102726
        // See https://stackoverflow.com/a/7770603/1102726
        // See https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.application.productversion?view=windowsdesktop-9.0
        // See https://github.com/dotnet/sdk/issues/39214#issuecomment-2024568399
        // See https://stackoverflow.com/a/7770165/1102726

        Assembly? assembly = Assembly.GetEntryAssembly();
        _version = assembly?.GetName()?.Version;
        _fileVersion = (assembly?.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false) as AssemblyFileVersionAttribute[])?.FirstOrDefault()?.Version;
        _informationalVersion = (assembly?.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false) as AssemblyInformationalVersionAttribute[])?.FirstOrDefault()?.InformationalVersion;
    }

    public String? VersionString => _informationalVersion;
}
