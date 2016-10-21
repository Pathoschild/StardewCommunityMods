using System;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StardewModdingAPI;
using TimeSpeed.Components;

namespace TimeSpeed
{
    [PublicAPI("Mod")]
    public class TimeSpeedConfig : ModConfig, ITimeFreezerConfig, ITimeScalerConfig
    {
        public override T GenerateDefaultConfig<T>()
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
            ReloadConfigKey = Keys.B;

            return this as T;
        }

        public int? FreezeTimeAt { get; set; }

        public bool FreezeTimeIndoors { get; set; }

        public bool FreezeTimeInMines { get; set; }

        public bool FreezeTimeOutdoors { get; set; }

        private int _outdoorTickLength;

        public int OutdoorTickLength
        {
            get { return _outdoorTickLength; }
            set
            {
                _outdoorTickLength = Math.Max(100, value);
                Log.Info($"Outdoor tick length set to {_outdoorTickLength / 1000f:0.###} sec.");
            }
        }

        private int _indoorTickLength;

        public int IndoorTickLength
        {
            get { return _indoorTickLength; }
            set
            {
                _indoorTickLength = Math.Max(100, value);
                Log.Info($"Indoor tick length set to {_indoorTickLength / 1000f:0.###} sec.");
            }
        }

        private int _mineTickLength;

        public int MineTickLength
        {
            get { return _mineTickLength; }
            set
            {
                _mineTickLength = Math.Max(100, value);
                Log.Info($"Mine tick length set to {_indoorTickLength / 1000f:0.###} sec.");
            }
        }

        public bool ChangeTimeSpeedOnFestivalDays { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Keys FreezeTimeKey { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Keys IncreaseTickLengthKey { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Keys DecreaseTickLengthKey { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Keys ReloadConfigKey { get; set; }
    }
}
