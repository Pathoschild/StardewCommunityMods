using StardewValley;

namespace TimeSpeed.Framework
{
    internal class Notifier
    {
        public void QuickNotify(string message)
        {
            Game1.hudMessages.Add(new HUDMessage(message, 2) { timeLeft = 1000 });
        }

        public void ShortNotify(string message)
        {
            Game1.hudMessages.Add(new HUDMessage(message, 2) { timeLeft = 2000 });
        }
    }
}
