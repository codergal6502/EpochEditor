
using ReactiveUI;

namespace EpochEditor.Gui.ViewModels;

public partial class AppViewModel : ReactiveObject
{
    public PlatformUtilities PlatformUtilities { get => PlatformUtilities.Instance; }
}