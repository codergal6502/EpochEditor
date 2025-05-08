# Epoch Editor

Epoch Editor is a cross-platform GUI editor for Chrono Trigger SNES Save RAM files.

## Description

Epoch Editor is a cross-platform GUI editor for Chrono Trigger SNES Save RAM files. It's also a learning project for CoderGal6502! It allows you to edit every character's name and statistics as well as your inventory.

### Console Version

The console version is currently deprioritized and may be deprecated in the future.

## Getting Started

### Dependencies

* If you are using the ARM64 Mac binary download or the Linux x64 binary, there should be no dependencies.
* Everyone else needs .NET 9.0 for now, including if you are downloading the Windows executable.

#### Buliding Releases
* To build Mac or Linux releases, you'll need `xmllint`.
* To build Linux releases, you'll need [`appimagetool-x86_64.AppImage`](https://github.com/AppImage/appimagetool/releases/download/continuous/appimagetool-x86_64.AppImage) in your `PATH`, e.g., in `$HOME/bin`.

### Installing

* You can currently download binaries:
   * for Mac (in a `.dmg` disk image),
   * for Linux (as an AppImage), and
   * for Windows (in a ZIP file).

### Executing program

* If you're on an ARM64 Mac, you can download the binary. You may need to allow "Gatekeeper" to launch the application or need to dequarantine the application:
   * Execute `xattr -dr com.apple.quarantine path/to/EpochEditor.app`, replacing the sample path to the actual path to your downloaded copy of EpochEditor.
* If you're on x64 Linux, you can download the AppImage.
* If you're on Windows, you can download the ZIP file. You'll need to install [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) if it's not already installed.
   * No testing on Windows has been done; only cursory testing has been done using WINE.
* You can also run the project from source:
```
dotnet run --project src/EpochEditor.Gui/EpochEditor.Gui.csproj
```

## Help

Feel free to post in [Discussions](https://github.com/codergal6502/EpochEditor/discussions).

## Authors

* [CoderGal6502](https://github.com/codergal6502)

## Version History

* 0.1.1
    * Binary for Linux x64 released; "About" dialog implemented.
    * Executable for Windows released.
* 0.1.0
    * Binary for macOS ARM64 released.

## License

This project is licensed under the [GNU GENERAL PUBLIC LICENSE Version 3] License - see the [LICENSE](https://github.com/codergal6502/EpochEditor/blob/main/LICENSE) file for details.

## Acknowledgments

* [DomPizzie's README Template](https://gist.github.com/DomPizzie/7a5ff55ffa9081f2de27c315f5018afc)
* [mcred's Chrono Trigger SNES Save File Editor](https://github.com/mcred/chrono-trigger-save-editor)
* [Data Crystal](https://datacrystal.tcrf.net/w/index.php?title=Chrono_Trigger_(SNES)/RAM_map)
