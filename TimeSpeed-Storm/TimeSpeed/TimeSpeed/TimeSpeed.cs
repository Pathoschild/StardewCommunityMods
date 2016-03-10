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
    [Mod]
    public class TimeSpeed : DiskResource
    {
        public Config ModConfig { get; private set; }
        public bool notUpdatedThisTick = true;
        public int timeCounter = 0;
        public int lastGameTimeInterval = 0;

        [Subscribe]
        public void InitializeCallback(InitializeEvent @event)
        {
            var configLocation = Path.Combine(PathOnDisk, "Config.json");
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
        public void Pre10MinuteClockUpdateCallback(Pre10MinuteClockUpdateEvent @event)
        {
            Console.WriteLine("Firing Pre10MinuteClockUpdateCallback");
            timeCounter = 0;
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
                        /*
                            new idea for smooth timer:

                            10 minute tick begins, gameTimeInterval 0.
                            New update--gameTimeInterval is now some value.
                            We have a seperate variable, myGameTimeInterval, that we add that value to.
                            Then we adjust gameTimeInterval to be proportional to the actual 10minuteticklength
                            so, like, if the length is 14, that's 2x slower, so we halve gameTimeInterval
                            That should make things like lighting and the clock smooth.
                            Hope it doesn't break animations or anything that would rely on it not going backwards.
                        */

                        timeCounter += (@event.Root.GameTimeInterval - lastGameTimeInterval); //time that has passed since last check
                        @event.Root.GameTimeInterval = (7000 * timeCounter / (1000 * ModConfig.TenMinuteTickLength));
                        lastGameTimeInterval = @event.Root.GameTimeInterval;
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