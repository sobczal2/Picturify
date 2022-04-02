using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Sobczal.Picturify.Core
{
    public static class PicturifyConfig
    {

        static PicturifyConfig()
        {
            ConfigureLogging();
        }
        public static void ConfigureLogging()
        {
            var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger = log;
        }
    
        public static void LogDebug(string message)
        {
            Log.Debug(message);
        }

        public static void LogInfo(string message)
        {
            Log.Information(message);
        }
    
        public static void LogWarn(string message)
        {
            Log.Warning(message);
        }
    
        public static void LogError(string message)
        {
            Log.Error(message);
        }

        public static void LogTimeDebug(string name, long timeMs)
        {
            Log.Debug($"{name} took {timeMs} ms.");
        }
        
        public static void LogTime(string name, long timeMs)
        {
            Log.Information($"{name} took {timeMs} ms.");
        }
    }
}