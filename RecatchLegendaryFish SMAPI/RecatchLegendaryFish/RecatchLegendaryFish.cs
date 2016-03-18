/*
    Copyright 2016 cantorsdust

    RecatchLegendaryFish is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    RecatchLegendaryFish is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with RecatchLegendaryFish.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
//using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using System.Threading;
using StardewValley;

/*
TODO:

    Add seeds to seed shop inventory.  See SeedShop.cs line 155, shopStock().
*/

namespace RecatchLegendaryFish
{
    public class RecatchLegendaryFish : Mod
    {
        public override string Name
        {
            get { return "RecatchLegendaryFish"; }
        }

        public override string Authour
        {
            get { return "cantorsdust"; }
        }

        public override string Version
        {
            get { return "1.0.0.0.SMAPI"; }
        }

        public override string Description
        {
            get { return "Allows legendary fish to be caught again and again."; }
        }

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
                Console.WriteLine("fishCaught not null!");
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
