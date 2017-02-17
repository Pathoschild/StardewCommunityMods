using JetBrains.Annotations;
using StardewValley;

namespace TimeSpeed.Framework
{
    [PublicAPI("Helper")]
    internal class Notifier
    {
        protected readonly string Name;

        public Notifier(string name = null)
        {
            this.Name = name;
        }

        private string FormatMessage(string message) => string.IsNullOrEmpty(this.Name) ? message : $"{this.Name}: {message}";

        public void QuickNotify(string message)
        {
            Game1.hudMessages.Add(new HUDMessage(this.FormatMessage(message), 2) { timeLeft = 1000 });
        }

        public void ShortNotify(string message)
        {
            Game1.hudMessages.Add(new HUDMessage(this.FormatMessage(message), 2) { timeLeft = 2000 });
        }
    }
}
