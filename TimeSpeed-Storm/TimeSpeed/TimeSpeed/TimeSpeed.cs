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
using Storm;
using Storm.ExternalEvent;
using Storm.StardewValley;
using Storm.StardewValley.Accessor;
using Storm.StardewValley.Event;
using Storm.StardewValley.Wrapper;

namespace TimeSpeed
{
    [Mod]
    public class TimeSpeed : DiskResource
    {
        public static ModConfig TimeSpeedConfig { get; private set; }
        public bool notUpdatedThisTick = true;
        public double timeCounter = 0;
        public double lastGameTimeInterval = 0;
        //public double counter = 0;

        [Subscribe]
        public void InitializeCallback(InitializeEvent @event)
        {
            TimeSpeedConfig = new ModConfig();
            TimeSpeedConfig = (ModConfig)Config.InitializeConfig(PathOnDisk + "\\Config.json", TimeSpeedConfig);


            /*
            Old config code

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
            */
            Console.WriteLine("The config file for TimeSpeed has been loaded. \n\tTenMinuteTickLength: {0}, ChangeTimeSpeedOnFestivalDays: {1}",
                    TimeSpeedConfig.TenMinuteTickLength, TimeSpeedConfig.ChangeTimeSpeedOnFestivalDays);
            Console.WriteLine("TimeSpeed Initialization Completed");
        }

        [Subscribe]
        public void Pre10MinuteClockUpdateCallback(Pre10MinuteClockUpdateEvent @event)
        {
            Console.WriteLine("Firing Pre10MinuteClockUpdateCallback");
            timeCounter = 0;
            lastGameTimeInterval = 0;
            //counter = 0;
        }

        [Subscribe]
        public void UpdateGameClockCallback(UpdateGameClockEvent @event)
        {
            if (notUpdatedThisTick)
            {
                if (@event.Root.DayOfMonth != null && @event.Root.CurrentSeason != null)
                {
                    if (!@event.Root.IsFestivalDay(@event.Root.DayOfMonth, @event.Root.CurrentSeason) || TimeSpeedConfig.ChangeTimeSpeedOnFestivalDays)
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

                        //timeCounter += Math.Abs((@event.Root.GameTimeInterval - lastGameTimeInterval)); //time that has passed since last check
                        timeCounter += Math.Abs((@event.Root.GameTimeInterval - lastGameTimeInterval));
                        double proportion = Math.Abs(7 * timeCounter / (TimeSpeedConfig.TenMinuteTickLength));
                        @event.Root.GameTimeInterval = Convert.ToInt32(proportion);
                        lastGameTimeInterval = @event.Root.GameTimeInterval;
                        /*
                        if (counter % 10 == 0)
                        {
                            Console.WriteLine("gameTimeInterval: " + @event.Root.GameTimeInterval.ToString());
                            Console.WriteLine("timeCounter: " + timeCounter.ToString());
                            Console.WriteLine("proportion: " + proportion.ToString());
                            Console.WriteLine("lastGameTimeInterval: " + lastGameTimeInterval.ToString());
                        }
                        */
                        //counter here for watching what was happening to gameTimeInterval during testing
                    }  
                }                         
            }
        }
    }

    public class ModConfig : Config
    {
        public int TenMinuteTickLength { get; set; }
        public bool ChangeTimeSpeedOnFestivalDays { get; set; }

        public override Config GenerateBaseConfig(Config baseConfig)
        {
            TenMinuteTickLength = 14;
            ChangeTimeSpeedOnFestivalDays = false;
            
            return this;
        }
    }
}

/*
7:25 PM <•Zoryn> whoever wanted to be able to read/write configs easily, you can call <YourConfigClass>.WriteConfig() to output it
7:25 PM <•cantorsdust> yay!
7:25 PM <•cantorsdust> that will write the current variables to the config?
7:25 PM <•Zoryn> yes
7:25 PM <•cantorsdust> sweet, thanks
7:26 PM <•Zoryn> to reload it back in do <YourConfigInstance> = <YourConfigInstance>.ReloadConfig()

    As<HoeDirtAccessor, HoeDirt>
*/

//var dirt = feature.As<HoeDirt, HoeDirtAccessor>()