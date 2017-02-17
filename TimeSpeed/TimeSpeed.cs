using System;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using TimeSpeed.Framework;

namespace TimeSpeed
{
    public sealed class TimeSpeed : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>Displays messages to the user.</summary>
        private readonly Notifier Notifier = new Notifier();

        private readonly TimeHelper _time = new TimeHelper();

        private TimeSpeedConfig Config;

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


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<TimeSpeedConfig>();

            ControlEvents.KeyPressed += (sender, args) =>
            {
                if (!Game1.hasLoadedGame) return;
                if (Game1.activeClickableMenu != null) return;

                var key = args.KeyPressed;

                if (key == Config.Keys.FreezeTime)
                    ToogleFreezeByKey();

                if (key == Config.Keys.IncreaseTickInterval || key == Config.Keys.DecreaseTickInterval)
                    ChangeTickInterval(increase: key == Config.Keys.IncreaseTickInterval);

                if (key == Config.Keys.ReloadConfig)
                    ReloadConfig();
            };

            _time.WhenTickProgressChanged(HandleTickProgress);

            TimeEvents.TimeOfDayChanged += (sender, args) => UpdateFreezeForTime();
            TimeEvents.DayOfMonthChanged += (sender, args) => UpdateScaleForDay();
            LocationEvents.CurrentLocationChanged += (sender, args) => UpdateSettingsForLocation();

            AddFreezeTimeBoxNotification();
        }


        /*********
        ** Protected methods
        *********/
        private void ReloadConfig()
        {
            this.UpdateScaleForDay();
            this.UpdateSettingsForLocation();
            this.Config = this.Helper.ReadConfig<TimeSpeedConfig>();
            this.Notifier.ShortNotify("Time feels differently now...");
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

            this.Notifier.QuickNotify($"10 minutes feels like {TickInterval / 1000} seconds.");
            this.Monitor.Log($"Tick length set to {TickInterval / 1000d: 0.##} seconds.", LogLevel.Info);
        }

        private void ToogleFreezeByKey()
        {
            if (!Frozen)
            {
                _frozenGlobal = true;
                this.Notifier.QuickNotify("Hey, you stopped the time!");
                this.Monitor.Log("Time is frozen globally.", LogLevel.Info);
            }
            else
            {
                Frozen = false;
                this.Notifier.QuickNotify("Time feels as usual now...");
                this.Monitor.Log($"Time is temporarily unfrozen at \"{Game1.currentLocation.name}\".", LogLevel.Info);
            }
        }

        private void UpdateFreezeForTime()
        {
            if (Config.ShouldFreeze(Game1.timeOfDay))
            {
                _frozenGlobal = true;
                this.Notifier.ShortNotify("Time suddenly stops...");
                this.Monitor.Log($"Time automatically set to frozen at {Game1.timeOfDay}.", LogLevel.Info);
            }
        }

        private void UpdateSettingsForLocation()
        {
            if (Game1.currentLocation == null) return;

            _frozenAtLocation = _frozenGlobal || Config.ShouldFreeze(Game1.currentLocation);
            if (Config.GetTickInterval(Game1.currentLocation) != null)
                TickInterval = Config.GetTickInterval(Game1.currentLocation) ?? TickInterval;


            if (Config.LocationNotify)
            {
                if (_frozenGlobal)
                    this.Notifier.ShortNotify("Looks like time stopped everywhere...");
                else if (_frozenAtLocation)
                    this.Notifier.ShortNotify("It feels like time is frozen here...");
                else
                    this.Notifier.ShortNotify($"10 minutes feels more like {TickInterval / 1000} seconds here...");
            }
        }

        private void UpdateScaleForDay()
        {
            _scale = this.Config.ShouldScale(Game1.currentSeason, Game1.dayOfMonth);
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
