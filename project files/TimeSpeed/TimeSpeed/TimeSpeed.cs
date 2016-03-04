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
            get { return "1.2"; }
        }

        public override string Description
        {
            get { return "Allows for a configurable day length."; }
        }

        public int DayLength;

        public override void Entry()
        {
            runConfig();
            Console.WriteLine("TimeSpeed Mod Has Loaded");
            Events.TimeOfDayChanged += Events_TimeChanged;

        }

        void runConfig()
        {
            string FilePathAppData = Environment.ExpandEnvironmentVariables("%AppData%\\StardewValley\\Mods\\TimeSpeedConfig.ini");
            string FilePathSVMods = "Mods\\TimeSpeedConfig.ini";
            try
            {
                System.IO.StreamReader reader;
                try
                {
                    reader = System.IO.File.OpenText(FilePathAppData);
                    Console.WriteLine("found INI in %appdata%");
                }
                catch
                {
                    reader = System.IO.File.OpenText(FilePathSVMods);
                    Console.WriteLine("found INI in Stardew Valley-Mods");
                }
                string line = reader.ReadLine();
                char[] delimiterChars = { '=' };
                Console.WriteLine(line);
                string[] words = line.Split(delimiterChars);
                int.TryParse(words[1], out DayLength);
                
            }
            catch
            {
                DayLength = 7;
                Console.WriteLine("WARNING:  Could not find INI, defaulting TenMinuteTickLength to 7.  Writing new INI in %appdata%\\StardewValley\\Mods");
                System.IO.File.AppendAllLines(FilePathAppData, new[] { "TenMinuteTickLength=7" });
            }

            

            if (DayLength < 0)
            {
                DayLength = 7;
                Console.WriteLine("WARNING:  DayLength set shorter than 0 seconds.  TimeSpeed cannot travel back in time, unfortunately, defaulting TenMinuteTickLength to 7");
            }


        }

        void Events_TimeChanged(Int32 time)      
        {
            StardewValley.Game1.gameTimeInterval = (7 - DayLength) * 1000;
        }
            
            
        

    }
}
