/*
    Copyright 2016 cantorsdust

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
using System.Threading;
using System.IO;
using StardewValley;
//using System.Configuration;
//using System.Web.Script.Serialization;

namespace TimeSpeed
{
    public class TimeSpeed : Mod
    {
        public override string Name
        {
            get { return "TimeSpeed Mod"; }
        }

        public override string Authour
        {
            get { return "cantorsdust"; }
        }

        public override string Version
        {
            get { return "1.4"; }
        }

        public override string Description
        {
            get { return "Allows for a configurable day length."; }
        }

        public float OutdoorTickLength = 14;
        public float IndoorTickLength = 14;
        public float MineTickLength = 14;
        public bool ChangeTimeSpeedOnFestivalDays = false;
        
        public bool FreezeTimeOutdoors = false;
        public bool FreezeTimeIndoors = false;
        public bool FreezeTimeInMines = false;
        public bool LetMachinesRunWhileTimeFrozen = false;
        public bool FreezeTimeAt1230AM = false;
        

        public double timeCounter = 0;
        public double lastGameTimeInterval = 0;
        public int lasttime = 600;
        //public bool firsttick = true;

        public override void Entry(params object[] objects)
        {
            runConfig();
            Console.WriteLine("TimeSpeed Has Loaded");
            TimeEvents.DayOfMonthChanged += Events_NewDay;
            TimeEvents.TimeOfDayChanged += Events_TimeChanged;
            GameEvents.UpdateTick += Events_UpdateTick;


        }
        
        void runConfig()
        {            
            string ConfigPathAppData = Environment.ExpandEnvironmentVariables("%AppData%\\StardewValley\\Mods\\TimeSpeedConfig.ini");
            string ConfigPathSVMods = "Mods\\TimeSpeedConfig.ini";
            string DLLPathAppData = Environment.ExpandEnvironmentVariables("%AppData%\\StardewValley\\Mods\\TimeSpeed.dll");
            string DLLPathSVMods = "Mods\\TimeSpeed.dll";
            string path = null;
            char[] delimiterChars = { '=' };
            if (File.Exists(ConfigPathAppData))
            {
                Console.WriteLine("found INI in %appdata%");
                path = ConfigPathAppData;
            }
            else if (File.Exists(ConfigPathSVMods))
            {
                Console.WriteLine("found INI in Stardew Valley-Mods");
                path = ConfigPathSVMods;
            }
            else
            {
                Console.WriteLine("WARNING:  Could not find INI.  Writing new INI with default values next to DLL");
                if (File.Exists(DLLPathAppData))
                {
                    File.AppendAllLines(ConfigPathAppData, new[] { "OutdoorTickLength=14", "IndoorTickLength=14", "MineTickLength=14", "ChangeTimeSpeedOnFestivalDays=false",
                    "FreezeTimeOutdoors=false", "FreezeTimeIndoors=false", "FreezeTimeInMines=false", "LetMachinesRunWhileTimeFrozen=false", "FreezeTimeAt1230AM=false" });
                    
                }
                else if (File.Exists(DLLPathSVMods))
                {
                    File.AppendAllLines(ConfigPathSVMods, new[] { "OutdoorTickLength=14", "IndoorTickLength=14", "MineTickLength=14", "ChangeTimeSpeedOnFestivalDays=false",
                    "FreezeTimeOutdoors=false", "FreezeTimeIndoors=false", "FreezeTimeInMines=false", "LetMachinesRunWhileTimeFrozen=false", "FreezeTimeAt1230AM=false" });
                }

            }

            if (path != null)
            {
                List<string> fileData = File.ReadAllLines(path).ToList();
                //each fileData[index] is a line
                //we'll check to see what each line holds and parse that line's data
                //if we see old values (TenMinuteTickLength), we will replace that line with new ones
                bool rewriteini = false;
                //int index = 0;
                foreach (string line in fileData)
                {
                    //Console.WriteLine("Parsing INI line " + index.ToString("g"));
                    string[] words = line.Split(delimiterChars);
                    //make sure you've fixed old INIs
                    if (words[0].Contains("TenMinuteTickLength"))
                    {
                        int TenMinuteTickLength = 14;
                        int.TryParse(words[1], out TenMinuteTickLength);
                        OutdoorTickLength = TenMinuteTickLength;
                        IndoorTickLength = TenMinuteTickLength;
                        MineTickLength = TenMinuteTickLength;
                        fileData.Remove(line);
                        fileData.Insert(0, ("MineTickLength=" + TenMinuteTickLength.ToString("g")));
                        fileData.Insert(0, ("IndoorTickLength=" + TenMinuteTickLength.ToString("g")));
                        fileData.Insert(0, ("OutdoorTickLength=" + TenMinuteTickLength.ToString("g")));
                        fileData.Add("ChangeTimeSpeedOnFestivalDays=" + ChangeTimeSpeedOnFestivalDays.ToString());
                        
                        fileData.Add("FreezeTimeOutdoors=" + FreezeTimeOutdoors.ToString());
                        fileData.Add("FreezeTimeIndoors=" + FreezeTimeIndoors.ToString());
                        fileData.Add("FreezeTimeInMines=" + FreezeTimeInMines.ToString());
                        fileData.Add("LetMachinesRunWhileTimeFrozen=" + LetMachinesRunWhileTimeFrozen.ToString());
                        fileData.Add("FreezeTimeAt1230AM=" + FreezeTimeAt1230AM.ToString());
                        
                        rewriteini = true;
                    }
                    if (words[0].Contains("OutdoorTickLength"))
                    {
                        float.TryParse(words[1], out OutdoorTickLength);
                        if (OutdoorTickLength <= 0)
                        {
                            Console.WriteLine("WARNING:  Cannot set OutdoorTickLength to 0 or less.  Resetting to default 14.");
                            OutdoorTickLength = 14;
                        }
                        Console.WriteLine("OutdoorTickLength is " + OutdoorTickLength.ToString("g"));
                    }
                    else if (words[0].Contains("IndoorTickLength"))
                    {
                        float.TryParse(words[1], out IndoorTickLength);
                        if (IndoorTickLength <= 0)
                        {
                            Console.WriteLine("WARNING:  Cannot set IndoorTickLength to 0 or less.  Resetting to default 14.");
                            IndoorTickLength = 14;
                        }
                        Console.WriteLine("IndoorTickLength is " + IndoorTickLength.ToString("g"));
                    }
                    else if (words[0].Contains("MineTickLength"))
                    {
                        float.TryParse(words[1], out MineTickLength);
                        if (MineTickLength <= 0)
                        {
                            Console.WriteLine("WARNING:  Cannot set MineTickLength to 0 or less.  Resetting to default 14.");
                            MineTickLength = 14;
                        }
                        Console.WriteLine("MineTickLength is " + MineTickLength.ToString("g"));
                    }
                    else if (words[0].Contains("ChangeTimeSpeedOnFestivalDays"))
                    {
                        bool.TryParse(words[1], out ChangeTimeSpeedOnFestivalDays);
                        Console.WriteLine("ChangeTimeSpeedOnFestivalDays is " + ChangeTimeSpeedOnFestivalDays.ToString());
                    }

                    else if (words[0].Contains("FreezeTimeOutdoors"))
                    {
                        bool.TryParse(words[1], out FreezeTimeOutdoors);
                        Console.WriteLine("FreezeTimeOutdoors is " + FreezeTimeOutdoors.ToString());
                    }
                    else if (words[0].Contains("FreezeTimeIndoors"))
                    {
                        bool.TryParse(words[1], out FreezeTimeIndoors);
                        Console.WriteLine("FreezeTimeIndoors is " + FreezeTimeIndoors.ToString());
                    }
                    else if (words[0].Contains("FreezeTimeInMines"))
                    {
                        bool.TryParse(words[1], out FreezeTimeInMines);
                        Console.WriteLine("FreezeTimeInMines is " + FreezeTimeInMines.ToString());
                    }
                    else if (words[0].Contains("LetMachinesRunWhileTimeFrozen"))
                    {
                        bool.TryParse(words[1], out LetMachinesRunWhileTimeFrozen);
                        Console.WriteLine("LetMachinesRunWhileTimeFrozen is " + LetMachinesRunWhileTimeFrozen.ToString());
                    }
                    else if (words[0].Contains("FreezeTimeAt1230AM"))
                    {
                        bool.TryParse(words[1], out FreezeTimeAt1230AM);
                        Console.WriteLine("FreezeTimeAt1230AM is " + FreezeTimeAt1230AM.ToString());
                    }
                    
                }
                if (rewriteini)
                {
                    File.Delete(path);
                    if (path == ConfigPathAppData)
                    {
                        File.AppendAllLines(ConfigPathAppData, fileData);
                    }
                    else if (path == ConfigPathSVMods)
                    {
                        File.AppendAllLines(ConfigPathSVMods, fileData);
                    }   
                }
            }
        }

        public void Events_NewDay(object sender, EventArgs e)
        {
            Game1.gameTimeInterval = 0;
            timeCounter = 0;
            lastGameTimeInterval = 0;
        }

        void Events_UpdateTick(object sender, EventArgs e)
        {
            
            if (Game1.currentSeason != null && 
                Game1.currentLocation != null &&
                (!Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason) || ChangeTimeSpeedOnFestivalDays))
            {
                GameLocation location = Game1.currentLocation;
                int time = Game1.timeOfDay;
                
                if (!LetMachinesRunWhileTimeFrozen &&
                (location != null) &&
                ((location.isOutdoors && FreezeTimeOutdoors) ||
                (!location.isOutdoors && !location.name.Equals("UndergroundMine") && FreezeTimeIndoors) ||
                (!location.isOutdoors && location.name.Equals("UndergroundMine") && FreezeTimeInMines) ||
                (time >= 2430 && FreezeTimeAt1230AM)))
                {
                    Game1.gameTimeInterval = 0;
                    Console.WriteLine("Freeze Time without machines requirements met");
                }
                else
                {
                    Console.WriteLine("Freeze Time without machines requirements NOT met");
                    timeCounter += Math.Abs((Game1.gameTimeInterval - lastGameTimeInterval));
                    double proportion;

                    if (Game1.currentLocation.isOutdoors)
                    {
                        proportion = Math.Abs(7 * timeCounter / OutdoorTickLength);
                    }
                    else if (Game1.currentLocation.name == "UndergroundMine")
                    {
                        proportion = Math.Abs(7 * timeCounter / MineTickLength);
                    }
                    else
                    {
                        proportion = Math.Abs(7 * timeCounter / IndoorTickLength);
                    }


                    Game1.gameTimeInterval = Convert.ToInt32(proportion);
                    lastGameTimeInterval = Game1.gameTimeInterval;
                    /*
                    Console.WriteLine(Game1.gameTimeInterval.ToString("g"));
                    Console.WriteLine(lastGameTimeInterval.ToString("g"));
                    Console.WriteLine(timeCounter.ToString("g"));
                    Console.WriteLine(proportion.ToString("g"));
                    */
                }
            }            
        }

        void Events_TimeChanged(object sender, EventArgsIntChanged e)     
        {
            Console.WriteLine("Firing Pre10MinuteClockUpdateCallback");
            var location = Game1.currentLocation;
            timeCounter = 0;
            lastGameTimeInterval = 0;
            Game1.gameTimeInterval = 0;
            int time = Game1.timeOfDay;

            if (location != null)
            {
                Console.WriteLine("Location name is: " + location.name);
                Console.WriteLine("Location is outdoors is: " + location.isOutdoors.ToString());
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
            if (LetMachinesRunWhileTimeFrozen &&
                (location != null) &&
                (Math.Abs(time - lasttime) <= 10) &&
                ((location.isOutdoors && FreezeTimeOutdoors) ||
                (!location.isOutdoors && !location.name.Equals("UndergroundMine") && FreezeTimeIndoors) ||
                (!location.isOutdoors && location.name.Equals("UndergroundMine") && FreezeTimeInMines) ||
                (time >= 2430 && FreezeTimeAt1230AM)))
            {
                Console.WriteLine("location requirements met, resetting time");
                Game1.timeOfDay -= (time % 100 == 0) ? 50 : 10;
                Console.WriteLine("resetting time to: " + time.ToString("G"));
            }
            else
            {
                lasttime = time;
                Console.WriteLine("location requirements not met, time advancing normally");
                //Thread.Sleep(5000);
            }
        }                                
    }
}
