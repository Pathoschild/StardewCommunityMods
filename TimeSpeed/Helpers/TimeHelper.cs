using System;
using JetBrains.Annotations;
using StardewModdingAPI.Events;
using StardewValley;

namespace TimeSpeed.Helpers
{
    [PublicAPI("Helper")]
    public class TimeHelper
    {
        public int CurrentDefaultTickInterval => 7000 + (Game1.currentLocation?.getExtraMillisecondsPerInGameMinuteForThisLocation() ?? 0);

        public double TickProgress
        {
            get { return (double)Game1.gameTimeInterval / CurrentDefaultTickInterval; }
            set { Game1.gameTimeInterval = (int)(value * CurrentDefaultTickInterval); }
        }

        [PublicAPI("Helper")]
        public class TickProgressChangedEventArgs : EventArgs
        {
            public double PreviousProgress { get; }

            public double NewProgress { get; }

            public bool TimeChanged => NewProgress < PreviousProgress;

            public TickProgressChangedEventArgs(double previousProgess, double newProgress)
            {
                PreviousProgress = previousProgess;
                NewProgress = newProgress;
            }
        }

        public Action WhenTickProgressChanged(Action<TickProgressChangedEventArgs> handler)
        {
            var previousProgress = 0d;
            EventHandler wrapper = (sender, args) =>
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator - intended
                if (previousProgress != TickProgress)
                    handler(new TickProgressChangedEventArgs(previousProgress, TickProgress));
                previousProgress = TickProgress;
            };

            GameEvents.UpdateTick += wrapper;
            return () => GameEvents.UpdateTick -= wrapper;
        }
    }
}
