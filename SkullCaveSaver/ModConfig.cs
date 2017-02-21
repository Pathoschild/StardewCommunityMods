using StardewModdingAPI;

namespace SkullCaveSaver
{
    public class ModConfig : Config
    {
        public int LastMineLevel { get; set; }
        public int SaveLevelEveryXFloors { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            LastMineLevel = 0;
            SaveLevelEveryXFloors = 5;

            return this as T;
        }
    }
}
