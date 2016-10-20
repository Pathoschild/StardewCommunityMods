using System;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace TimeSpeed
{
    public sealed class TimeSpeedMod : Mod
    {
#if DEBUG
        static TimeSpeedMod()
        {
            while (!System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(1000);
        }
#endif

        private const int DefaultClockTickLength = 7000;

        private ModConfig Config { get; set; }

        public override void Entry(params object[] objects)
        {
            Config = new ModConfig().InitializeConfig(BaseConfigPath);

            ControlEvents.KeyPressed += (sender, pressed) => HandleKey(pressed.KeyPressed);

            TimeEvents.TimeOfDayChanged += (sender, changed) => HandleFreeTimeAt();

            var isFestivalDay = false;
            GameEvents.UpdateTick += (sender, args) =>
            {
                if (!Config.ChangeTimeSpeedOnFestivalDays && isFestivalDay) return;
                UpdateCounter();
            };
            TimeEvents.DayOfMonthChanged += (sender, changed) =>
            {
                isFestivalDay = Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason);
            };

            GameEvents.FirstUpdateTick += (sender, args) => AddTimeStatusDisplayHack();

            Log.Info("TimeSpeed has loaded");
        }

        private void HandleFreeTimeAt()
        {
            if (Game1.timeOfDay == Config.FreezeTimeAt)
                FreezeTime = true;
        }

        private void AddTimeStatusDisplayHack()
        {
            bool prevPaused = false;

            var timeBox = Game1.dayTimeMoneyBox;

            timeBox.BeforeDraw(b =>
            {
                prevPaused = Game1.paused;
                if (FreezeTime) Game1.paused = true;
            });

            timeBox.AfterDraw(b => Game1.paused = prevPaused);
        }

        private void NotifyPlayer(string message)
        {
            Game1.hudMessages.Add(new HUDMessage(message, 2) { timeLeft = 1000 });
        }

        private void HandleKey(Keys key)
        {
            if (key == Config.FreezeTimeKey)
            {
                FreezeTime = !FreezeTime;
                NotifyPlayer(FreezeTime ? "Time is frozen." : "Time is unfrozen.");
            }

            else if (key == Config.IncreaseTickLengthKey || key == Config.DecreaseTickLengthKey)
            {
                int modifier = 1000;
                if (key == Config.DecreaseTickLengthKey) modifier *= -1;
                if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt)) modifier /= 10;
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) modifier *= 10;

                var minAllowed = Math.Min(ClockTickLength, Math.Abs(modifier));
                ClockTickLength = Math.Max(minAllowed, ClockTickLength + modifier);
                NotifyPlayer($"Tick length set to {ClockTickLength / 1000f:0.###} sec.");
            }
        }

        private bool? _forceTimeIsFrozen;

        private bool FreezeTime
        {
            get
            {
                var location = Game1.currentLocation;
                if (location == null) return false;

                if (_forceTimeIsFrozen.HasValue) return _forceTimeIsFrozen.Value;

                if (location.isOutdoors) return Config.FreezeTimeOutdoors;
                if (location.IsMine()) return Config.FreezeTimeInMines;
                return Config.FreezeTimeIndoors;
            }
            set
            {
                _forceTimeIsFrozen = value;
                Log.Info(FreezeTime ? "Time is frozen." : "Time is unfrozen.");
            }
        }

        private int ClockTickLength
        {
            get
            {
                var location = Game1.currentLocation;
                if (location == null) return DefaultClockTickLength;

                if (location.isOutdoors) return Config.OutdoorTickLength;
                if (location.IsMine()) return Config.MineTickLength;
                return Config.IndoorTickLength;
            }
            set
            {
                var location = Game1.currentLocation;
                if (location == null) return;

                if (location.isOutdoors) Config.OutdoorTickLength = value;
                else if (location.IsMine()) Config.MineTickLength = value;
                else Config.IndoorTickLength = value;
            }
        }

        private double TickProgress
        {
            get { return ((double)Game1.gameTimeInterval) / DefaultClockTickLength; }
            set { Game1.gameTimeInterval = (int)(value * DefaultClockTickLength); }
        }

        private double _previousTickProgress;

        private double Scale(double progress) => progress * DefaultClockTickLength / ClockTickLength;

        private void UpdateCounter()
        {
            var progressDiff = TickProgress - _previousTickProgress;

            if (FreezeTime) progressDiff = 0;

            if (progressDiff < 0) TickProgress = Scale(TickProgress);
            else TickProgress = _previousTickProgress + Scale(progressDiff);

            _previousTickProgress = TickProgress;
        }
    }
}
