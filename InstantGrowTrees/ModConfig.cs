using StardewModdingAPI;

namespace InstantGrowTrees
{
    public class ModConfig : Config
    {
        public bool FruitTreesInstantGrow { get; set; }
        public bool RegularTreesInstantGrow { get; set; }
        public bool RegularTreesGrowInWinter { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            this.FruitTreesInstantGrow = false;
            this.RegularTreesInstantGrow = true;
            this.RegularTreesGrowInWinter = false;

            return this as T;
        }
    }
}