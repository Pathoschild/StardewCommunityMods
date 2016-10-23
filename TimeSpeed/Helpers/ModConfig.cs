using System;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using StardewModdingAPI;

namespace TimeSpeed.Helpers
{
    [PublicAPI("Helper")]
    public class ModConfig: Config
    {
        //public override T UpdateConfig<T>()
        //{
        //    var obj = base.UpdateConfig<T>();
        //    var @default = JObject.FromObject(GenerateDefaultConfig<T>());
        //    var updated = JObject.Parse(File.ReadAllText(ConfigLocation));

        //    var content = JsonConvert.SerializeObject((object)this, typeof(T), Formatting.Indented, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });

        //    @default.Merge(updated);
        //    var merged = @default.ToObject<T>();
        //    ((ModConfig)(object)merged).ConfigLocation = ConfigLocation;

        //    return merged;
        //}

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