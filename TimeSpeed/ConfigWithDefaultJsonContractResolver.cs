using System.IO;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;

namespace TimeSpeed
{
    // Todo: remove once SMAPI learns how to use JsonContractResolver
    public class ConfigWithDefaultJsonContractResolver: Config
    {
        public override T UpdateConfig<T>()
        {
            var @default = JObject.FromObject(GenerateDefaultConfig<T>());
            var updated = JObject.Parse(File.ReadAllText(ConfigLocation));

            @default.Merge(updated);
            var merged = @default.ToObject<T>();
            ((ConfigWithDefaultJsonContractResolver)(object)merged).ConfigLocation = ConfigLocation;

            return merged;
        }
    }
}