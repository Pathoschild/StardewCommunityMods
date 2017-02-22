using System;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using TimeSpeed.Framework;

namespace TimeSpeed
{
    /// <summary>The entry class called by SMAPI.</summary>
    public sealed class TimeSpeed : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>Displays messages to the user.</summary>
        private readonly Notifier Notifier = new Notifier();

        /// <summary>Provides helper methods for tracking time flow.</summary>
        private readonly TimeHelper TimeHelper = new TimeHelper();

        /// <summary>The mod configuration.</summary>
        private TimeSpeedConfig Config;

        /// <summary>Whether time should be frozen everywhere.</summary>
        private bool FrozenGlobally;

        /// <summary>Whether time should be frozen at the current location.</summary>
        private bool FrozenAtLocation;

        /// <summary>Whether time should be frozen.</summary>
        private bool Frozen
        {
            get { return this.FrozenGlobally || this.FrozenAtLocation; }
            set { this.FrozenGlobally = this.FrozenAtLocation = value; }
        }

        /// <summary>Whether the flow of time should be adjusted.</summary>
        private bool AdjustTime;

        /// <summary>Backing field for <see cref="TickInterval"/>.</summary>
        private int _tickInterval;

        /// <summary>The number of seconds per 10-game-minutes to apply.</summary>
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
            // read config
            Config = helper.ReadConfig<TimeSpeedConfig>();

            // add time events
            this.TimeHelper.WhenTickProgressChanged(this.ReceiveTickProgress);
            ControlEvents.KeyPressed += this.ReceiveKeyPressed;
            LocationEvents.CurrentLocationChanged += this.ReceiveCurrentLocationChanged;
            TimeEvents.TimeOfDayChanged += this.ReceiveTimeOfDayChanged;
            TimeEvents.DayOfMonthChanged += this.ReceiveDayChanged;

            // add time freeze/unfreeze notification
            {
                bool wasPaused = false;
                GraphicsEvents.OnPreRenderHudEvent += (sender, args) =>
                {
                    wasPaused = Game1.paused;
                    if (this.Frozen) Game1.paused = true;
                };

                GraphicsEvents.OnPostRenderHudEvent += (sender, args) =>
                {
                    Game1.paused = wasPaused;
                };
            }
        }


        /*********
        ** Private methods
        *********/
        /****
        ** Event handlers
        ****/
        /// <summary>The method called when the player presses a keyboard button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ReceiveKeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (!Game1.hasLoadedGame || Game1.activeClickableMenu != null)
                return;

            var key = e.KeyPressed;

            if (key == this.Config.Keys.FreezeTime)
                this.ToogleFreeze();
            else if (key == this.Config.Keys.IncreaseTickInterval || key == this.Config.Keys.DecreaseTickInterval)
                this.ChangeTickInterval(increase: key == Config.Keys.IncreaseTickInterval);
            else if (key == this.Config.Keys.ReloadConfig)
                this.ReloadConfig();
        }

        /// <summary>The method called when the player moves to a new location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ReceiveCurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {
            this.UpdateSettingsForLocation(Game1.currentLocation);
        }

        /// <summary>The method called when the time of day changes.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ReceiveTimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            this.UpdateFreezeForTime(Game1.timeOfDay);
        }

        /// <summary>The method called when the day changes.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ReceiveDayChanged(object sender, EventArgsIntChanged e)
        {
            this.UpdateScaleForDay(Game1.currentSeason, Game1.dayOfMonth);
        }

        /// <summary>The method called when the <see cref="Framework.TimeHelper.TickProgress"/> value changes.</summary>
        /// <param name="e">The event arguments.</param>
        private void ReceiveTickProgress(TickProgressChangedEventArgs e)
        {
            if (this.Frozen)
                this.TimeHelper.TickProgress = e.TimeChanged ? 0 : e.PreviousProgress;
            else
            {
                if (!this.AdjustTime)
                    return;
                if (this.TickInterval == 0)
                    this.TickInterval = 1000;

                if (e.TimeChanged)
                    this.TimeHelper.TickProgress = this.ScaleTickProgress(this.TimeHelper.TickProgress, this.TickInterval);
                else
                    this.TimeHelper.TickProgress = e.PreviousProgress + this.ScaleTickProgress(e.NewProgress - e.PreviousProgress, this.TickInterval);
            }
        }

        /****
        ** Methods
        ****/
        /// <summary>Reload <see cref="Config"/> from the config file.</summary>
        private void ReloadConfig()
        {
            this.Config = this.Helper.ReadConfig<TimeSpeedConfig>();
            this.UpdateScaleForDay(Game1.currentSeason, Game1.dayOfMonth);
            this.UpdateSettingsForLocation(Game1.currentLocation);
            this.Notifier.ShortNotify("Time feels differently now...");
        }

        /// <summary>Increment or decrement the tick interval, taking into account the held modifier key if applicable.</summary>
        /// <param name="increase">Whether to increment the tick interval; else decrement.</param>
        private void ChangeTickInterval(bool increase)
        {
            // get offset to apply
            int change = 1000;
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.LeftControl))
                    change *= 100;
                else if (state.IsKeyDown(Keys.LeftShift))
                    change *= 10;
                else if (state.IsKeyDown(Keys.LeftAlt))
                    change /= 10;
            }

            // update tick interval
            if (!increase)
            {
                int minAllowed = Math.Min(this.TickInterval, change);
                this.TickInterval = Math.Max(minAllowed, this.TickInterval - change);
            }
            else
                this.TickInterval = this.TickInterval + change;

            // log change
            this.Notifier.QuickNotify($"10 minutes feels like {TickInterval / 1000} seconds.");
            this.Monitor.Log($"Tick length set to {TickInterval / 1000d: 0.##} seconds.", LogLevel.Info);
        }

        /// <summary>Toggle whether time is frozen.</summary>
        private void ToogleFreeze()
        {
            if (!this.Frozen)
            {
                this.FrozenGlobally = true;
                this.Notifier.QuickNotify("Hey, you stopped the time!");
                this.Monitor.Log("Time is frozen globally.", LogLevel.Info);
            }
            else
            {
                this.Frozen = false;
                this.Notifier.QuickNotify("Time feels as usual now...");
                this.Monitor.Log($"Time is temporarily unfrozen at \"{Game1.currentLocation.name}\".", LogLevel.Info);
            }
        }

        /// <summary>Update the time freeze settings for the given time of day.</summary>
        /// <param name="time">The time of day in 24-hour military format (e.g. 1600 for 8pm).</param>
        private void UpdateFreezeForTime(int time)
        {
            if (this.Config.ShouldFreeze(time))
            {
                this.FrozenGlobally = true;
                this.Notifier.ShortNotify("Time suddenly stops...");
                this.Monitor.Log($"Time automatically set to frozen at {Game1.timeOfDay}.", LogLevel.Info);
            }
        }

        /// <summary>Update the time settings for the given location.</summary>
        /// <param name="location">The game location.</param>
        private void UpdateSettingsForLocation(GameLocation location)
        {
            if (location == null)
                return;

            // update time settings
            this.FrozenAtLocation = this.FrozenGlobally || this.Config.ShouldFreeze(location);
            if (this.Config.GetTickInterval(location) != null)
                this.TickInterval = this.Config.GetTickInterval(Game1.currentLocation) ?? this.TickInterval;

            // notify player
            if (this.Config.LocationNotify)
            {
                if (this.FrozenGlobally)
                    this.Notifier.ShortNotify("Looks like time stopped everywhere...");
                else if (this.FrozenAtLocation)
                    this.Notifier.ShortNotify("It feels like time is frozen here...");
                else
                    this.Notifier.ShortNotify($"10 minutes feels more like {TickInterval / 1000} seconds here...");
            }
        }

        /// <summary>Update the time settings for the given date.</summary>
        /// <param name="season">The current season.</param>
        /// <param name="dayOfMonth">The current day of month.</param>
        private void UpdateScaleForDay(string season, int dayOfMonth)
        {
            this.AdjustTime = this.Config.ShouldScale(season, dayOfMonth);
        }

        /// <summary>Get the adjusted progress towards the next 10-game-minute tick.</summary>
        /// <param name="progress">The current progress.</param>
        /// <param name="newTickInterval">The new tick interval.</param>
        private double ScaleTickProgress(double progress, int newTickInterval)
        {
            return progress * this.TimeHelper.CurrentDefaultTickInterval / newTickInterval;
        }
    }
}
