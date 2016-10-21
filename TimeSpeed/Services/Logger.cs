using System;
using System.Diagnostics;
using JetBrains.Annotations;
using StardewModdingAPI;

namespace TimeSpeed.Services
{
    [PublicAPI]
    public class Logger
    {
        protected readonly string Name;

        public Logger(string name)
        {
            Name = name;
        }

        private string FormatMessage(string message) => $"[{Name}] {message}";

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

        public void Error(Exception exception)
        {
            Log.Error(FormatMessage($"[{exception.GetType().Name}] {exception.Message}"));
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

        [Conditional("DEBUG")]
        public void DebugError(Exception exception)
        {
            Log.Error(FormatMessage($"[{exception.GetType().Name}] {exception.Message}"));
        }
    }
}
