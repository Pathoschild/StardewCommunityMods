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
            while(!Debugger.IsAttached) Thread.Sleep(1000);
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
                ResetCounter();
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
            if (key == Config.DecreaseTickLengthKey)
            {
                ClockTickLength -= Keyboard.GetState().IsKeyDown(Keys.LeftShift) ? 1000 : 100;
                NotifyPlayer($"Tick length set to {ClockTickLength / 1000f:0.###} sec.");
            }

            else if (key == Config.IncreaseTickLengthKey)
            {
                ClockTickLength += Keyboard.GetState().IsKeyDown(Keys.LeftShift) ? 1000 : 100;
                NotifyPlayer($"Tick length set to {ClockTickLength / 1000f:0.###} sec.");
            }

            else if (key == Config.FreezeTimeKey)
            {
                FreezeTime = !FreezeTime;
                NotifyPlayer(FreezeTime ? "Time is frozen." : "Time is unfrozen.");
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

        private int _clockTickInterval;

        private void ResetCounter()
        {
            _clockTickInterval = 0;
        }

        private void UpdateCounter()
        {
            if (!FreezeTime) _clockTickInterval += Game1.gameTimeInterval;
            Game1.gameTimeInterval = 0;

            if (_clockTickInterval > ClockTickLength)
            {
                Game1.gameTimeInterval = DefaultClockTickLength;
                _clockTickInterval -= ClockTickLength;
            }
        }
    }
}
