/*
    Copyright 2016 cantorsdust

    AllProfessions is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    AllProfessions is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with AllProfessions.  If not, see <http://www.gnu.org/licenses/>.
 */

using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllProfessions
{
    public class AllProfessions : Mod
    {


        public override void Entry(params object[] objects)
        {
            runConfig();
            Console.WriteLine("AllProfessions Has Loaded");
            LocationEvents.CurrentLocationChanged += Events_LocationChanged;


        }

        public List<int> FarmerLvlFive = new List<int> { 0, 1 };
        public List<int> FarmerLvlTen = new List<int> { 2, 3, 4, 5 };

        public List<int> FishingLvlFive = new List<int> { 6, 7 };
        public List<int> FishingLvlTen = new List<int> { 8, 9, 10, 11 };

        public List<int> ForagingLvlFive = new List<int> { 12, 13 };
        public List<int> ForagingLvlTen = new List<int> { 14, 15, 16, 17 };

        public List<int> MiningLvlFive = new List<int> { 18, 19 };
        public List<int> MiningLvlTen = new List<int> { 20, 21, 22, 23 };

        public List<int> CombatLvlFive = new List<int> { 24, 25 };
        public List<int> CombatLvlTen = new List<int> { 26, 27, 28, 29 };



        void runConfig()
        {
        }


        public void Events_LocationChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("Leveled up!");
            AddMissingProfessions();
        }

        public void AddMissingProfessions()
        {
            //Console.WriteLine("Adding new professions");
            var professions = Game1.player.professions;
            List<List<int>> ProfessionsList = new List<List<int>> { FarmerLvlFive, FarmerLvlTen, FishingLvlFive, FishingLvlTen,
                ForagingLvlFive, ForagingLvlTen, MiningLvlFive, MiningLvlTen, CombatLvlFive, CombatLvlTen };

            foreach (List<int> list in ProfessionsList)
            {
                //Console.WriteLine("iterating over lists");
                if (professions.Intersect(list).Any())
                {
                    //Console.WriteLine("profession intersection found" + list.ToString());
                    foreach (int element in list)
                    {
                        //Console.WriteLine("checking element: " + element.ToString("g"));
                        if (!professions.Contains(element))
                        {
                            professions.Add(element);
                            //Console.WriteLine("Adding profession number: " + element.ToString("g"));

                            //adding in health bonuses that are a special case of LevelUpMenu.getImmediateProfessionPerk
                            if (element == 24)
                            {
                                Game1.player.maxHealth += 15;
                            }
                            if (element == 27)
                            {
                                Game1.player.maxHealth += 25;
                            }
                        }
                    }
                }
            }            
        }
    }
}
