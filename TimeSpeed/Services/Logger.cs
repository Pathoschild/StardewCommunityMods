using System.Diagnostics;
using StardewModdingAPI;

namespace TimeSpeed.Services
{
    public class Logger
    {
        private readonly string _name;

        public Logger(string name)
        {
            _name = name;
        }

        private string FormatMessage(string message) => $"[{_name}] {message}";

        public void Info(string message)
        {
            Log.Info(FormatMessage(message));
        }

        public void Success(string message)
        {
            Log.Success(FormatMessage(message));
        }

        public void Error(string message)
        {
            Log.Error(FormatMessage(message));
        }

        [Conditional("DEBUG")]
        public void DebugInfo(string message)
        {
            Log.Info(FormatMessage(message));
        }

        [Conditional("DEBUG")]
        public void DebugSuccess(string message)
        {
            Log.Info(FormatMessage(message));
        }

        [Conditional("DEBUG")]
        public void DebugError(string message)
        {
            Log.Info(FormatMessage(message));
        }
    }
}
