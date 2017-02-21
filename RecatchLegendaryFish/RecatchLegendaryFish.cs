using System;
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
        /// <summary>The number of fish previously caught by the player.</summary>
        private int FishCaughtCount;

        /// <summary>The fish IDs for legendary fish.</summary>
        private readonly int[] LegendaryFishIDs = { 159, 160, 163, 682, 775 };


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.UpdateTick += this.ReceiveUpdateTick;
        }


        /*********
        ** Private methods
        *********/
        /****
        ** Event handlers
        ****/
        /// <summary>The method called when the game updates state.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        void ReceiveUpdateTick(object sender, EventArgs e)
        {
            if (Game1.hasLoadedGame && Game1.player.fishCaught.Count != FishCaughtCount)
                this.MarkLegendaryFishUncaught();
        }

        /****
        ** Methods
        ****/
        /// <summary>Mark all legendary fish uncaught.</summary>
        private void MarkLegendaryFishUncaught()
        {
            // mark legendary fish uncaught
            foreach (int fishID in this.LegendaryFishIDs)
            {
                if (Game1.player.fishCaught.ContainsKey(fishID))
                    Game1.player.fishCaught.Remove(fishID);
            }

            // update fish count
            this.FishCaughtCount = Game1.player.fishCaught.Count;
        }
    }
}
