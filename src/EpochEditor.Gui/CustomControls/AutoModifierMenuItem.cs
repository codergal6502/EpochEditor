using Avalonia.Input;
using EpochEditor.Gui.ViewModels;

namespace EpochEditor.Gui.CustomControls;

/// <summary>
/// Automatically adds Meta+ (i.e., command) to the gesture for Macs or
/// Control+ for Windows and Linux.
/// </summary>
public class AutoModifierMenuItem : Avalonia.Controls.NativeMenuItem {

    public KeyGesture? AutoModifierGesture {
        get { return base.Gesture; }
        set {
            if (value is KeyGesture g) {
                var platformDefaultModifier = 
                    PlatformUtilities.Instance.IsMac     ? KeyModifiers.Meta
                  : PlatformUtilities.Instance.IsLinux   ? KeyModifiers.Control
                  : PlatformUtilities.Instance.IsWindows ? KeyModifiers.Control
                  : KeyModifiers.Control; // Not sure when this would happen, but defaulting to control on Mac is less chaotic than defaulting to meta on Linux or Windows.
                
                base.Gesture = new KeyGesture(g.Key, g.KeyModifiers | platformDefaultModifier);
            }
            else {
                base.Gesture = value;
            }
        }
    }
}
