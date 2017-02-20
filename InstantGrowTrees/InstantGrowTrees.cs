using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace InstantGrowTrees
{
    public class InstantGrowTrees : Mod
    {
        public static ModConfig InstantGrowTreesConfig { get; private set; }

        public override void Entry(params object[] objects)
        {
            runConfig();
            Console.WriteLine("InstantGrowTrees Has Loaded");
            TimeEvents.DayOfMonthChanged += Events_NewDay;
        }

        void runConfig()
        {
            InstantGrowTreesConfig = new ModConfig().InitializeConfig(BaseConfigPath);
        }

        public void Events_NewDay(object sender, EventArgs e)
        {
            foreach (var location in Game1.locations)
            {
                foreach (var terrainfeature in location.terrainFeatures)
                {
                    if (InstantGrowTreesConfig.RegularTreesInstantGrow && terrainfeature.Value is Tree)
                    {
                        Tree tree = (Tree)terrainfeature.Value;
                        GrowTree(tree, location, terrainfeature.Key);
                    }
                    if (InstantGrowTreesConfig.FruitTreesInstantGrow && terrainfeature.Value is FruitTree)
                    {
                        FruitTree fruittree = (FruitTree)terrainfeature.Value;
                        GrowFruitTree(fruittree, location, terrainfeature.Key);
                    }
                }
            }
        }

        public void GrowTree(Tree tree, GameLocation location, Vector2 tileLocation)
        {
            Rectangle rectangle = new Rectangle((int)(((double)tileLocation.X - 1.0) * (double)Game1.tileSize), (int)(((double)tileLocation.Y - 1.0) * (double)Game1.tileSize), Game1.tileSize * 3, Game1.tileSize * 3);
            if (!Game1.currentSeason.Equals("winter") || tree.treeType == 6 || InstantGrowTreesConfig.RegularTreesGrowInWinter)
            {
                string str = location.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "NoSpawn", "Back");
                if (str != null && (str.Equals("All") || str.Equals("Tree")))
                    return;
                if (tree.growthStage < 5)
                {
                    foreach (KeyValuePair<Vector2, TerrainFeature> keyValuePair in (Dictionary<Vector2, TerrainFeature>)location.terrainFeatures)
                    {
                        if (keyValuePair.Value is Tree && !keyValuePair.Value.Equals((object)this) && ((Tree)keyValuePair.Value).growthStage >= 5 && keyValuePair.Value.getBoundingBox(keyValuePair.Key).Intersects(rectangle))
                        {
                            tree.growthStage = 4;
                            return;
                        }
                    }
                }
                else if (tree.growthStage == 0 && location.objects.ContainsKey(tileLocation))
                    return;
                tree.growthStage = 5;
            }
        }

        public void GrowFruitTree(FruitTree fruittree, GameLocation environment, Vector2 tileLocation)
        {
            bool flag = false;
            foreach (Vector2 tileLocation1 in Utility.getSurroundingTileLocationsArray(tileLocation))
            {
                if (environment.isTileOccupied(tileLocation1, "") && (!environment.terrainFeatures.ContainsKey(tileLocation) || !(environment.terrainFeatures[tileLocation] is HoeDirt) || (environment.terrainFeatures[tileLocation] as HoeDirt).crop == null))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                fruittree.daysUntilMature = 0;
                fruittree.growthStage = 4;
            }
        }
    }

    public class ModConfig : Config
    {
        public bool FruitTreesInstantGrow { get; set; }
        public bool RegularTreesInstantGrow { get; set; }
        public bool RegularTreesGrowInWinter { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            FruitTreesInstantGrow = false;
            RegularTreesInstantGrow = true;
            RegularTreesGrowInWinter = false;

            return this as T;
        }
    }
}
