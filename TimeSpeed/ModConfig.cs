using System.IO;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;

namespace TimeSpeed
{
    public class ModConfig: Config
    {
        public override T UpdateConfig<T>()
        {
            var @default = JObject.FromObject(GenerateDefaultConfig<T>());
            var updated = JObject.Parse(File.ReadAllText(ConfigLocation));

            @default.Merge(updated);
            var merged = @default.ToObject<T>();
            ((ModConfig)(object)merged).ConfigLocation = ConfigLocation;

            return merged;
        }
    }
}