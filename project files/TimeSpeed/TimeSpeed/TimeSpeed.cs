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

        public int TenMinuteTickLength = 14;
        public bool ChangeTimeSpeedOnFestivalDays = false;

        public override void Entry(params object[] objects)
        {
            runConfig();
            Console.WriteLine("TimeSpeed Mod Has Loaded");
            TimeEvents.TimeOfDayChanged += Events_TimeChanged;

        }

        void runConfig()
        {
            string FilePathAppData = Environment.ExpandEnvironmentVariables("%AppData%\\StardewValley\\Mods\\TimeSpeedConfig.ini");
            string FilePathSVMods = "Mods\\TimeSpeedConfig.ini";
            string path = "";
            char[] delimiterChars = { '=' };
            if (System.IO.File.Exists(FilePathAppData))
            {
                Console.WriteLine("found INI in %appdata%");
                path = FilePathAppData;
            }
            else if (System.IO.File.Exists(FilePathSVMods))
            {
                Console.WriteLine("found INI in Stardew Valley-Mods");
                path = FilePathSVMods;
            }
            else
            {
                //TenMinuteTickLength = 14;
                //ChangeTimeSpeedOnFestivalDays = false;
                Console.WriteLine("WARNING:  Could not find INI, defaulting TenMinuteTickLength to 14 and ChangeTimeSpeedOnFestivalDays to false.  Writing new INI in %appdata%\\StardewValley\\Mods");
                System.IO.File.AppendAllLines(FilePathAppData, new[] { "TenMinuteTickLength=14", "ChangeTimeSpeedOnFestivalDays=false" });
            }

            if (path != "")
            {
                var fileData = System.IO.File.ReadAllLines(path);
                if (fileData.Length > 1)
                {                  
                    Console.WriteLine(fileData[0]);
                    string[] words = fileData[0].Split(delimiterChars);
                    int.TryParse(words[1], out TenMinuteTickLength);

                    Console.WriteLine(fileData[1]);
                    words = fileData[1].Split(delimiterChars);
                    bool.TryParse(words[1], out ChangeTimeSpeedOnFestivalDays);
                }
                else if (fileData.Length > 0) 
                {
                    Console.WriteLine(fileData[0]);
                    string[] words = fileData[0].Split(delimiterChars);
                    int.TryParse(words[1], out TenMinuteTickLength);
                    System.IO.File.AppendAllLines(FilePathAppData, new[] { "ChangeTimeSpeedOnFestivalDays=false" });
                }
            }
            /*
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
                int.TryParse(words[1], out TenMinuteTickLength);

                string line2 = reader.ReadLine();
                Console.WriteLine(line);
                string[] words2 = line.Split(delimiterChars);
                bool.TryParse(words2[1], out ChangeTimeSpeedOnFestivalDays);
                
            }
            */


            

            if (TenMinuteTickLength <= 0)
            {
                TenMinuteTickLength = 7;
                Console.WriteLine("WARNING:  TenMinuteTickLength set shorter than 0 seconds.  TimeSpeed cannot travel back in time, unfortunately, defaulting TenMinuteTickLength to 7");
            }


        }

        void Events_TimeChanged(object sender, EventArgs e)     
        {
            if ((!StardewValley.Utility.isFestivalDay(StardewValley.Game1.dayOfMonth, StardewValley.Game1.currentSeason)) || ChangeTimeSpeedOnFestivalDays)
            {
                StardewValley.Game1.gameTimeInterval = (7 - TenMinuteTickLength) * 1000;
                Console.WriteLine("fired!");
            }
            
        }
            
            
        

    }
}
