Adjusts the game clock speed by a configurable amount.  Speed up or slow down time.  Now includes all FreezeInside functionality--it is recommended not to run the two mods together.
By cantorsdust and Syndlig with technical help from Zoryn.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
INSTALLATION
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

Run StormLoader.exe in your main Stardew Valley folder.  This will load the mods and then start the game.
The speed at which the game clock runs will be changed according to your settings.

Please note that this game comes with a Config.json file with nine options:

  "OutdoorTickLength": 14.0,
  "IndoorTickLength": 14.0,
  "MineTickLength": 14.0,
  "ChangeTimeSpeedOnFestivalDays": false,
  "FreezeTimeOutdoors": false,
  "FreezeTimeIndoors": false,
  "FreezeTimeInMines": false,
  "LetMachinesRunWhileTimeFrozen": false,
  "FreezeTimeAt1230AM": false

1.  OutdoorTickLength, which defaults to 14.0.  Controls the length of the 10 minute tick outdoors.  Set to your desired ten minute tick length in seconds.  The base day is 7 seconds.  Setting TenMinuteTickLength to 0 seconds or lower will not make time run backwards ;)
2.  IndoorTickLength, which defaults to 14.0.  Controls the length of the 10 minute tick indoors.  Set to your desired ten minute tick length in seconds.  The base day is 7 seconds.  Setting TenMinuteTickLength to 0 seconds or lower will not make time run backwards ;)
3.  IndoorTickLength, which defaults to 14.0.  Controls the length of the 10 minute tick while in the mine.  Set to your desired ten minute tick length in seconds.  The base day is 7 seconds.  Setting TenMinuteTickLength to 0 seconds or lower will not make time run backwards ;)
4.  ChangeTimeSpeedOnFestivalDays, which defaults to false.  Set to true to enable time changing on festival days.  This option is here because some users have reported problems with festival days if time is changed.
5.  FreezeTimeOutdoors, which defaults to false.  Set to true to freeze time outdoors.
6.  FreezeTimeIndoors, which defaults to false.  Set to true to freeze time indoors, except for the mines.
7.  FreezeTimeInMines, which defaults to false.  Set to true to freeze time in the mines.
8.  LetMachinesRunWhileTimeFrozen, which defaults to false.  If set to true, machines continue to run while you are inside and time is frozen.  Some consider this "cheaty".  Setting it to false will prevent machines from running while you are inside.  As a general rule, I recommend this be set to false unless you desire the gameplay change.  This option is more likely to break things than any other--although most obvious bugs have been removed.
9.  FreezeTimeAt1230AM, which defaults to false.  Set to true to freeze time when the day reaches 12:30 AM (24:30 military time).  This occurs no matter where you are.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CHANGELOG
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
v1.6.0.0
Rewrite/refactoring to merge FreezeInside mod features.  Added FreezeTimeAt1230AM option.  Changed tick lengths to floats.

v1.5.3.1
Fixed overflow bug for very long tick lengths.

v1.5.3.0
Updated for changes to Storm API.  Added smooth clock functionality!

v1.5.2
Updated for changes to Storm API.

v1.5.1
Updated dependencies.

v1.5
Ported to Storm.  Future releases will likely only use Storm.

v1.4.2
Fixed bug if user upgraded dll from 1.2 without upgrading INI

v1.4.1
Fixed incorrect file path breaking function if INI is in Stardew Valley\Mods rather than %appdata%.

v1.4
Added new config option ChangeTimeSpeedOnFestivalDays, defaulted to false, due to reports that this mod messes up festival days.  Not convinced, but this will allow further testing.