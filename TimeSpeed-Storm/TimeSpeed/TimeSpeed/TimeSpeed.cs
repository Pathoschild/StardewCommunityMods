/*
    Copyright 2016 cantorsdust and Syndlig

    TimeSpeed is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    TimeSpeed is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with TimeSpeed.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using Storm;
using Storm.ExternalEvent;
using Storm.StardewValley.Event;

namespace TimeSpeed
{
    [Mod]
    public class TimeSpeed : DiskResource
    {
        public static ModConfig TimeSpeedConfig { get; private set; }
        public double timeCounter = 0;
        public double lastGameTimeInterval = 0;
        public int lasttime = 600;
        public bool firsttick = true;
        public bool TimeFreezeOverride = false;

        public bool oldFreezeTimeIndoors = false;
        public bool oldFreezeTimeInMines = false;
        public bool oldFreezeTimeOutdoors = false;

        [Subscribe]
        public void InitializeCallback(InitializeEvent @event)
        {
            TimeSpeedConfig = new ModConfig();
            TimeSpeedConfig = (ModConfig)Config.InitializeConfig(PathOnDisk + "\\Config.json", TimeSpeedConfig);

            Console.WriteLine("The config file for TimeSpeed + FreezeInside has been loaded." +
                              "\n\tOutdoorTickLength:             {0}" +
                              "\n\tIndoorTickLength:              {1}" +
                              "\n\tMineTickLength:                {2}" +
                              "\n\tChangeTimeSpeedOnFestivalDays: {3}" +
                              "\n\tFreezeTimeOutdoors:            {4}" +
                              "\n\tFreezeTimeIndoors:             {5}" +
                              "\n\tFreezeTimeInMines:             {6}" +
                              "\n\tLetMachineRunsWhileTimeFrozen: {7}" +
                              "\n\tFreezeTimeAt1230AM:            {8}",
                              TimeSpeedConfig.OutdoorTickLength, TimeSpeedConfig.IndoorTickLength, TimeSpeedConfig.MineTickLength, TimeSpeedConfig.ChangeTimeSpeedOnFestivalDays,
                              TimeSpeedConfig.FreezeTimeOutdoors, TimeSpeedConfig.FreezeTimeIndoors, TimeSpeedConfig.FreezeTimeInMines, TimeSpeedConfig.LetMachinesRunWhileTimeFrozen,
                              TimeSpeedConfig.FreezeTimeAt1230AM);
            Console.WriteLine("TimeSpeed + FreezeInside Initialization Completed");
        }

        [Subscribe]
        public void PostNewDayCallback(PostNewDayEvent @event)
        {
            lasttime = 600;
            @event.Root.GameTimeInterval = 0;
            timeCounter = 0;
            lastGameTimeInterval = 0;
        }

        [Subscribe]
        public void Pre10MinuteClockUpdateCallback(Pre10MinuteClockUpdateEvent @event)
        {
            Console.WriteLine("Firing Pre10MinuteClockUpdateCallback");
            var location = @event.Root.CurrentLocation;
            timeCounter = 0;
            lastGameTimeInterval = 0;
            int time = @event.Root.TimeOfDay;

            if (location != null)
            {
                Console.WriteLine("Location name is: " + location.Name);
                Console.WriteLine("Location is outdoors is: " + location.IsOutdoors.ToString());
            }
            Console.WriteLine("time is " + time.ToString("G"));
            /*  REQUIREMENTS FOR FREEZING TIME
                IF (location is not null), AND,
                IF the difference between time and lasttime is not more than 10 minutes, 
                AND one of the following is true:
                    IF location is outdoors and FreezeTimeOutdoors == true, OR
                    IF location is not outdoors, not named "UndergroundMine", and FreezeTimeIndoors == true, OR
                    IF location is not outdoors, named "UndergroundMine", and FreezeTimeInMines == true, OR
                    IF the time is 12:30 AM and FreezeTimeAt1230AM == true
            */
            if (location != null &&
                (TimeSpeedConfig.LetMachinesRunWhileTimeFrozen) &&
                (Math.Abs(time - lasttime) <= 10) &&
                ((location.IsOutdoors && TimeSpeedConfig.FreezeTimeOutdoors) ||
                (!location.IsOutdoors && !location.Name.Equals("UndergroundMine") && TimeSpeedConfig.FreezeTimeIndoors) ||
                (!location.IsOutdoors && location.Name.Equals("UndergroundMine") && TimeSpeedConfig.FreezeTimeInMines) ||
                (time >= 2430 && TimeSpeedConfig.FreezeTimeAt1230AM)))
            {
                Console.WriteLine("location requirements met, resetting time");
                @event.Root.TimeOfDay -= (time % 100 == 0) ? 50 : 10;
                Console.WriteLine("resetting time to: " + time.ToString("G"));
            }
            else
            {
                lasttime = time;
                Console.WriteLine("location requirements not met, time advancing normally");
            }
        }

        [Subscribe]
        public void UpdateGameClockCallback(UpdateGameClockEvent @event)
        {
            var location = @event.Root.CurrentLocation;
            int time = @event.Root.TimeOfDay;
            if (location != null &&
                (!TimeSpeedConfig.LetMachinesRunWhileTimeFrozen) &&
                (Math.Abs(time - lasttime) <= 10) &&
                ((location.IsOutdoors && TimeSpeedConfig.FreezeTimeOutdoors) ||
                (!location.IsOutdoors && !location.Name.Equals("UndergroundMine") && TimeSpeedConfig.FreezeTimeIndoors) ||
                (!location.IsOutdoors && location.Name.Equals("UndergroundMine") && TimeSpeedConfig.FreezeTimeInMines) ||
                (time >= 2430 && TimeSpeedConfig.FreezeTimeAt1230AM)))
            {
                @event.Root.GameTimeInterval = 0;
            }
            else if (@event.Root.CurrentSeason != null && (!@event.Root.IsFestivalDay(@event.Root.DayOfMonth, @event.Root.CurrentSeason) || TimeSpeedConfig.ChangeTimeSpeedOnFestivalDays))
            {
                timeCounter += Math.Abs((@event.Root.GameTimeInterval - lastGameTimeInterval));
                double proportion;

                if (@event.Root.CurrentLocation.IsOutdoors)
                    proportion = Math.Abs(7 * timeCounter / TimeSpeedConfig.OutdoorTickLength);
                else if (@event.Root.CurrentLocation.Name == "UndergroundMine")
                    proportion = Math.Abs(7 * timeCounter / TimeSpeedConfig.MineTickLength);
                else
                    proportion = Math.Abs(7 * timeCounter / TimeSpeedConfig.IndoorTickLength);

                @event.Root.GameTimeInterval = Convert.ToInt32(proportion);
                lastGameTimeInterval = @event.Root.GameTimeInterval;
            }
        }

        [Subscribe]
        public void KeyPressedCallback(KeyPressedEvent @event)
        {
            var key = @event.Key;
            //Console.WriteLine("Key Pressed: " + key.ToString());

            // N toggles freeze time override, freezing time everywhere.  hitting it again restores old values.
            if (key.ToString().Equals("N"))
            {
                if (!TimeFreezeOverride)
                {
                    TimeFreezeOverride = true;
                    oldFreezeTimeIndoors = TimeSpeedConfig.FreezeTimeIndoors;
                    oldFreezeTimeInMines = TimeSpeedConfig.FreezeTimeInMines;
                    oldFreezeTimeOutdoors = TimeSpeedConfig.FreezeTimeOutdoors;
                    TimeSpeedConfig.FreezeTimeOutdoors = true;
                    TimeSpeedConfig.FreezeTimeIndoors = true;
                    TimeSpeedConfig.FreezeTimeInMines = true;
                }
                else
                {
                    TimeFreezeOverride = false;
                    TimeSpeedConfig.FreezeTimeOutdoors = oldFreezeTimeOutdoors;
                    TimeSpeedConfig.FreezeTimeIndoors = oldFreezeTimeIndoors;
                    TimeSpeedConfig.FreezeTimeInMines = oldFreezeTimeInMines;
                }
            }
            // , halves current tick lengths
            else if (key.ToString().Equals("OemComma"))
            {
                TimeSpeedConfig.OutdoorTickLength = (TimeSpeedConfig.OutdoorTickLength / 2);
                TimeSpeedConfig.IndoorTickLength = (TimeSpeedConfig.IndoorTickLength / 2);
                TimeSpeedConfig.MineTickLength = (TimeSpeedConfig.MineTickLength / 2);
            }
            // . doubles current tick lengths
            else if (key.ToString().Equals("OemPeriod"))
            {
                TimeSpeedConfig.OutdoorTickLength = (TimeSpeedConfig.OutdoorTickLength * 2);
                TimeSpeedConfig.IndoorTickLength = (TimeSpeedConfig.IndoorTickLength * 2);
                TimeSpeedConfig.MineTickLength = (TimeSpeedConfig.MineTickLength * 2);
            }
            // / restores tick lengths to the original ini values
            else if (key.ToString().Equals("B"))
            {
                TimeSpeedConfig = (ModConfig)Config.InitializeConfig(PathOnDisk + "\\Config.json", TimeSpeedConfig);
            }
        }
    }

    public class ModConfig : Config
    {
        public float OutdoorTickLength { get; set; }
        public float IndoorTickLength { get; set; }
        public float MineTickLength { get; set; }
        public bool ChangeTimeSpeedOnFestivalDays { get; set; }
        public bool FreezeTimeOutdoors { get; set; }
        public bool FreezeTimeIndoors { get; set; }
        public bool FreezeTimeInMines { get; set; }
        public bool LetMachinesRunWhileTimeFrozen { get; set; }
        public bool FreezeTimeAt1230AM { get; set; }

        public override Config GenerateBaseConfig(Config baseConfig)
        {
            OutdoorTickLength = 14;
            IndoorTickLength = 14;
            MineTickLength = 14;
            ChangeTimeSpeedOnFestivalDays = false;
            FreezeTimeOutdoors = false;
            FreezeTimeIndoors = false;
            FreezeTimeInMines = false;
            LetMachinesRunWhileTimeFrozen = false;
            FreezeTimeAt1230AM = false;

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