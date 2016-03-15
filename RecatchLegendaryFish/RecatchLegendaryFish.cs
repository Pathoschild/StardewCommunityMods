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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.ExternalEvent;
using Storm.StardewValley;
using Storm.StardewValley.Event;
using Storm.StardewValley.Wrapper;
using Storm;

namespace RecatchLegendaryFish
{
    [Mod]
    public class RecatchLegendaryFish : DiskResource
    {
        //public static ModConfig RecatchLegendaryFishConfig { get; private set; }
        public List<int> bossFishList = new List<int> { 159, 160, 163, 682, 775 };
        [Subscribe]
        public void InitializeCallback(InitializeEvent @event)
        {
            /*
            RecatchLegendaryFishConfig = new ModConfig();
            RecatchLegendaryFishConfig = (ModConfig)Config.InitializeConfig(PathOnDisk + "\\Config.json", RecatchLegendaryFishConfig);
            Console.WriteLine("The config file for RecatchLegendaryFish has been loaded. \n\tFreezeTimeInMines: {0}, LetMachinesRunWhileTimeFrozen: {1}",
                    RecatchLegendaryFishConfig.FreezeTimeInMines, RecatchLegendaryFishConfig.LetMachinesRunWhileTimeFrozen);
            */
            Console.WriteLine("RecatchLegendaryFish Initialization Completed");
        }

        [Subscribe]
        public void PostCatchFishCallback(PostFarmerCaughtFishEvent @event)
        {
            {
                @event.Farmer.FishCaught.Remove(@event.Index);
                Console.WriteLine("Removed caught legendary fish");
            }
            foreach (var fish in bossFishList)
            {
                if (@event.Farmer.FishCaught.Keys.Contains(fish))
                {
                    @event.Farmer.FishCaught.Remove(fish);
                    Console.WriteLine("Removed caught legendary fish");
                }
            }
        }
        /*
        [Subscribe]
        public void Pre10MinuteClockUpdateCallback(Pre10MinuteClockUpdateEvent @event)
        {
            Console.WriteLine("Firing Pre10MinuteClockUpdateCallback");
            if (@event.Root.Player.FishCaught != null)
            {
                foreach (var fish in @event.Root.Player.FishCaught.Keys)
                {
                    Console.WriteLine("Fish caught include: " + fish.ToString("G"));
                }
                Console.WriteLine("All fish printed");
            }
            else
            {
                Console.WriteLine("fishCaught is null!");
            }
            
        }
        */



    }
    /*
    public class ModConfig : Config
    {
        public bool FreezeTimeInMines { get; set; }
        public bool LetMachinesRunWhileTimeFrozen { get; set; }

        public override Config GenerateBaseConfig(Config baseConfig)
        {
            FreezeTimeInMines = false;
            LetMachinesRunWhileTimeFrozen = true;

            return this;
        }
    }
    */
}
