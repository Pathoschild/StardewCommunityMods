using JetBrains.Annotations;
using StardewValley;

namespace TimeSpeed.Services
{
    [PublicAPI]
    public class Notifier
    {
        protected readonly string Name;

        public Notifier(string name)
        {
            Name = name;
        }

        private string FormatMessage(string message) => $"{Name}: {message}";

        public void QuickNotify(string message)
        {
            Game1.hudMessages.Add(new HUDMessage(FormatMessage(message), 2) { timeLeft = 1000 });
        }
    }
}
