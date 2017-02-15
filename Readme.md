# TimeSpeed (unofficial)
Continuation of [TimeSpeed](http://www.nexusmods.com/stardewvalley/mods/169/) mod for Stardew Valley.  
Json configuration documentation can be found [here](https://github.com/alexb5dh/TimeSpeed/blob/master/TimeSpeed/TimeSpeedConfig.json.cs).  
Forum topic can be found [here](http://community.playstarbound.com/threads/timespeed-unofficial.126181/).

## Requirements
- Stardew Valley 1.11
- [SMAPI](https://github.com/cjsu/SMAPI/releases) 0.40.1.1+

## Compiling the mod
Installing the stable release is recommended for most users. If you really want to compile the mod
yourself, read on.

These mods use the [crossplatform build config](https://github.com/Pathoschild/Stardew.ModBuildConfig#readme)
so they can be built on Linux, Mac, and Windows without changes. See [the build config documentation](https://github.com/Pathoschild/Stardew.ModBuildConfig#readme)
for troubleshooting.

### Compiling a mod for testing
To compile a mod and add it to your game's `Mods` directory:

1. Rebuild the project in [Visual Studio](https://www.visualstudio.com/vs/community/) or [MonoDevelop](http://www.monodevelop.com/).  
   <small>This will compile the code and package it into the mod directory.</small>
2. Launch the project with debugging.  
   <small>This will start the game through SMAPI and attach the Visual Studio debugger.</small>

### Compiling a mod for release
To package a mod for release:

1. Delete the mod's directory in `Mods`.  
   <small>(This ensures the package is clean and has default configuration.)</small>
2. Recompile the mod per the previous section.
3. Launch the game with SMAPI to generate the default `config.json`.
4. Create a zip file of the mod's folder in the `Mods` folder. The zip name should include the
   mod name and version (like `TimeSpeed-1.0.zip`).
