using System;
using JetBrains.Annotations;
using StardewModdingAPI.Events;
using StardewValley;

namespace TimeSpeed.Framework
{
    [PublicAPI("Helper")]
    internal class TimeHelper
    {
        public int CurrentDefaultTickInterval => 7000 + (Game1.currentLocation?.getExtraMillisecondsPerInGameMinuteForThisLocation() ?? 0);

        public double TickProgress
        {
            get { return (double)Game1.gameTimeInterval / this.CurrentDefaultTickInterval; }
            set { Game1.gameTimeInterval = (int)(value * this.CurrentDefaultTickInterval); }
        }

        [PublicAPI("Helper")]
        public class TickProgressChangedEventArgs : EventArgs
        {
            public double PreviousProgress { get; }

            public double NewProgress { get; }

            public bool TimeChanged => this.NewProgress < this.PreviousProgress;

            public TickProgressChangedEventArgs(double previousProgess, double newProgress)
            {
                this.PreviousProgress = previousProgess;
                this.NewProgress = newProgress;
            }
        }

        public Action WhenTickProgressChanged(Action<TickProgressChangedEventArgs> handler)
        {
            var previousProgress = 0d;
            EventHandler wrapper = (sender, args) =>
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator - intended
                if (previousProgress != this.TickProgress)
                    handler(new TickProgressChangedEventArgs(previousProgress, this.TickProgress));
                previousProgress = this.TickProgress;
            };

            GameEvents.UpdateTick += wrapper;
            return () => GameEvents.UpdateTick -= wrapper;
        }
    }
}
