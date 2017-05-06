﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TractorMod.Framework
{
    internal class Save
    {
        /*********
        ** Accessors
        *********/
        public string FarmerName { get; set; } = "";
        public ulong SaveSeed { get; set; }
        public List<Vector2> TractorHouse = new List<Vector2>();


        /*********
        ** Public methods
        *********/
        public Save()
        {
            SaveSeed = ulong.MaxValue;
        }

        public Save(string nameInput, ulong input)
        {
            SaveSeed = input;
            FarmerName = nameInput;
        }

        public IEnumerable<Vector2> GetGarages()
        {
            foreach (Vector2 position in this.TractorHouse)
                yield return position;
        }

        public Save AddGarage(int inputX, int inputY)
        {
            foreach (Vector2 tile in TractorHouse)
            {
                if (tile.X == inputX && tile.Y == inputY)
                    return this;
            }
            this.TractorHouse.Add(new Vector2(inputX, inputY));
            return this;
        }
    }
}
