using System;
using System.Linq;
using System.Runtime.Serialization;
using StardewValley;

namespace TimeSpeed
{
    public partial class TimeSpeedConfig
    {
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
