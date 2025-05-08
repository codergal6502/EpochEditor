using System.Collections;
using System.Runtime.InteropServices;
using Avalonia.Input;

namespace EpochEditor.Gui.ViewModels;

public class PlatformUtilities {

    static PlatformUtilities() {
        Instance = new PlatformUtilities();
    }

    public static PlatformUtilities Instance { get; }

    private PlatformUtilities() {
        IsMac       = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        IsWindows   = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        IsLinux     = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }

    public bool IsMenuInWindow { get => IsWindows || IsLinux; }
    public bool HasApplicationMenu { get => IsMac; }
    public bool IsMac { get; }
    public bool IsWindows { get; }
    public bool IsLinux { get; }

    public KeyGesture MakeGesture(Key key) {
        return new KeyGesture(key, KeyModifiers.Meta);
    }
}