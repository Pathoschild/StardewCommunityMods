using JetBrains.Annotations;
using StardewValley;

namespace TimeSpeed.Helpers
{
    [PublicAPI("Helper")]
    public class Notifier
    {
        protected readonly string Name;

        public Notifier(string name = null)
        {
            Name = name;
        }

        private string FormatMessage(string message) => string.IsNullOrEmpty(Name) ? message : $"{Name}: {message}";

        public void QuickNotify(string message)
        {
            Game1.hudMessages.Add(new HUDMessage(FormatMessage(message), 2) { timeLeft = 1000 });
        }

        public void ShortNotify(string message)
        {
            Game1.hudMessages.Add(new HUDMessage(FormatMessage(message), 2) { timeLeft = 2000 });
        }
    }
}
