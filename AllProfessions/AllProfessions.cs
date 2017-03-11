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
        /// <summary>Professions to gain for each level. Each entry represents the skill, level requirement, and profession IDs.</summary>
        private readonly Tuple<Skill, int, int[]>[] ProfessionsToGain =
        {
            Tuple.Create(Skill.Farming, 5, new[] { 0, 1 }),
            Tuple.Create(Skill.Farming, 10, new[] { 2, 3, 4, 5 }),
            Tuple.Create(Skill.Fishing, 5, new[] { 6, 7 }),
            Tuple.Create(Skill.Fishing, 10, new[] { 8, 9, 10, 11 }),
            Tuple.Create(Skill.Foraging, 5, new[] { 12, 13 }),
            Tuple.Create(Skill.Foraging, 10, new[] { 14, 15, 16, 17 }),
            Tuple.Create(Skill.Mining, 5, new[] { 18, 19 }),
            Tuple.Create(Skill.Mining, 10, new[] { 20, 21, 22, 23 }),
            Tuple.Create(Skill.Combat, 5, new[] { 24, 25 }),
            Tuple.Create(Skill.Combat, 10, new[] { 26, 27, 28, 29 })
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
        private void AddMissingProfessions()
        {
            // get missing professions
            List<int> expectedProfessions = new List<int>();
            foreach (var entry in this.ProfessionsToGain)
            {
                Skill skill = entry.Item1;
                int level = entry.Item2;
                int[] professions = entry.Item3;

                if (Game1.player.getEffectiveSkillLevel((int)skill) >= level)
                    expectedProfessions.AddRange(professions);
            }

            // add professions
            foreach (int professionID in expectedProfessions.Distinct().Except(Game1.player.professions))
            {
                // add profession
                Game1.player.professions.Add(professionID);

                // add health bonuses that are a special case of LevelUpMenu.getImmediateProfessionPerk
                switch (professionID)
                {
                    // fighter
                    case 24:
                        Game1.player.maxHealth += 15;
                        break;

                    // defender
                    case 27:
                        Game1.player.maxHealth += 25;
                        break;
                }
            }
        }
    }
}
