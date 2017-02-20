using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;

/*
TODO:
    Add seeds to seed shop inventory.  See SeedShop.cs line 155, shopStock().
*/

namespace AllCropsAllSeasons
{
    public class AllCropsAllSeasons : Mod
    {

        public Dictionary<Microsoft.Xna.Framework.Vector2, StardewValley.TerrainFeatures.HoeDirt> savedhoedirts = new Dictionary<Microsoft.Xna.Framework.Vector2, StardewValley.TerrainFeatures.HoeDirt>();
        public Dictionary<Microsoft.Xna.Framework.Vector2, StardewValley.Crop> savedcrops = new Dictionary<Microsoft.Xna.Framework.Vector2, StardewValley.Crop>();

        public override void Entry(IModHelper helper)
        {
            Console.WriteLine("All Crops All Seasons Mod Has Loaded");
            LocationEvents.CurrentLocationChanged += Events_LocationChanged;
            TimeEvents.DayOfMonthChanged += Events_DayChanged;

        }


        void Events_LocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {
            StardewValley.GameLocation location;
            //Console.WriteLine("firing event location changed");
            if (e.PriorLocation != null)
            {
                location = e.PriorLocation;
            }
            else
            {
                location = StardewValley.Game1.getLocationFromName("FarmHouse");
            }
            //Console.WriteLine(location);
            if ((StardewValley.Game1.dayOfMonth != null) && (StardewValley.Game1.currentSeason != null) && (StardewValley.Game1.currentSeason != null) && (StardewValley.Game1.dayOfMonth != null))
            {
                //Console.WriteLine("not all null");

                if (((StardewValley.Game1.dayOfMonth == 28) && (StardewValley.Game1.currentSeason.Equals("fall"))) || ((StardewValley.Game1.currentSeason.Equals("winter")) && !(StardewValley.Game1.dayOfMonth == 28)))
                {
                    //Console.WriteLine("winter is coming");

                    if (location.name == "Farm")
                    //if player has come from farm into farmhouse, we're assuming they're going to bed, bad assumption, i know
                    {
                        //Console.WriteLine("firing saveCurrentCrops");
                        saveCurrentCrops();
                    }

                }

            }


        }

        void Events_DayChanged(object sender, EventArgsIntChanged e)
        {
            //day is changed, safe again, now we go back to reset the crops properly

            if ((StardewValley.Game1.currentSeason != null) && (StardewValley.Game1.currentSeason.Equals("winter")))
            {
                //Console.WriteLine("firing restoreCurrentCrops");
                restoreCurrentCrops();
            }

        }

        //TODO:  Save crops to file for saved game because they'll be deleted upon save!  Aaaaa!

        void saveCurrentCrops()
        {

            StardewValley.GameLocation farm = StardewValley.Game1.getLocationFromName("Farm");
            StardewValley.SerializableDictionary<Microsoft.Xna.Framework.Vector2, StardewValley.TerrainFeatures.TerrainFeature> terrainFeaturelist = farm.terrainFeatures;
            savedhoedirts.Clear();
            savedcrops.Clear();
            //now get a list of HoeDirts with crops on them and save them in savedhoedirts
            foreach (KeyValuePair<Microsoft.Xna.Framework.Vector2, StardewValley.TerrainFeatures.TerrainFeature> entry in terrainFeaturelist)
            {
                //Console.WriteLine("made it to foreach");


                StardewValley.TerrainFeatures.TerrainFeature value = entry.Value;
                Microsoft.Xna.Framework.Vector2 key = entry.Key;
                if (value is StardewValley.TerrainFeatures.HoeDirt)
                {
                    StardewValley.TerrainFeatures.HoeDirt hdvalue = (StardewValley.TerrainFeatures.HoeDirt)value;
                    //Console.WriteLine("adding entry to saveCurrentCrops");
                    //var hdvalue = new KeyValuePair<Microsoft.Xna.Framework.Vector2, StardewValley.TerrainFeatures.HoeDirt>(entry.Key, (StardewValley.TerrainFeatures.HoeDirt)value);
                    savedhoedirts.Add(key, hdvalue);
                    //savedcropkeys.Add(key);
                    //Console.WriteLine(key.X.ToString("G"));
                    //Console.WriteLine(key.Y.ToString("G"));
                    if (hdvalue.crop != null)
                    {
                        //Console.WriteLine("Found a CROP!");
                        savedcrops.Add(key, hdvalue.crop);
                    }


                }



            }


        }

        void restoreCurrentCrops()
        {
            //see GameLocation.cs line 1038, moveObject for an example of how I'm trying to manipulate the list
            StardewValley.GameLocation farm = StardewValley.Game1.getLocationFromName("Farm");
            //StardewValley.GameLocation greenhouse = StardewValley.Game1.getLocationFromName("Greenhouse");
            StardewValley.SerializableDictionary<Microsoft.Xna.Framework.Vector2, StardewValley.TerrainFeatures.TerrainFeature> terrainFeaturelist = farm.terrainFeatures;
            foreach (var entry in savedhoedirts)
            {
                if (entry.Value.crop != null)
                {
                    //Console.WriteLine("Found a CROP!");
                }

                //now update saved crops as if they were in a greenhouse
                //Console.WriteLine("running dayUpdate on saveCurrentCrops");
                //entry.Value.dayUpdate(greenhouse, entry.Key);
                //now restore crops to proper location
                //if for whatever reason something is in the way of what we're restoring, delete it
                if (terrainFeaturelist.ContainsKey(entry.Key))
                {
                    terrainFeaturelist.Remove(entry.Key);
                }
                //Console.WriteLine("adding back saveCurrentCrops");
                terrainFeaturelist.Add(entry.Key, entry.Value);
                if (savedcrops.ContainsKey(entry.Key))
                {
                    StardewValley.TerrainFeatures.HoeDirt croptile = (StardewValley.TerrainFeatures.HoeDirt)terrainFeaturelist[entry.Key];
                    croptile.crop = savedcrops[entry.Key];
                }
            }
        }




    }
}
