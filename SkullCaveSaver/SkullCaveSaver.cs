using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;

namespace SkullCaveSaver
{
    public class SkullCaveSaver : Mod
    {
        public static ModConfig SkullCaveSaverConfig { get; private set; }
        public bool loadingNewLevel = false;

        public override void Entry(params object[] objects)
        {
            Console.WriteLine("SkullCaveSaver Has Loaded");
            PlayerEvents.LoadedGame += Events_GameLoaded;
            GameEvents.HalfSecondTick += Events_HalfSecond;


        }

        void runConfig()
        {
            SkullCaveSaverConfig = new ModConfig().InitializeConfig(PerSaveConfigPath);
        }

        public void Events_GameLoaded(object sender, EventArgs e)
        {
            runConfig();
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
                    SkullCaveSaverConfig.WriteConfig<ModConfig>();
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

    public class ModConfig : Config
    {
        public int LastMineLevel { get; set; }
        public int SaveLevelEveryXFloors { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            LastMineLevel = 0;
            SaveLevelEveryXFloors = 5;

            return this as T;
        }
    }
}
