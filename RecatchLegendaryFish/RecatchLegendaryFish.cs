using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

/*
TODO:

    Add seeds to seed shop inventory.  See SeedShop.cs line 155, shopStock().
*/

namespace RecatchLegendaryFish
{
    public class RecatchLegendaryFish : Mod
    {
        public int FishCaughtCount = 0;
        public List<int> bossFishList = new List<int> { 159, 160, 163, 682, 775 };


        public override void Entry(params object[] objects)
        {
            Console.WriteLine("RecatchLegendaryFish Has Loaded");
            GameEvents.UpdateTick += Events_UpdateTick;
            GameEvents.GameLoaded += Events_GameLoaded;
        }

        void Events_GameLoaded(object sender, EventArgs e)
        {
            CheckFish();
        }


        void Events_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.player.fishCaught.Count() != FishCaughtCount)
            {
                CheckFish();
            }
        }

        void CheckFish()
        {
            ParseFishCaught();
            FishCaughtCount = Game1.player.fishCaught.Count();
        }

        void ParseFishCaught()
        {
            if (Game1.player.fishCaught != null)
            {
                //Console.WriteLine("fishCaught not null!");
                foreach (var fish in bossFishList)
                {
                    if (Game1.player.fishCaught.ContainsKey(fish))
                    {
                        Game1.player.fishCaught.Remove(fish);
                    }
                }
            }
        }


    }
}
