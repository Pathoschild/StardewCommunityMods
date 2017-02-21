**TimeSpeed** is a [Stardew Valley](http://stardewvalley.net/) mod which lets you control the flow
of time in the game: speed it up, slow it down, or freeze it altogether. This can happen
automatically or when you press a key in the game.

Compatible with Stardew Valley 1.11+ on Linux, Mac, and Windows. Originally written by cantorsdust,
and now maintained by the community — pull requests are welcome!

## Contents
* [Install](#install)
* [Use](#use)
* [Versions](#versions)
* [See also](#see-also)

## Install
1. [Install the latest version of SMAPI](http://canimod.com/for-players/install-smapi).
2. Install [this mod from Nexus mods](http://www.nexusmods.com/stardewvalley/mods/169).
3. Run the game using SMAPI.

## Use
You can press these keys in-game (configurable):

key | effect
:-- | :-----
`N` | Freeze or unfreeze time. Freezing time will stay in effect until you unfreeze it; unfreezing time will stay in effect until you enter a new location with time settings.
`,` | Slow down time by one second per 10-game-minutes. Combine with `Control` to increase by 100 seconds, `Shift` to increase by 10 seconds, or `Alt` to increase by 0.1 seconds.
`.` | Speed up time by one second per 10-game-minutes. Combine with Control to decrease by 100 seconds, Shift to decrease by 10 seconds, or Alt to decrease by 0.1 seconds.
`B` | Reload the config settings from file.

The mod creates a `config.json` file the first time you run it. You can open the file in a text
editor to configure the mod:

setting | effect
:------ | :-----
`DefaultTickLength` | The default number of seconds per 10-game-minutes, or `null` to freeze time globally. The game uses 7 seconds by default.
`TickLengthsByLocation` | The number of seconds per 10-game-minutes (or `null` to freeze time) for each location name. You can also use `Indoors` or `Outdoors` as a location name, which will be the default for all indoors/outdoors locations.
`EnableOnFestivalDays` | Whether to change tick length on festival days.
`FreezeTimeAt` | The time at which to freeze time everywhere (or `null` to disable this). This should be 24-hour military time (e.g. 800 for 8am, 1600 for 8pm, etc). (Be careful the number doesn't start with a zero, due to a bug in the underlying parser.)
`LocationNotify` | Whether to show a message about the time settings when you enter a location.
`Keys` | The keyboard bindings used to control the flow of time. See [available keys](https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx). Set a key to `null` to disable it.

## Versions
### 1.0
* Initial release.

### 1.1
* Added support for speeding up and slowing down time.

### 1.2
* Renamed config setting for clarity.

### 1.3
* Updated for SMAPI 0.37.
* Fixed config being autocreated with the wrong field name.

### 1.4
* _(Test release.)_

### 1.4.1
* Fixed config being read from wrong path in some cases.

### 1.4.2
* Fixed fast time bug if user updated from 1.2 without updating config.

### 1.5
* _(Storm API release.)_

### 1.6
* Added options to freeze time automatically based on location.
* Added smoother time slowing.
* Fixed overflow bug for long tick lengths.

### 1.7
* Updated for SMAPI 0.38.
* Migrated ini file to `config.json`.
* Added hotkeys to freeze/unfreeze/speed up/slow down time, and reset settings.
* Added support for holding `Shift` to change time flow by 10-second intervals.

### 1.8
* Updated for SMAPI 0.39.

### 1.9
* _(Lost to the sands of time.)_

### 1.9.1
* Added notifications when time is frozen or unfrozen.

### 1.9.2
* Added option to disable time changes on festival days.
* Added support for holding `Alt` to change time flow by 0.1-second intervals.

### 1.9.3
* Added smoother time flow changes.
* Added validation to prevent invalid time flow changes.

### 1.9.4
* Internal refactoring.
* Added hotkey to reload settings from `config.json`.

### 1.9.5
* Fixed time scaling in game areas that have a different clock rate.
* Fixed an issue with festival time scaling.

### 2.0
* Internal refactoring.

### 2.0.1
* Added Linux/Mac support.
* Fixed config resetting & parsing.

### 2.0.2
* Fixed hotkeys being processed before game is loaded.
* Fixed an issue when time is frozen between ticks.

### 2.0.3
* Fixed hotkeys being processed when a menu is open.

### 2.1
* Updated for SMAPI 1.9+ and Stardew Valley 1.2.
* Internal refactoring.

## See also
* [Nexus mod](http://www.nexusmods.com/stardewvalley/mods/169)
* [Discussion thread](http://community.playstarbound.com/threads/storm-and-smapi-timespeed-mod-configurable-day-lengths.107876/)
