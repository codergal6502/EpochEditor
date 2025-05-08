#!/usr/local/bin/pwsh
$ROOT = "$(Split-Path -Path $MyInvocation.MyCommand.Path -Parent)/.."

$CSPROJ = "$ROOT/src/EpochEditor.Gui/EpochEditor.Gui.csproj"
# $ASSETS = "$ROOT/build/assets/linux-gui-single-file" 

$DIST = "$ROOT/build/output/windows/EpochEditor"

if (Test-Path -Path "$ROOT/build/output/windows/EpochEditor") {
    Remove-Item "$ROOT/build/output/windows/EpochEditor" -Recurse
}

$VERSION = (Select-XML -XPath "/Project/PropertyGroup/Version/text()" -Path "$ROOT/src/Directory.Build.Props").Node.InnerText
$EPOCH = [System.Math]::Floor((Get-Date -UFormat %s)/3600/24)

$LONGVERSION="$VERSION.$EPOCH"

dotnet publish "$CSPROJ" -r win-x64 "/p:Version=$VERSION" "/p:FileVersion=$LONGVERSION" "/p:InformationalVersion=$LONGVERSION (.NET Dependency)" --configuration release -o "$DIST"
Compress-Archive -Path "$DIST" -DestinationPath "$DIST-win-x64.zip"
