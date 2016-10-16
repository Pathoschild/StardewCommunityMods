# TimeSpeed (unofficial)
Rework of [TimeSpeed](http://www.nexusmods.com/stardewvalley/mods/169/) mod for Stardew Valley.  
Added configurable hotkeys and player notifications.

## Configuration
- __OutdoorTickLength__, __IndoorTickLength__, __MineTickLength__: defaults to 14 seconds. Controls the length of the 10 minute tick. Set to your desired ten-minute tick length in milliseconds. The base day is 7 seconds.  
Minimal allowed value is 100 milliseconds.

- __FreezeTimeOutdoors__: defaults to false. Set to true to freeze time outdoors.

- __FreezeTimeIndoors__: defaults to false. Set to true to freeze time indoors, except for the mines.

- __FreezeTimeInMines__: defaults to false. Set to true to freeze time in the mines.
 
- __FreezeTimeAt__: defaults to null. Set to integer to freeze time when the day reaches specified time (example: `1230` -> freze at 12:30). This occurs no matter where you are.

- __FreezeTimeKey__: this key will force time to change its state (from frozen to unfrozen and vice verca).  
If set to frozen it will stay this way until key is pressed again.  
If set to unfrozen it will freeze after changing area if _FreezeTime*_ option is true for new location.

- __IncreaseTickLengthKey__, __DecreaseTickLengthKey__: keys to increase/decrease tick length by 100 milliseconds (1 second if Left Shift is hold).

## Requirements
- Stardew Valley 1.11
- [SMAPI](https://github.com/cjsu/SMAPI/releases) 0.40.1.1+
