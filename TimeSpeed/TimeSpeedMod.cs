using JetBrains.Annotations;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using TimeSpeed.Components;
using TimeSpeed.Services;

namespace TimeSpeed
{
    [PublicAPI("Mod")]
    public sealed class TimeSpeedMod : Mod
    {
#if DEBUG
        static TimeSpeedMod()
        {
            while (!System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(1000);
        }
#endif
        public TimeSpeedConfig Config { get; set; }

        public override void Entry(params object[] objects)
        {
            Config = new TimeSpeedConfig().InitializeConfig(BaseConfigPath);
            
            var notifier = new InGameNotifier("TimeSpeed");
            var logger = new Logger("TimeSpeed");

            new TimeFreezer(Config, notifier, logger).Enable();
            new TimeScaler(Config, notifier).Enable();
        }
    }
}
