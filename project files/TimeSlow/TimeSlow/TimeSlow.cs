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
using System.Threading;
//using System.Configuration;
//using System.Web.Script.Serialization;

namespace TimeSlow
{
    public class TimeSlow : Mod
    {
        public override string Name
        {
            get { return "Time Slow Mod"; }
        }

        public override string Authour
        {
            get { return "cantorsdust"; }
        }

        public override string Version
        {
            get { return "1.0"; }
        }

        public override string Description
        {
            get { return "Allows for a configurable day length."; }
        }

        public int DayLength;
        public bool FreezeTimeInstalled = (System.IO.File.Exists(Environment.ExpandEnvironmentVariables("%AppData%\\StardewValley\\Mods\\FreezeInside.dll")) || System.IO.File.Exists("Mods\\FreezeInside.dll"));
        public bool FreezeTimeInMines = false;

        public override void Entry()
        {
            runConfig();
            Console.WriteLine("TimeSlow Mod Has Loaded");
            Events.TimeOfDayChanged += Events_TimeChanged;

        }

        void runConfig()
        {
            try
            {
                System.IO.StreamReader reader;
                try
                {
                    reader = System.IO.File.OpenText(Environment.ExpandEnvironmentVariables("%AppData%\\StardewValley\\Mods\\TimeSlowConfig.ini"));
                    Console.WriteLine("found INI in %appdata%");
                }
                catch
                {
                    reader = System.IO.File.OpenText("Mods\\TimeSlowConfig.ini");
                    Console.WriteLine("found INI in Stardew Valley-Mods");
                }
                string line = reader.ReadLine();
                char[] delimiterChars = { '=' };
                Console.WriteLine(line);
                string[] words = line.Split(delimiterChars);
                int.TryParse(words[1], out DayLength);
                DayLength -= 7;
                
            }
            catch
            {
                DayLength = 0;
                Console.WriteLine("WARNING:  Could not find INI, defaulting DayLength to the game's default");
            }

            
            if (FreezeTimeInstalled) 
            {
                Console.WriteLine("NOTE:  FreezeTime installed, adjusting mod behavior.");
                try
                {
                    System.IO.StreamReader reader;
                    try
                    {
                        reader = System.IO.File.OpenText(Environment.ExpandEnvironmentVariables("%AppData%\\StardewValley\\Mods\\FreezeInsideConfig.ini"));
                        Console.WriteLine("found FreezeInside INI in %appdata%");
                    }
                    catch
                    {
                        reader = System.IO.File.OpenText("Mods\\FreezeInsideConfig.ini");
                        Console.WriteLine("found FreezeInside INI in Stardew Valley-Mods");
                    }
                    string line = reader.ReadLine();
                    char[] delimiterChars = { '=' };
                    Console.WriteLine(line);
                    string[] words = line.Split(delimiterChars);
                    bool.TryParse(words[1], out FreezeTimeInMines);
                }
                catch
                {
                    FreezeTimeInMines = false;
                    Console.WriteLine("WARNING:  Could not find INI, defaulting FreezeTimeInMines to false");
                }
            }

            if (DayLength < 0)
            {
                DayLength = 0;
                Console.WriteLine("WARNING:  DayLength set shorter than 7 seconds.  TimeSlow can only slow time, unfortunately.");
            }


        }

        void Events_TimeChanged(Int32 time)      
        {
            if (!FreezeTimeInstalled) 
            {
                Command.CallCommand("world_freezetime 1");
                Thread.Sleep(DayLength*1000);
                Command.CallCommand("world_freezetime 0");
            }
            else
            {
                StardewValley.GameLocation location = StardewValley.Game1.currentLocation;
                if ((location != null) && (location.isOutdoors || (((location is StardewValley.Locations.MineShaft) || (location is StardewValley.Locations.FarmCave)) && FreezeTimeInMines)))
                {
                    //Console.WriteLine("FreezeInside sees that you are inside");
                    //Command.CallCommand("world_freezetime 1");
                    Command.CallCommand("world_freezetime 1");
                    //Thread.Sleep(DayLength * 1000);
                    Command.CallCommand("world_freezetime 0");
                }
            }

        }
            
            
        

    }
}
