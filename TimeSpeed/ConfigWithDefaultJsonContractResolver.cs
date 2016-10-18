using System.IO;
using System.Threading;
using Newtonsoft.Json;
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
            var updated = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigLocation));

            @default.Merge(JObject.FromObject(updated));
            var merged = @default.ToObject<T>();
            ((ConfigWithDefaultJsonContractResolver)(object)merged).ConfigLocation = ConfigLocation;

            return merged;
        }
    }
}