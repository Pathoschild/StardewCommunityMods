/*
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
 */


using System;
//using System.Dynamic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
//using System.Configuration;
//using System.Web.Script.Serialization;

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
