-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SkullCaveSaver v1.1.0.0.SMAPI
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Saves your progress in the skull cave every X floors (X is configurable).  When you return, it will warp you to your last saved level.
By cantorsdust.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
INSTALLATION
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

The zip contains one folder, SkullCaveSaver, with two files SkullCaveSaver.dll and manifest.json. The folder must be placed in %appdata%\StardewValley\Mods.

Thus, the total path for all 3 of the files required for this mod to function are:
%appdata%\StardewValley\Mods\SkullCaveSaver\SkullCaveSaver.dll

AND

%appdata%\StardewValley\Mods\SkullCaveSaver\manifest.json

Upon running the mod for the first time and loading a game, the folder psconfigs will be created.  Inside that will be your config file for each save game.


REQUIRES SMAPI 0.39+ to be installed!
https://github.com/ClxS/SMAPI/releases


-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
USAGE
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Run StardewModdingAPI.exe in your main Stardew Valley folder. This will load the mods and then start the game.
As you progress in the Skull Cave, your level will be saved every X floors, where X is defined by SaveLevelEveryXFloors in the config.  By default, this is 5, so your level will be saved once you pass floor 5, 10, 15, etc.  It saves if you've made it to that level or past it, so you can fall down holes and not worry about skipping a save level. For example, falling from level 6 to level 11 without specifically hitting level 10 will still save your last level as 10.

Please note that this game uses SMAPI 0.39+'s new per-save config system.  Your config file is specific to your save.  This allows me to save your mine progress individually for each save file you're using.  To find your config file, look in the psconfigs folder within your mod folder (full path %appdata%\StardewValley\SkullCaveSaver\psconfigs.  There you will find a file that looks like [YOUR SAVE NAME]_[LOTS OF NUMBERS].json  Inside are the following options:

  "LastMineLevel": 0,
  "SaveLevelEveryXFloors": 5
  
LastMineLevel, which defaults to 0.  This is the deepest level of the mine that the mod has saved.  Edit this only if you want to skip ahead a number of levels.  The skull cave starts on level 121.
SaveLevelEveryXFloors, which defaults to 5.  This is how often the mod will save your cave level.  It defaults to 5, like in the mine, where the elevator appears every five levels.  You can set this as low as 1 to save your progress every level.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
LICENSE
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

SkullCaveSaver is licensed under GPL v3.  You will find a copy of its source at the same place you downloaded it, https://github.com/cantorsdust/SkullCaveSaver

Copyright 2016 cantorsdust

SkullCaveSaver is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SkullCaveSaver is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SkullCaveSaver.  If not, see <http://www.gnu.org/licenses/>.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CHANGELOG
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
v1.0.0.0.SMAPI
Initial public release.