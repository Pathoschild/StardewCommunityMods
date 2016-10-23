using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TimeSpeed.Helpers;

// ReSharper disable RedundantDefaultMemberInitializer - intentional explicit initialization

namespace TimeSpeed
{
    [PublicAPI("JSON")]
    public partial class TimeSpeedConfig : ModConfig
    {
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
        /// Example: {"Farm":null, "Outdoors": 14}
        /// </summary>
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

        [JsonProperty(PropertyName = "Keys")]
        public KeysConfig Control { get; set; } = new KeysConfig();
    }
}
