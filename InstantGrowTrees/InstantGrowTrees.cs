using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace InstantGrowTrees
{
    /// <summary>The entry class called by SMAPI.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod configuration.</summary>
        private ModConfig Config;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();
            TimeEvents.AfterDayStarted += this.ReceiveAfterDayStarted;
        }


        /*********
        ** Private methods
        *********/
        /****
        ** Event handlers
        ****/
        /// <summary>The method called when the current day changes.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ReceiveAfterDayStarted(object sender, EventArgs e)
        {
            // When the player loads a saved game, or after the overnight save,
            // check for any trees that should be grown.
            this.GrowTrees();
        }

        /****
        ** Methods
        ****/
        /// <summary>Grow all trees eligible for growth.</summary>
        private void GrowTrees()
        {
            foreach (GameLocation location in Game1.locations)
            {
                foreach (var entry in location.terrainFeatures)
                {
                    Vector2 tile = entry.Key;
                    TerrainFeature feature = entry.Value;

                    if (this.Config.RegularTreesInstantGrow && feature is Tree)
                        this.GrowTree((Tree)feature, location, tile);
                    if (this.Config.FruitTreesInstantGrow && feature is FruitTree)
                        GrowFruitTree((FruitTree)feature, location, tile);
                }
            }
        }

        /// <summary>Grow a tree if it's eligible for growth.</summary>
        /// <param name="tree">The tree to grow.</param>
        /// <param name="location">The tree's location.</param>
        /// <param name="tile">The tree's tile position.</param>
        private void GrowTree(Tree tree, GameLocation location, Vector2 tile)
        {
            if (this.Config.RegularTreesGrowInWinter || !Game1.currentSeason.Equals("winter") || tree.treeType == Tree.palmTree)
            {
                // ignore fully-grown trees
                if (tree.growthStage >= Tree.treeStage)
                    return;

                // ignore trees on nospawn tiles
                string isNoSpawn = location.doesTileHaveProperty((int)tile.X, (int)tile.Y, "NoSpawn", "Back");
                if (isNoSpawn != null && (isNoSpawn == "All" || isNoSpawn == "Tree"))
                    return;

                // ignore blocked seeds
                if (tree.growthStage == Tree.seedStage && location.objects.ContainsKey(tile))
                    return;

                // get max growth stage
                int maxStage = Tree.treeStage;
                foreach (Vector2 surroundingTile in Utility.getSurroundingTileLocationsArray(tile))
                {
                    // get tree on this tile
                    if (!location.terrainFeatures.TryGetValue(surroundingTile, out TerrainFeature feature) || !(feature is Tree otherTree))
                        continue;

                    // check if blocking growth
                    if (otherTree.growthStage >= Tree.treeStage)
                    {
                        maxStage = Tree.treeStage - 1;
                        break;
                    }
                }

                // grow tree to max allowed
                tree.growthStage = maxStage;
            }
        }

        /// <summary>Grow a fruit tree if it's eligible for growth.</summary>
        /// <param name="tree">The tree to grow.</param>
        /// <param name="location">The tree's location.</param>
        /// <param name="tile">The tree's tile position.</param>
        private void GrowFruitTree(FruitTree tree, GameLocation location, Vector2 tile)
        {
            // ignore fully-grown trees
            if (tree.growthStage >= FruitTree.treeStage)
                return;

            // ignore if tree blocked
            foreach (Vector2 adjacentTile in Utility.getSurroundingTileLocationsArray(tile))
            {
                if (location.isTileOccupied(adjacentTile) && (!location.terrainFeatures.ContainsKey(tile) || !(location.terrainFeatures[tile] is HoeDirt) || ((HoeDirt)location.terrainFeatures[tile]).crop == null))
                    return;
            }

            // grow tree
            tree.daysUntilMature = 0;
            tree.growthStage = FruitTree.treeStage;
        }
    }
}
