using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StardewValley;

// ReSharper disable RedundantDefaultMemberInitializer - intentional explicit initialization

namespace TimeSpeed
{
    [PublicAPI("JSON")]
    public class TimeSpeedConfig
    {
        /*********
        ** Accessors
        *********/
        /// <summary>
        /// Default length of in-game 10 minutes tick in seconds.
        /// Game uses 7 seconds by default.
        /// Specifying zero or negative values will cause undefined behavior :)
        /// Set to null to freeze time globally.
        /// </summary>
        public double? DefaultTickLength { get; set; } = 14.0;

        public enum LocationTypes
        {
            Indoors,
            Outdoors
        }

        /// <summary>
        /// Time speed for each location or location type. This will override <see cref="DefaultTickLength"/>.
        /// Specifying zero or negative values will cause undefined behavior.
        /// Set to null to freeze time for location or location type.
        /// </summary>
        /// <remarks>
        /// Supported location types are specified in <see cref="LocationTypes"/>.
        /// Most location names can be found at "\Stardew Valley\Content\Maps" directory. They usually match file name without xnb extension.
        /// Or you can use "CurrentLocation" mod by AlphaOmegasis: http://www.nexusmods.com/stardewvalley/mods/638
        /// </remarks>
        /// <example>
        /// This will set time in Mines and Skull Cavern to 28 seconds per 10 in-game minutes, freeze time indoors and use <see cref="DefaultTickLength"/> for outdoors.
        /// <code>
        /// "TickLengthByLocation": {
        ///     "UndergroundMine": 28,
        ///     "Indoors": null
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// This will freeze time on you Farm and set it to 14 seconds per 10 in-game minutes elsewhere.
        /// <code>
        /// "TickLengthByLocation": {
        ///     "Indoors": 14,
        ///     "Outdoors": 14,
        ///     "Farm":null
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// This will freeze time in Saloon. In all other location it will act as specified by <see cref="DefaultTickLength"/>.
        /// <code>
        /// "TickLengthByLocation": {
        ///     "Saloon": null
        /// }
        /// </code>
        /// </example>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, double?> TickLengthByLocation { get; set; } = new Dictionary<string, double?>
        {
            { LocationTypes.Indoors.ToString(), 14.0 },
            { LocationTypes.Outdoors.ToString(), 14.0 },
        };

        /// <summary>
        /// Enable tick length override on festival days.
        /// </summary>
        public bool EnableOnFestivalDays { get; set; } = false;

        /// <summary>
        /// If not null time will be freezed everywhere at specified time.
        /// Format: 1230 for 12:30PM; 800 for 8:00AM
        /// </summary>
        public int? FreezeTimeAt { get; set; } = null;

        /// <summary>
        /// If true mod will notify about time settings at current location.
        /// </summary>
        public bool LocationNotify { get; set; } = false;

        /// <summary>
        /// Allowed keys can be found here:
        /// https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx
        /// Set key to "None" or 0 to disable it.
        /// </summary>
        [PublicAPI("Config")]
        public class KeysConfig
        {
            /// <summary>
            /// If time is not frozen this key will freeze it everywhere.
            /// If time is frozen this key will unfreeze it untill location changed - then other settings will take precedence.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public Keys FreezeTime { get; set; } = Keys.N;

            /// <summary>
            /// Increases current tick length by 1 second.
            /// If pressed with Control - 100, Shift - 10, Alt - 0.1 seconds.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public Keys IncreaseTickInterval { get; set; } = Keys.OemPeriod;

            /// <summary>
            /// Decreases current tick length by 1 second.
            /// If pressed with Control - 100, Shift - 10, Alt - 0.1 seconds.
            /// For safety reasons tick length won't became less than difference.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public Keys DecreaseTickInterval { get; set; } = Keys.OemComma;

            /// <summary>
            /// Reloads all values from config and applies them immediately.
            /// Time will stay frozen if it was frozen via hotkey.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public Keys ReloadConfig { get; set; } = Keys.B;
        }

        [JsonProperty(PropertyName = "Keys", NullValueHandling = NullValueHandling.Ignore)]
        public KeysConfig Control { get; set; } = new KeysConfig();


        /*********
        ** Private methods
        *********/
        private double? GetTickLengthOrFreeze(GameLocation location)
        {
            double? locationTickLength;
            if (TickLengthByLocation.TryGetValue(location.Name, out locationTickLength) ||
                (location.IsOutdoors && TickLengthByLocation.TryGetValue(LocationTypes.Outdoors.ToString(), out locationTickLength)) ||
                (!location.IsOutdoors && TickLengthByLocation.TryGetValue(LocationTypes.Indoors.ToString(), out locationTickLength)))
            {
                return locationTickLength;
            }

            return DefaultTickLength;
        }

        [OnDeserialized]
        private void OnDeserializedMethod(StreamingContext context)
        {
            TickLengthByLocation = TickLengthByLocation.ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.OrdinalIgnoreCase);
        }

        public bool ShouldFreeze(GameLocation location)
        {
            return GetTickLengthOrFreeze(location) == null;
        }

        public bool ShouldFreeze(int time)
        {
            return time == FreezeTimeAt;
        }

        public bool ShouldScale(string season, int dayOfMonth)
        {
            if (EnableOnFestivalDays) return true;
            return !Utility.isFestivalDay(dayOfMonth, season);
        }

        public int? GetTickInterval(GameLocation location)
        {
            return (int?)((GetTickLengthOrFreeze(location) ?? DefaultTickLength) * 1000);
        }
    }
}
