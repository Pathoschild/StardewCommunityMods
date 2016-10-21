using StardewValley;

namespace TimeSpeed.Services
{
    public class Notifier
    {
        private readonly string _name;

        public Notifier(string name)
        {
            _name = name;
        }

        private string FormatMessage(string message) => $"{_name}: {message}";

        public void QuickNotify(string message)
        {
            Game1.hudMessages.Add(new HUDMessage(FormatMessage(message), 2) { timeLeft = 1000 });
        }
    }
}
