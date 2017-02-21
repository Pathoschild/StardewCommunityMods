# TimeSpeed
Slows game clock by a configurable amount in Stardew Valley.

Adjusts the game clock speed by a configurable amount. Speed up, slow down, or completely freeze time.

Json configuration documentation can be found [here](https://github.com/alexb5dh/TimeSpeed/blob/master/TimeSpeed/TimeSpeedConfig.json.cs).  

## Usage
Default key bindings:

key | effect
:-- | :-----
Press `N` | Freezes or unfreezes time. Freezing time will stay in effect until you unfreeze it; unfreezing time will stay in effect until you enter a new location with time settings.
Press `,` | Slow down time by one second per 10-game-minutes. Combine with `Control` to increase by 100 seconds, `Shift` to increase by 10 seconds, or `Alt` to increase by 0.1 seconds.
Press `.` | Speed up time by one second per 10-game-minutes. Combine with Control to decrease by 100 seconds, Shift to decrease by 10 seconds, or Alt to decrease by 0.1 seconds.
Press `B` | Reload the config settings from file.

The mod creates a `config.json` the first time you run it, where you can configure the mod.

setting | effect
:------ | :-----
`DefaultTickLength` | The default number of seconds per 10-game-minutes, or `null` to freeze time globally. The game uses 7 seconds by default.
`TickLengthsByLocation` | The number of seconds per 10-game-minutes (or `null` to freeze time) for each location name. You can also use `Indoors` or `Outdoors` as a location name, which will be the default for all indoors/outdoors locations.
`EnableOnFestivalDays` | Whether to change tick length on festival days.
`FreezeTimeAt` | The time at which to freeze time everywhere (or `null` to disable this). This should be 24-hour military time (e.g. 800 for 8am, 1600 for 8pm, etc). (Be careful the number doesn't start with a zero, due to a bug in the underlying parser.)
`LocationNotify` | Whether to show a message about the time settings when you enter a location.
`Keys` | The keyboard bindings used to control the flow of time. See [available keys](https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx). Set a key to `null` to disable it.

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
