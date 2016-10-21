using System;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI.Events;
using StardewValley;
using TimeSpeed.Extensions;
using TimeSpeed.Services;

namespace TimeSpeed.Components
{
    public sealed class TimeScaler
    {
        private ITimeScalerConfig Config { get; }

        private Notifier Notifier { get; }

        public TimeScaler(ITimeScalerConfig config, Notifier notifier)
        {
            Config = config;
            Notifier = notifier;
        }

        public void Enable()
        {
            EnableTimeScaling();
            EnableScaleControlHotkeys();
        }

        private int DefaultClockTickLength => 7000 + (Game1.currentLocation?.getExtraMillisecondsPerInGameMinuteForThisLocation() ?? 0);

        private int LocationTickLength
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

        private double ScaleTickProgress(double progress) => progress * DefaultClockTickLength / LocationTickLength;

        private double _previousTickProgress;

        private void EnableTimeScaling()
        {
            var scaleOnThisDay = true;

            TimeEvents.DayOfMonthChanged += (sender, changed) =>
            {
                if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason))
                {
                    scaleOnThisDay = Config.ChangeTimeSpeedOnFestivalDays;
                }
            };

            GameEvents.UpdateTick += (sender, args) =>
            {
                if (!scaleOnThisDay) return;

                var progressDiff = TickProgress - _previousTickProgress;

                if (progressDiff < 0) TickProgress = ScaleTickProgress(TickProgress);
                else TickProgress = _previousTickProgress + ScaleTickProgress(progressDiff);

                _previousTickProgress = TickProgress;
            };
        }

        private void EnableScaleControlHotkeys()
        {
            ControlEvents.KeyPressed += (sender, pressed) =>
            {
                var key = pressed.KeyPressed;

                if (key == Config.IncreaseTickLengthKey || key == Config.DecreaseTickLengthKey)
                {
                    int modifier = 1000;
                    if (key == Config.DecreaseTickLengthKey) modifier *= -1;
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt)) modifier /= 10;
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) modifier *= 10;

                    var minAllowed = Math.Min(LocationTickLength, Math.Abs(modifier));
                    LocationTickLength = Math.Max(minAllowed, LocationTickLength + modifier);
                    Notifier.QuickNotify($"Tick length set to {LocationTickLength / 1000f:0.###} sec.");
                }
            };
        }
    }
}
