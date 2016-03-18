# TimeSpeed
Slows game clock by a configurable amount in Stardew Valley.

Adjusts the game clock speed by a configurable amount.  Speed up, slow down, or completely freeze time.  Now includes all FreezeInside functionality--it is recommended not to run the two mods together.
By cantorsdust and Syndlig with technical help from Zoryn.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
INSTALLATION--SMAPI
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

The zip contains two files, TimeSpeed.dll and TimeSpeedConfig.ini. These files may be placed in %appdata%\StardewValley\Mods or your main Stardew Valley\Mods folder.

Thus, the total path for both of the two files required for this mod to function are:
%appdata%\StardewValley\Mods\TimeSpeed.dll

AND

%appdata%\StardewValley\Mods\TimeSpeedConfig.ini

OR

Stardew Valley\Mods\TimeSpeed.dll

AND

Stardew Valley\Mods\TimeSpeedConfig.ini


REQUIRES SMAPI 0.37+ to be installed!  Version 1.2 is for SMAPI 0.35 or 0.36.
https://github.com/ClxS/SMAPI/releases

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
INSTALLATION--STORM
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

There is one folder in this .zip containing three files, TimeSpeed.dll, Config.json, and manifest.json.  This folder may ONLY be placed in %appdata%\StardewValley\Mods.

Thus, the total path for the three files required for this mod to function are:
%appdata%\StardewValley\Mods\TimeSpeed\TimeSpeed.dll

%appdata%\StardewValley\Mods\TimeSpeed\Config.json.

%appdata%\StardewValley\Mods\TimeSpeed\manifest.json.


REQUIRES Storm to be installed!
https://gitlab.com/Demmonic/Storm

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
USAGE
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Run StardewModdingAPI.exe in your main Stardew Valley folder. This will load the mods and then start the game.
The speed at which the game clock runs will be changed according to your settings.

You can override your freeze time settings and immediately freeze time by pressing N.  Pressing N again restores your previous freeze time settings.

You can halve each of your tick lengths by pressing ,

You can double each of your tick lengths by pressing .

You can restore your original config settings that are stored in your config file by pressing B.

Please note that this game comes with a TimeSpeedConfig.ini file with nine options:

"OutdoorTickLength": 14,
"IndoorTickLength": 14,
"MineTickLength": 14,
"ChangeTimeSpeedOnFestivalDays": false,
"FreezeTimeOutdoors": false,
"FreezeTimeIndoors": false,
"FreezeTimeInMines": false,
"LetMachinesRunWhileTimeFrozen": false,
"FreezeTimeAt1230AM": false

OutdoorTickLength, which defaults to 14. Controls the length of the 10 minute tick outdoors. Set to your desired ten minute tick length in seconds. The base day is 7 seconds. Setting TenMinuteTickLength to 0 seconds or lower will not make time run backwards ;)
IndoorTickLength, which defaults to 14. Controls the length of the 10 minute tick indoors. Set to your desired ten minute tick length in seconds. The base day is 7 seconds. Setting TenMinuteTickLength to 0 seconds or lower will not make time run backwards ;)
IndoorTickLength, which defaults to 14. Controls the length of the 10 minute tick while in the mine. Set to your desired ten minute tick length in seconds. The base day is 7 seconds. Setting TenMinuteTickLength to 0 seconds or lower will not make time run backwards ;)
ChangeTimeSpeedOnFestivalDays, which defaults to false. Set to true to enable time changing on festival days. This option is here because some users have reported problems with festival days if time is changed.
FreezeTimeOutdoors, which defaults to false. Set to true to freeze time outdoors.
FreezeTimeIndoors, which defaults to false. Set to true to freeze time indoors, except for the mines.
FreezeTimeInMines, which defaults to false. Set to true to freeze time in the mines.
LetMachinesRunWhileTimeFrozen, which defaults to false. If set to true, machines continue to run while you are inside and time is frozen. Some consider this "cheaty". Setting it to false will prevent machines from running while you are inside. As a general rule, I recommend this be set to false unless you desire the gameplay change. This option is more likely to break things than any other--although most obvious bugs have been removed.
FreezeTimeAt1230AM, which defaults to false. Set to true to freeze time when the day reaches 12:30 AM (24:30 military time). This occurs no matter where you are.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
LICENSE
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

TimeSpeed is licensed under GPL v3.  You will find a copy of its source at the same place you downloaded it, https://github.com/cantorsdust/TimeSpeed

Copyright 2016 cantorsdust

TimeSpeed is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

TimeSpeed is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with TimeSpeed.  If not, see <http://www.gnu.org/licenses/>.
