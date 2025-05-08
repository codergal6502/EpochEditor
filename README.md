# Epoch Editor

Epoch Editor is a cross-platform GUI editor for Chrono Trigger SNES Save RAM files.

## Description

Epoch Editor is a cross-platform GUI editor for Chrono Trigger SNES Save RAM files. It's also a learning project for CoderGal6502! It allows you to edit every character's name and statistics as well as your inventory.

### Console Version

The console version is currently deprioritized and may be deprecated in the future.

## Getting Started

### Dependencies

* If you are using the ARM64 Mac binary download, there should be no dependencies.
* Everyone else needs .NET 9.0 for now.

#### Buliding Releases
* To build Mac or Linux releases, you'll need `xmllint`.

### Installing

* Downloadable applications are planned for Linux (as an AppImage), macOS (as an application bundle in a ZIP or DMG file), and a Windows executable (in a ZIP file).

### Executing program

* If you're on an ARM64 Mac, you can download the binary. You may need to allow "Gatekeeper" to launch the application or need to dequarantine the application:
   * Execute `xattr -dr com.apple.quarantine path/to/EpochEditor.app`, replacing the sample path to the actual path to your downloaded copy of EpochEditor.
* Otherwise, the fastest way to get started is:
```
dotnet run --project src/EpochEditor.Gui/EpochEditor.Gui.csproj
```

## Help

Feel free to post in [Discussions](https://github.com/codergal6502/EpochEditor/discussions).

## Authors

* [CoderGal6502](https://github.com/codergal6502)

## Version History

* 0.1.0
    * Binary for macOS ARM64 released.

## License

This project is licensed under the [GNU GENERAL PUBLIC LICENSE Version 3] License - see the [LICENSE](https://github.com/codergal6502/EpochEditor/blob/main/LICENSE) file for details.

## Acknowledgments

* [DomPizzie's README Template](https://gist.github.com/DomPizzie/7a5ff55ffa9081f2de27c315f5018afc)
* [mcred's Chrono Trigger SNES Save File Editor](https://github.com/mcred/chrono-trigger-save-editor)
* [Data Crystal](https://datacrystal.tcrf.net/w/index.php?title=Chrono_Trigger_(SNES)/RAM_map)
