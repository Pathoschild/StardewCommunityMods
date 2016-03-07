/*
    Copyright 2016 cantorsdust

    Storm is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Storm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Storm.  If not, see <http://www.gnu.org/licenses/>.
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

namespace TimeSpeed
{
    [Mod(Author = "cantorsdust", Name = "TimeSpeed", Version = 1.5)]
    public class TimeSpeed : DiskResource
    {
        public Config ModConfig { get; private set; }
        public bool notUpdatedThisTick = true;

        [Subscribe]
        public void InitializeCallback(InitializeEvent @event)
        {
            var configLocation = Path.Combine(ParentPathOnDisk, "Config.json");
            if (!File.Exists(configLocation))
            {
                Console.WriteLine("The config file for TimeSpeed was not found, attempting creation...");
                ModConfig = new Config();
                ModConfig.TenMinuteTickLength = 14;
                ModConfig.ChangeTimeSpeedOnFestivalDays = false;
                File.WriteAllBytes(configLocation, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ModConfig, Formatting.Indented)));
                Console.WriteLine("The config file for TimeSpeed has been loaded. \n\tTenMinuteTickLength: {0}, ChangeTimeSpeedOnFestivalDays: {1}",
                    ModConfig.TenMinuteTickLength, ModConfig.ChangeTimeSpeedOnFestivalDays);
            }
            else
            {
                ModConfig = JsonConvert.DeserializeObject<Config>(Encoding.UTF8.GetString(File.ReadAllBytes(configLocation)));
                Console.WriteLine("The config file for TimeSpeed has been loaded.\n\tTenMinuteTickLength: {0}, ChangeTimeSpeedOnFestivalDays: {1}",
                    ModConfig.TenMinuteTickLength, ModConfig.ChangeTimeSpeedOnFestivalDays);
            }

            Console.WriteLine("TimeSpeed Initialization Completed");
        }

        [Subscribe]
        public void PerformClockUpdateCallback(BeforeClockUpdateEvent @event)
        {
            Console.WriteLine("Firing PerformClockUpdateCallback");
            notUpdatedThisTick = true;
        }

        [Subscribe]
        public void UpdateGameClockCallback(UpdateGameClockEvent @event)
        {
            if (notUpdatedThisTick)
            {
                if (@event.Root.DayOfMonth != null && @event.Root.CurrentSeason != null)
                {
                    if (!@event.Root.IsFestivalDay(@event.Root.DayOfMonth, @event.Root.CurrentSeason) || ModConfig.ChangeTimeSpeedOnFestivalDays)
                    //!@event.Root.IsFestivalDay(@event.Root.DayOfMonth, @event.Root.CurrentSeason)
                    //((!StardewValley.Utility.isFestivalDay(@event.Root.DayOfMonth, @event.Root.CurrentSeason)) || ModConfig.ChangeTimeSpeedOnFestivalDays)
                    {
                        @event.Root.GameTimeInterval += (7 - ModConfig.TenMinuteTickLength) * 1000;
                        notUpdatedThisTick = false;
                    }  
                }                         
            }
        }
    }

    public class Config
    {
        public int TenMinuteTickLength { get; set; }
        public bool ChangeTimeSpeedOnFestivalDays { get; set; }
    }
}
