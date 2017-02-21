using System;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;

namespace SkullCaveSaver
{
    public class SkullCaveSaver : Mod
    {
        private static ModConfig SkullCaveSaverConfig;

        public bool loadingNewLevel = false;

        public override void Entry(IModHelper helper)
        {
            Console.WriteLine("SkullCaveSaver Has Loaded");
            SaveEvents.AfterLoad += Events_AfterLoad;
            GameEvents.HalfSecondTick += Events_HalfSecond;
        }

        void Events_AfterLoad(object sender, EventArgs e)
        {
            string path = Path.Combine("config", $"{Constants.SaveFolderName}.json");
            SkullCaveSaver.SkullCaveSaverConfig = this.Helper.ReadJsonFile<ModConfig>(path) ?? new ModConfig();
            this.Helper.WriteJsonFile(path, SkullCaveSaver.SkullCaveSaverConfig);
        }

        void Events_HalfSecond(object sender, EventArgs e)
        {
            GameLocation location = Game1.currentLocation;
            if (location is MineShaft)
            {
                var ms = location as MineShaft;
                if (ms.mineLevel > SkullCaveSaverConfig.LastMineLevel && (ms.mineLevel - SkullCaveSaverConfig.LastMineLevel) >= SkullCaveSaverConfig.SaveLevelEveryXFloors)
                {
                    SkullCaveSaverConfig.LastMineLevel = ms.mineLevel - (ms.mineLevel % SkullCaveSaverConfig.SaveLevelEveryXFloors);
                    this.Helper.WriteConfig(SkullCaveSaver.SkullCaveSaverConfig);
                }
                if (ms.mineLevel > 120 && ms.mineLevel < SkullCaveSaverConfig.LastMineLevel && !loadingNewLevel)
                {
                    Game1.enterMine(false, SkullCaveSaverConfig.LastMineLevel, "");
                    loadingNewLevel = true;
                    GameEvents.UpdateTick += Events_Update;
                }
            }
        }

        void Events_Update(object sender, EventArgs e)
        {
            if (loadingNewLevel)
            {
                GameLocation location = Game1.currentLocation;
                if (location is MineShaft)
                {
                    var ms = location as MineShaft;
                    if (ms.mineLevel >= SkullCaveSaverConfig.LastMineLevel)
                    {
                        loadingNewLevel = false;
                        GameEvents.UpdateTick -= Events_Update;
                    }
                }
            }
        }
    }
}
