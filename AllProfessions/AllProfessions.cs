using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace AllProfessions
{
    /// <summary>The entry class called by SMAPI.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>Professions to gain for each level.</summary>
        private readonly IDictionary<string, int[]> ProfessionsToGain = new Dictionary<string, int[]>
        {
            ["Farmer:5"] = new[] { 0, 1 },
            ["Farmer:10"] = new[] { 2, 3, 4, 5 },
            ["Fishing:5"] = new[] { 6, 7 },
            ["Fishing:10"] = new[] { 8, 9, 10, 11 },
            ["Foraging:5"] = new[] { 12, 13 },
            ["Foraging:10"] = new[] { 14, 15, 16, 17 },
            ["Mining:5"] = new[] { 18, 19 },
            ["Mining:10"] = new[] { 20, 21, 22, 23 },
            ["Combat:5"] = new[] { 24, 25 },
            ["Combat:10"] = new[] { 26, 27, 28, 29 }
        };


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            LocationEvents.CurrentLocationChanged += this.ReceiveCurrentLocationChanged;
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
        public void ReceiveCurrentLocationChanged(object sender, EventArgs e)
        {
            this.AddMissingProfessions();
        }

        /****
        ** Methods
        ****/
        /// <summary>Add all missing professions.</summary>
        public void AddMissingProfessions()
        {
            int[] missingProfessions = this.ProfessionsToGain.Values
                .SelectMany(p => p)
                .Except(Game1.player.professions)
                .ToArray();

            foreach (int professionID in missingProfessions)
            {
                if (!Game1.player.professions.Contains(professionID))
                {
                    // add profession
                    Game1.player.professions.Add(professionID);

                    // add health bonuses that are a special case of LevelUpMenu.getImmediateProfessionPerk
                    if (professionID == 24)
                        Game1.player.maxHealth += 15;
                    else if (professionID == 27)
                        Game1.player.maxHealth += 25;
                }
            }
        }
    }
}
