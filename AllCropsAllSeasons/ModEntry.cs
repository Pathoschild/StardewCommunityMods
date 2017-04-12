using System;
using System.Collections.Generic;
using System.Linq;
using AllCropsAllSeasons.Framework;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;

namespace AllCropsAllSeasons
{
    /// <summary>The entry class called by SMAPI.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The hoed dirt tiles which should be saved for the next day.</summary>
        private TileState[] SavedTiles = new TileState[0];


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            LocationEvents.CurrentLocationChanged += this.ReceiveCurrentLocationChanged;
            SaveEvents.BeforeSave += this.ReceiveBeforeSave;
        }


        /*********
        ** Private methods
        *********/
        /****
        ** Event handlers
        ****/
        /// <summary>The method called when the player warps to a new location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ReceiveCurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {
            // when player enters farmhouse (including on new day), back up crops in case they're
            // about to end the day
            if (e.NewLocation is FarmHouse)
                this.SavedTiles = this.GetCropTiles(Game1.getFarm()).ToArray();
        }

        /// <summary>The method called when the player warps to a new location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ReceiveBeforeSave(object sender, EventArgs e)
        {
            // before save (but after tomorrow's day updates), fix any crops that died due to the day update
            this.RestoreCrops(this.SavedTiles);
        }

        /****
        ** Methods
        ****/
        /// <summary>Restore the temporarily-saved crops.</summary>
        /// <param name="tiles">The crops to restore.</param>
        private void RestoreCrops(TileState[] tiles)
        {
            if (!tiles.Any())
                return;

            GameLocation farm = Game1.getFarm();
            GameLocation greenhouse = Game1.getLocationFromName("Greenhouse");
            foreach (TileState saved in tiles)
            {
                // get actual tile
                if (!farm.terrainFeatures.ContainsKey(saved.Tile) || !(farm.terrainFeatures[saved.Tile] is HoeDirt))
                    farm.terrainFeatures[saved.Tile] = new HoeDirt(); // reset dirt tile if needed (e.g. to clear debris)
                HoeDirt dirt = (HoeDirt)farm.terrainFeatures[saved.Tile];

                // reset crop tile if needed
                if (dirt.crop == null || dirt.crop.dead)
                {
                    // reset values changed by day update
                    if (dirt.state != HoeDirt.watered)
                        dirt.state = saved.State;
                    dirt.fertilizer = saved.Fertilizer;
                    dirt.crop = saved.Crop;
                    dirt.crop.dead = false;
                    dirt.dayUpdate(greenhouse, saved.Tile);
                }
            }
        }

        /// <summary>Get all tiles on the farm with a live crop.</summary>
        /// <param name="farm">The farm to search.</param>
        private IEnumerable<TileState> GetCropTiles(Farm farm)
        {
            foreach (KeyValuePair<Vector2, TerrainFeature> entry in farm.terrainFeatures)
            {
                Vector2 tile = entry.Key;
                HoeDirt dirt = entry.Value as HoeDirt;
                Crop crop = dirt?.crop;
                if (crop != null && !crop.dead)
                    yield return new TileState(tile, crop, dirt.state, dirt.fertilizer);
            }
        }
    }
}
