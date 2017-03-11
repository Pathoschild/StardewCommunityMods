**Skull Cave Saver** is a [Stardew Valley](http://stardewvalley.net/) mod which saves your progress
in the Skull Cave every 5th floor (configurable). When you return, it will warp you to the saved
level.

Compatible with Stardew Valley 1.11+ on Linux, Mac, and Windows. Originally written by cantorsdust,
and now maintained by the community — pull requests are welcome!

## Contents
* [Install](#install)
* [Use](#use)
* [Versions](#versions)
* [See also](#see-also)

## Install
1. [Install the latest version of SMAPI](http://canimod.com/for-players/install-smapi).
2. Install [this mod from Nexus mods](http://www.nexusmods.com/stardewvalley/mods/175).
3. Run the game using SMAPI.

## Use
Just install the mod and play the game. It will save your Skull Cave level and warp you back to it
automatically.

The mod creates a `config/<save id>.json` file the first time you run it. You can open the file in a text
editor to configure the mod:

setting | effect
:------ | :-----
`SaveLevelEveryXFloors` | The multiple of floors at which to save (e.g. `5` to save every fifth floor).
`LastMineLevel` | The last saved Skull Cave floor. This is updated automatically, but you can set it manually to skip to that floor.

## Versions
### 1.0
* Initial release.

### 1.1
* Updated for SMAPI 1.9+ and Stardew Valley 1.2.

## See also
* [Nexus mod](http://www.nexusmods.com/stardewvalley/mods/175)
* [Discussion thread](http://community.playstarbound.com/threads/smapi-skullcavesaver.111429/)
