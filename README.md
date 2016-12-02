# Tractor Mod - A SMAPI Mod for Stardew Valley
#### A mod for stardew valley: auto till, water, fertilize, and seed dirt tiles on your farm by simply walking over them. 

Modder: PhthaloBlue  

This is a mod that allows players to quickly till dirt, sow seeds, fertilize soil, and water crop by simple walking over them.  

It is default to only work with iridium tools equipped so that the mod doesnt ruin your early game but you can change that in the config file.

###Latest Version: [v.2.1.1](https://github.com/lambui/StardewValleyMod_TractorMod/releases)
####Change Log **v.2.1.1**:
+ **Fix** a bug that prevents game from saving when sleeping if player left Tractor outside Farm.
+ **Fix** a bug that prevents player from summon horse if horse is outside Farm.

####Change Log **v.2.1**:
+ **Change** config.json:
  - Remove WTFMode
  - Remove harvestMode, harvestRadius  
  - Remove minToolPower
  - Remove mapWidth, mapHeight. You no longer have to worry about your map size
  - Add **tool** list and **info** regarding how to use it (do not delete info).
+ **Improve** algorithm, improve performance.

####Change Log **v.2.0**:
+ **Remove** horseMode.
  - needHorse in config.json is removed.
  - Ability to toggle TractorMode on horse is removed.
+ **Add** Tractor:
  - Now you have a brand spanking new **Tractor** seperated from your horse.
  - Tractor will return to the spot right behind your selling box every norming.
  - Riding the Tractor automatically turn on TractorMode.
  - tractorKey in config.json now sets hotkey to summon Tractor to your location. Default to B.
  - tractor sprite and animation by [Horse to Tractor mod by Pewtershmitz](http://community.playstarbound.com/threads/tractor-v-1-3-horse-replacement.108604/)
  - You can change tractor sprite and animation by modding tractor.xnb in TractorXNB folder.
+ **Add** option to change mouse-activation hotkey (activating TractorMod while not on Tractor).
  - holdActivate in config.json file.
  - 0: no mouse hotkey (can't activate TractorMode while not on Tractor). Default.
  - 1: hold left mouse button to activate.
  - 2: hold right mouse button to activate (this is the old one).
  - 3: hold mouse wheel down to activate.
+ **Change** TractorMod buff.
  - TractorMod no longer provides +1 speed buff.
  - TractorMod now gives -2 speed buff (for balance).
  - You can change it by changing tractorSpeed in config.json because I'm a good person.
+ **Add** horse summon hotkey.
  - horseKey in config.json now sets hotkey to summon your horse (if you have one) to your location.
  - Default to None (deactivated).
  
####Past [changelog.](https://github.com/lambui/StardewValleyMod_TractorMod/blob/master/Changelog.md)

###Demo:  
####BOTH TRACTOR AND HORSE!!!!!!!!
![image](https://github.com/lambui/StardewValleyMod_TractorMod/blob/gif/images/realtractor.png)

####Tractor [(thank Horse to Tractor mod by Pewtershmitz for sprite)](http://community.playstarbound.com/threads/tractor-v-1-3-horse-replacement.108604/)
![gif-horse-mode](https://github.com/lambui/StardewValleyMod_TractorMod/blob/gif/images/tractor2.gif)  

####Till Dirt  
![gif-till dirt](https://github.com/lambui/StardewValleyMod_TractorMod/blob/gif/images/TillDirt.gif)  

####Water Crop    
![gif-water crop](https://github.com/lambui/StardewValleyMod_TractorMod/blob/gif/images/water.gif)  

####Fertilize Soil    
![gif-fertilize](https://github.com/lambui/StardewValleyMod_TractorMod/blob/gif/images/fertilizing.gif)  

####Sow Seeds      
![gif-sow seed](https://github.com/lambui/StardewValleyMod_TractorMod/blob/gif/images/sowingSeed.gif)  

####Harvest Crops      
![gif-harvest_crop](https://github.com/lambui/StardewValleyMod_TractorMod/blob/gif/images/harvestCrop.gif)  

####Harvest Fruits      
![gif-harvest_fruit](https://github.com/lambui/StardewValleyMod_TractorMod/blob/gif/images/harvestFruitTree.gif)  

####Harvest Truffles      
![gif-harvest_drop](https://github.com/lambui/StardewValleyMod_TractorMod/blob/gif/images/harvestDrop.gif)  

###Require:  
1. [Stardew Valley](http://store.steampowered.com/app/413150/)
2. [SMAPI: +1.1](https://github.com/ClxS/SMAPI/releases)

###Install:  
1. Get [Stardew Valley](http://store.steampowered.com/app/413150/) $$$
2. [Install SMAPI](http://canimod.com/guides/using-mods#installing-smapi)
3. Unzip the mod folder into Stardew Valley/Mods (just put TractorMod folder into /Mods folder)
4. Run [SMAPI](http://canimod.com/guides/using-mods#installing-smapi)


###Download [here](https://github.com/lambui/StardewValleyMod_TractorMod/releases)

###How To Use:
+ Normal: 
  1. The Tractor will be behind selling box every morning.
  2. Get on the Tractor.
  3. Hold your hoe if you want to till dirt  
    Hold water can if you want to water crop  
    Hold seed bag(s) to sow seeds  
    Hold fertilizer to fertilize tilled soil
    Hold scythe to harvest
  4. When done get off the Tractor to turn Tractor Mode off.
+ Mouse-Activating Mode:  
    1. Get to your farm (this mod can only be activated on farmland, not in town, not even in greenhouse)
    2. Turn on Tractor Mode by holding down appropriate mouse button (set by holdActivate in config.json). 
    You will receive a speed buff showing that the Tractor Mode is now on.
    3. Hold your hoe if you want to till dirt  
    Hold water can if you want to water crop  
    Hold seed bag(s) to sow seeds  
    Hold fertilizer to fertilize tilled soil
    4. After you are done with your farmwork, simply release right click to turn it off.  
    The buff will go away showing that the Mode is now off.


###Customize: [see here in Customize section.](https://github.com/lambui/StardewValleyMod_TractorMod/blob/master/Changelog.md)  

###Note:  
+ Be careful when tilling your soil because the hoe can destroy your small-medium sized fruit trees.  
+ Tractor Mode doesn't drain your stamina when using Hoe and Water can.  
+ Water can doesn't need to be refilled but needs to have some water in it to work.  
+ You can fertilize your crop AFTER sowing seeds with Tractor Mode.  
+ Be extra careful with WTFMode haha
+ **Have fun farming! :)**

Contact me @ [buiphuonglamvn@gmail.com](mailto:buiphuonglamvn@gmail.com) regarding whatever.
