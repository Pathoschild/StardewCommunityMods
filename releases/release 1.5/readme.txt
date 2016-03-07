Adjusts the game clock speed by a configurable amount.  Speed up or slow down time.  Compatible with FreezeInside.
By cantorsdust with technical help from Zoryn.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
INSTALLATION
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

There is one folder in this .zip containing two files, TimeSpeed.dll and Config.json.  This folder may be placed in %appdata%\StardewValley\Mods.

Thus, the total path for both of the two files required for this mod to function are:
%appdata%\StardewValley\Mods\TimeSpeed\TimeSpeed.dll

AND

%appdata%\StardewValley\Mods\TimeSpeed\Config.json.


REQUIRES Storm to be installed!
https://gitlab.com/Demmonic/Storm

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
USAGE
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Run StormLoader.exe in your main Stardew Valley folder.  This will load the mods and then start the game.
The speed at which the game clock runs will be changed according to your settings.

Please note that this game comes with a Config.json file with two options:

1.  TenMinuteTickLength, which defaults to 14.  Set to your desired day length in seconds.  The base day is 7 seconds.  Setting TenMinuteTickLength to 0 seconds or lower will not make time run backwards ;)
2.  ChangeTimeSpeedOnFestivalDays, which defaults to false.  Set to true to enable time changing on festival days.  This option is here because some users have reported problems with festival days if time is changed.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CHANGELOG
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

v1.5
Ported to Storm.  Future releases will likely only use Storm.

v1.4.2
Fixed bug if user upgraded dll from 1.2 without upgrading INI

v1.4.1
Fixed incorrect file path breaking function if INI is in Stardew Valley\Mods rather than %appdata%.

v1.4
Added new config option ChangeTimeSpeedOnFestivalDays, defaulted to false, due to reports that this mod messes up festival days.  Not convinced, but this will allow further testing.