using System;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using TimeSpeed.Helpers;

namespace TimeSpeed
{
    [PublicAPI("Mod")]
    public sealed class TimeSpeed : Mod
    {
        private Notifier _immersiveNotifier = new Notifier("");
        private Notifier _notifier = new Notifier(nameof(TimeSpeed));
        private Logger _logger = new Logger(nameof(TimeSpeed));
        private TimeHelper _time = new TimeHelper();

        private TimeSpeedConfig _config;

        private bool _frozenGlobal;
        private bool _frozenAtLocation;

        private bool Frozen
        {
            get { return _frozenGlobal || _frozenAtLocation; }
            set { _frozenGlobal = _frozenAtLocation = value; }
        }

        private bool _scale;
        private int _tickInterval;

        private int TickInterval
        {
            get { return _tickInterval; }
            set { _tickInterval = Math.Max(value, 0); }
        }

        public override void Entry(params object[] objects)
        {
            _config = new TimeSpeedConfig().InitializeConfig(BaseConfigPath);

            ControlEvents.KeyPressed += (sender, args) =>
            {
                if (!Game1.hasLoadedGame) return;
                if (Game1.activeClickableMenu != null) return;

                var key = args.KeyPressed;

                if (key == _config.Control.FreezeTime)
                    ToogleFreezeByKey();

                if (key == _config.Control.IncreaseTickInterval || key == _config.Control.DecreaseTickInterval)
                    ChangeTickInterval(increase: key == _config.Control.IncreaseTickInterval);

                if (key == _config.Control.ReloadConfig)
                    ReloadConfig();
            };

            _time.WhenTickProgressChanged(HandleTickProgress);

            TimeEvents.TimeOfDayChanged += (sender, args) => UpdateFreezeForTime();

            _config.Reloaded += (sender, args) => UpdateScaleForDay();
            TimeEvents.DayOfMonthChanged += (sender, args) => UpdateScaleForDay();

            _config.Reloaded += (sender, args) => UpdateSettingsForLocation();
            LocationEvents.CurrentLocationChanged += (sender, args) => UpdateSettingsForLocation();
            
            AddFreezeTimeBoxNotification();
        }

        private void ReloadConfig()
        {
            _config.Reload();
            _immersiveNotifier.ShortNotify("Time feels differently now...");
        }

        private void ChangeTickInterval(bool increase)
        {
            var modifier = 1000;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.LeftControl)) modifier *= 100;
            else if (state.IsKeyDown(Keys.LeftShift)) modifier *= 10;
            else if (state.IsKeyDown(Keys.LeftAlt)) modifier /= 10;

            if (!increase)
            {
                var minAllowed = Math.Min(TickInterval, modifier);
                TickInterval = Math.Max(minAllowed, TickInterval - modifier);
            }
            else
            {
                TickInterval = TickInterval + modifier;
            }

            _immersiveNotifier.QuickNotify($"10 minutes feels like {TickInterval/1000} seconds.");
            _logger.Info($"Tick length set to {TickInterval / 1000d: 0.##} seconds.");
        }

        private void ToogleFreezeByKey()
        {
            if (!Frozen)
            {
                _frozenGlobal = true;
                _immersiveNotifier.QuickNotify("Hey, you stopped the time!");
                _logger.Info("Time is frozen globally.");
            }
            else
            {
                Frozen = false;
                _immersiveNotifier.QuickNotify("Time feels as usual now...");
                _logger.Info($"Time is temporarily unfrozen at \"{Game1.currentLocation.name}\".");
            }
        }

        private void UpdateFreezeForTime()
        {
            if (_config.ShouldFreeze(Game1.timeOfDay))
            {
                _frozenGlobal = true;
                _immersiveNotifier.ShortNotify("Time suddenly stops...");
                _logger.Info($"Time automatically set to frozen at {Game1.timeOfDay}.");
            }
        }

        private void UpdateSettingsForLocation()
        {
            if (Game1.currentLocation == null) return;

            _frozenAtLocation = _frozenGlobal || _config.ShouldFreeze(Game1.currentLocation);
            if (_config.GetTickInterval(Game1.currentLocation) != null)
                TickInterval = _config.GetTickInterval(Game1.currentLocation) ?? TickInterval;


            if (_config.LocationNotify)
            {
                if (_frozenGlobal)
                    _immersiveNotifier.ShortNotify("Looks like time stopped everywhere...");
                else if (_frozenAtLocation)
                    _immersiveNotifier.ShortNotify("It feels like time is frozen here...");
                else
                    _immersiveNotifier.ShortNotify($"10 minutes feels more like {TickInterval / 1000} seconds here...");
            }
        }
        
        private void UpdateScaleForDay()
        {
            _scale = _config.ShouldScale(Game1.currentSeason, Game1.dayOfMonth);
        }

        private void AddFreezeTimeBoxNotification()
        {
            var wasPaused = false;

            GraphicsEvents.OnPreRenderHudEvent += (sender, args) =>
            {
                wasPaused = Game1.paused;
                if (Frozen) Game1.paused = true;
            };

            GraphicsEvents.OnPostRenderHudEvent += (sender, args) =>
            {
                Game1.paused = wasPaused;
            };
        }

        private double ScaleTickProgress(double progress, int newTickInterval)
            => progress * _time.CurrentDefaultTickInterval / newTickInterval;

        private void HandleTickProgress(TimeHelper.TickProgressChangedEventArgs args)
        {
            if (Frozen)
                _time.TickProgress = args.TimeChanged ? 0 : args.PreviousProgress;
            else
            {
                if (!_scale) return;
                if (TickInterval == 0) TickInterval = 1000;

                if (args.TimeChanged)
                    _time.TickProgress = ScaleTickProgress(_time.TickProgress, TickInterval);
                else
                    _time.TickProgress = args.PreviousProgress +
                                         ScaleTickProgress(args.NewProgress - args.PreviousProgress, TickInterval);
            }
        }
    }
}
