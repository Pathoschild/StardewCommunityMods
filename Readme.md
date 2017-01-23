# TimeSpeed (unofficial)
Continuation of [TimeSpeed](http://www.nexusmods.com/stardewvalley/mods/169/) mod for Stardew Valley.  
Json configuration documentation can be found [here](https://github.com/alexb5dh/TimeSpeed/blob/master/TimeSpeed/TimeSpeedConfig.json.cs).

## Requirements
- Stardew Valley 1.11
- [SMAPI](https://github.com/cjsu/SMAPI/releases) 0.40.1.1+

## Building
Solution is created in Visual Studio 2015.

Mod can be build for Windows, Mac and Linux game versions.
You can change desired platform via solution configurations.  
Each platform requires corresponding binaries from the game folder to be present in _External_ directory:

* **Windows**:  
_External/Windows_:  
  * Stardew Valley.exe
  * StardewModdingAPI.exe
  * xTile.dll

* **Mac**:  
_External/Mac_:  
  * MonoGame.Framework.dll
  * StardewModdingAPI.exe
  * StardewValley.exe
  * xTile.dll

* **Linux**:  
_External/Linux_:  
  * MonoGame.Framework.dll
  * StardewModdingAPI.exe
  * StardewValley.exe
  * xTile.dll

Linux and Mac files are probably the same by I haven't tested it. 

If you want to build for platform(s) other that the one you are currently using
you can use [SteamCmd](https://developer.valvesoftware.com/wiki/SteamCMD#Cross-Platform_Installation) to download files for other platforms.
