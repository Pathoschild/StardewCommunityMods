using StardewModdingAPI.Events;
using StardewValley;
using TimeSpeed.Extensions;
using TimeSpeed.Services;

namespace TimeSpeed.Components
{
    public sealed class TimeFreezer
    {
        private readonly Notifier _notifier;

        private readonly ITimeFreezerConfig _config;

        private readonly Logger _logger;

        public TimeFreezer(ITimeFreezerConfig config, Notifier notifier, Logger logger)
        {
            _logger = logger;
            _notifier = notifier;
            _config = config;
        }

        private bool FreezeTimeAtCurrentLocation
        {
            get
            {
                var location = Game1.currentLocation;
                if (location == null) return false;

                if (location.isOutdoors) return _config.FreezeTimeOutdoors;
                if (location.IsMine()) return _config.FreezeTimeInMines;
                return _config.FreezeTimeIndoors;
            }
        }

        public void Enable()
        {
            StartTimeFreezeLoop();
            EnableTimeFreezingAtSpecificTime();
            EnableFreezeTimeHotkey();
            EnableHUDFreezeNotification();
        }

        private bool? _forceTimeIsFrozen;

        private bool FreezeTime
        {
            get
            {
                return _forceTimeIsFrozen ?? FreezeTimeAtCurrentLocation;
            }
            set
            {
                _forceTimeIsFrozen = value;
                _logger.Info(FreezeTime ? "Time is frozen." : "Time is unfrozen.");
            }
        }

        private int _previousInterval;

        private void StartTimeFreezeLoop()
        {
            GameEvents.UpdateTick += (sender, args) =>
            {
                if (FreezeTime)
                {
                    if (Game1.gameTimeInterval - _previousInterval < 0) Game1.gameTimeInterval = 0;
                    else Game1.gameTimeInterval = _previousInterval;

                    _previousInterval = Game1.gameTimeInterval;
                }
            };
        }

        private void EnableFreezeTimeHotkey()
        {
            ControlEvents.KeyPressed += (sender, pressed) =>
            {
                if (pressed.KeyPressed == _config.FreezeTimeKey)
                {
                    FreezeTime = !FreezeTime;
                    _notifier.QuickNotify(FreezeTime ? "Time is frozen." : "Time is unfrozen.");
                }
            };
        }

        private void EnableTimeFreezingAtSpecificTime()
        {
            TimeEvents.TimeOfDayChanged += (sender, dayTime) =>
            {
                if (_config.FreezeTimeAt == dayTime.NewInt)
                    FreezeTime = true;
            };
        }

        private void EnableHUDFreezeNotification()
        {
            var wasPaused = false;

            GraphicsEvents.OnPreRenderHudEvent += (sender, args) =>
            {
                wasPaused = Game1.paused;
                if (FreezeTime) Game1.paused = true;
            };

            GraphicsEvents.OnPostRenderHudEvent += (sender, args) =>
            {
                Game1.paused = wasPaused;
            };
        }
    }
}
