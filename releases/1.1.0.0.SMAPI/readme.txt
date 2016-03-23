-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
InstantGrowTrees v1.1.0.0.SMAPI
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Allows any tree that can grow to be instantly grown overnight.  Configurable for regular trees and/or fruit trees.
By cantorsdust.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
INSTALLATION
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

The zip contains one folder, InstantGrowTrees, with three files InstantGrowTrees.dll, config.json, and manifest.json. The folder must be placed in %appdata%\StardewValley\Mods.

Thus, the total path for all 3 of the files required for this mod to function are:
%appdata%\StardewValley\Mods\InstantGrowTrees\InstantGrowTrees.dll

AND

%appdata%\StardewValley\Mods\InstantGrowTrees\manifest.json

AND

%appdata%\StardewValley\Mods\InstantGrowTrees\config.json


REQUIRES SMAPI 0.39+ to be installed!
https://github.com/ClxS/SMAPI/releases


-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
USAGE
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Run StardewModdingAPI.exe in your main Stardew Valley folder. This will load the mods and then start the game.
Any saplings or tree seeds planted that meet the base game requirements to grow (space around them) will grow instantly overnight.

Please note that this game comes with a config.json file with three options:

  "FruitTreesInstantGrow": false,
  "RegularTreesInstantGrow": true,
  "RegularTreesGrowInWinter": false
  
FruitTreesInstantGrow, which defaults to false.  Setting to true will have fruit trees grow overnight.  Setting to false disables this.
RegularTreesInstantGrow, which defaults to true.  Setting to true will have regular trees (oak, maple, and pine) grow overnight.  Setting to false disables this.
RegularTreesGrowInWinter, which defaults to false.  Setting to true will have regular trees continue to grow in winter.  The base game does not allow regular trees to do so.  Setting to false disables this.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
LICENSE
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

InstantGrowTrees is licensed under GPL v3.  You will find a copy of its source at the same place you downloaded it, https://github.com/cantorsdust/InstantGrowTrees

Copyright 2016 cantorsdust

InstantGrowTrees is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

InstantGrowTrees is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with InstantGrowTrees.  If not, see <http://www.gnu.org/licenses/>.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CHANGELOG
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
v1.1.0.0.SMAPI
Updated to SMAPI 0.39, initial public release.