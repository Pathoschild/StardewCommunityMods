using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
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
        private readonly IDictionary<Vector2, HoeDirt> SavedTiles = new Dictionary<Vector2, HoeDirt>();

        /// <summary>The crops which should be saved for the next day.</summary>
        private readonly IDictionary<Vector2, Crop> SavedCrops = new Dictionary<Vector2, Crop>();


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            LocationEvents.CurrentLocationChanged += this.ReceiveCurrentLocationChanged;
            TimeEvents.DayOfMonthChanged += this.ReceiveDayChanged;
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
            GameLocation fromLocation = e.PriorLocation ?? Game1.getLocationFromName("FarmHouse");
            if ((Game1.dayOfMonth == 28 && Game1.currentSeason == "fall") || (Game1.currentSeason == "winter" && Game1.dayOfMonth != 28))
            {
                if (fromLocation is Farm)
                    this.SaveCurrentCrops(); //if player has come from farm into farmhouse, we're assuming they're going to bed, bad assumption, i know
            }
        }

        /// <summary>The method called when the player warps to a new location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ReceiveDayChanged(object sender, EventArgsIntChanged e)
        {
            // day is changed, safe again, now we go back to reset the crops properly
            if (Game1.currentSeason != null && Game1.currentSeason.Equals("winter"))
                this.RestoreCurrentCrops();

        }

        /****
        ** Methods
        ****/
        /// <summary>Save the current crops temporarily.</summary>
        /// <remarks>TODO: Save crops to file for saved game because they'll be deleted upon save! Aaaaa!</remarks>
        private void SaveCurrentCrops()
        {
            GameLocation farm = Game1.getFarm();
            this.SavedTiles.Clear();
            this.SavedCrops.Clear();

            //now get a list of HoeDirts with crops on them and save them in savedhoedirts
            foreach (KeyValuePair<Vector2, TerrainFeature> entry in farm.terrainFeatures)
            {
                Vector2 tile = entry.Key;
                HoeDirt dirt = entry.Value as HoeDirt;
                if (dirt != null)
                {
                    this.SavedTiles.Add(tile, dirt);
                    if (dirt.crop != null)
                        this.SavedCrops.Add(tile, dirt.crop);
                }
            }
        }

        /// <summary>Restore the temporarily-saved crops.</summary>
        private void RestoreCurrentCrops()
        {
            GameLocation farm = Game1.getFarm();
            foreach (var entry in this.SavedTiles)
            {
                // update saved crops as if they were in a greenhouse
                // entry.Value.dayUpdate(greenhouse, entry.Key);

                // now restore crops to proper location
                // if for whatever reason something is in the way of what we're restoring, delete it
                if (farm.terrainFeatures.ContainsKey(entry.Key))
                    farm.terrainFeatures.Remove(entry.Key);

                farm.terrainFeatures.Add(entry.Key, entry.Value);
                if (this.SavedCrops.ContainsKey(entry.Key))
                {
                    HoeDirt dirt = (HoeDirt)farm.terrainFeatures[entry.Key];
                    dirt.crop = this.SavedCrops[entry.Key];
                }
            }
        }
    }
}
