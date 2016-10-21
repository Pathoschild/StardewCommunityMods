using StardewValley;
using StardewValley.Locations;

namespace TimeSpeed.Extensions
{
    public static class LocationExtensions
    {
        public static bool IsMine(this GameLocation location)
        {
            return (location is MineShaft);
        }
    }
}
