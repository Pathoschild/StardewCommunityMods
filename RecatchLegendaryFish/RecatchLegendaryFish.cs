using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace RecatchLegendaryFish
{
    /// <summary>The entry class called by SMAPI.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>Get whether the mod is loaded and ready to intercept catches.</summary>
        private bool IsLoaded;

        /// <summary>The number of unique fish previously caught by the player.</summary>
        private int FishTypesCaught;

        /// <summary>The fish IDs for legendary fish.</summary>
        private readonly int[] LegendaryFishIDs = { 159, 160, 163, 682, 775 };

        /// <summary>A backup of legendaries caught by the current player.</summary>
        private readonly IDictionary<int, int[]> Stash = new Dictionary<int, int[]>();


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            SaveEvents.BeforeSave += this.SaveEvents_BeforeSave;
            SaveEvents.AfterSave += this.SaveEvents_AfterSave;
            SaveEvents.AfterReturnToTitle += this.SaveEvents_AfterReturnToTitle;

            GameEvents.UpdateTick += this.ReceiveUpdateTick;
        }

        
        /*********
        ** Private methods
        *********/
        /****
        ** Event handlers
        ****/
        /// <summary>The method called after the player loads the game.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            // reset legendary catches when a game starts
            this.Stash.Clear();
            this.StashLegendaries();
            this.FishTypesCaught = Game1.player.fishCaught.Count;
            this.IsLoaded = true;
        }

        /// <summary>The method called before the player saves the game.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            // restore legendaries before save
            this.RestoreLegendaries();
        }

        /// <summary>The method called after the player saves the game.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SaveEvents_AfterSave(object sender, EventArgs e)
        {
            // remove legendaries after save
            this.StashLegendaries();
        }

        /// <summary>The method called after the player exits to the title screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SaveEvents_AfterReturnToTitle(object sender, EventArgs e)
        {
            this.IsLoaded = false;
        }

        /// <summary>The method called when the game updates state.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        void ReceiveUpdateTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame || !this.IsLoaded)
                return;

            // intercept any caught legendaries
            if (Game1.player.fishCaught.Count != this.FishTypesCaught)
            {
                // trigger achievement check
                this.RestoreLegendaries();
                Game1.stats.checkForFishingAchievements();

                // mark legendaries uncaught
                this.StashLegendaries();

                // update number caught
                this.FishTypesCaught = Game1.player.fishCaught.Count;
            }
        }

        /****
        ** Methods
        ****/
        /// <summary>Mark all legendary fish uncaught, stashing the previous catch data for safekeeping.</summary>
        private void StashLegendaries()
        {
            foreach (int fishID in this.LegendaryFishIDs)
            {
                if (Game1.player.fishCaught.TryGetValue(fishID, out int[] freshValues))
                {
                    // remember new catches
                    this.Stash[fishID] = this.Stash.TryGetValue(fishID, out int[] stashedValues)
                        ? stashedValues.Union(freshValues).ToArray()
                        : freshValues;

                    // mark uncaught
                    Game1.player.fishCaught.Remove(fishID);
                }
            }
        }

        /// <summary>Restore any previous legendary catches from the stash.</summary>
        private void RestoreLegendaries()
        {
            foreach (int fishID in this.Stash.Keys)
            {
                Game1.player.fishCaught[fishID] = Game1.player.fishCaught.TryGetValue(fishID, out int[] currentValues)
                    ? this.Stash[fishID].Union(currentValues).ToArray()
                    : this.Stash[fishID];
            }
            this.Stash.Clear();
        }
    }
}
