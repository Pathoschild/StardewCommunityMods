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
        private TimeSpeedConfig _config;
        private Notifier _notifier;
        private Logger _logger;

        public override void Entry(params object[] objects)
        {
            _config = new TimeSpeedConfig().InitializeConfig(BaseConfigPath);
            _notifier = new Notifier("TimeSpeed");
            _logger = new Logger("TimeSpeed");

            new TimeFreezer(_config, _notifier, _logger).Enable();
            new TimeScaler(_config, _notifier).Enable();
        }

        private void EnableConfigurationReload()
        {
            ControlEvents.KeyPressed += (sender, pressed) =>
            {
                if (pressed.KeyPressed == _config.ReloadConfigKey)
                {
                    _config.Reload();
                    _notifier.QuickNotify("Configuration reloaded.");
                }
            };
        }
    }
}
