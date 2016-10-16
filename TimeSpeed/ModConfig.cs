using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StardewModdingAPI;

namespace TimeSpeed
{
    public class ModConfig : Config
    {
        public ModConfig()
        {
            _outdoorTickLength = 14000;
            _indoorTickLength = 14000;
            _mineTickLength = 14000;
            FreezeTimeOutdoors = false;
            FreezeTimeIndoors = false;
            FreezeTimeInMines = false;
            FreezeTimeAt = null;
            FreezeTimeKey = Keys.N;
            IncreaseTickLengthKey = Keys.OemPeriod;
            DecreaseTickLengthKey = Keys.OemComma;
        }

        public override T GenerateDefaultConfig<T>()
        {
            return this as T;
        }

        public int? FreezeTimeAt { get; private set; }

        public bool FreezeTimeIndoors { get; private set; }

        public bool FreezeTimeInMines { get; private set; }

        public bool FreezeTimeOutdoors { get; private set; }

        private int _outdoorTickLength;

        public int OutdoorTickLength
        {
            get { return _outdoorTickLength; }
            set
            {
                _outdoorTickLength = Math.Max(100, value);
                Log.Info($"Outdoor tick length set to {_outdoorTickLength/1000f:0.###} sec.");
            }
        }

        private int _indoorTickLength;

        public int IndoorTickLength
        {
            get { return _indoorTickLength; }
            set
            {
                _indoorTickLength = Math.Max(100, value);
                Log.Info($"Indoor tick length set to {_indoorTickLength/1000f:0.###} sec.");
            }
        }

        private int _mineTickLength;

        public int MineTickLength
        {
            get { return _mineTickLength; }
            set
            {
                _mineTickLength = Math.Max(100, value);
                Log.Info($"Mine tick length set to {_indoorTickLength/1000f:0.###} sec.");
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Keys FreezeTimeKey { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Keys IncreaseTickLengthKey { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Keys DecreaseTickLengthKey { get; private set; }
    }
}
