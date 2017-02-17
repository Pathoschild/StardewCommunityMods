using System;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;

namespace TimeSpeed.Helpers
{
    [PublicAPI("Helper")]
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

        public event EventHandler Reloaded;

        public void Reload()
        {
            JsonConvert.PopulateObject(File.ReadAllText(ConfigLocation), this,
                new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace });
            Reloaded?.Invoke(this, EventArgs.Empty);
        }

        public override T GenerateDefaultConfig<T>()
        {
            return this as T;
        }
    }
}