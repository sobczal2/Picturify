using System;
using System.Text;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Sobczal.Picturify.Core
{
    public static class PicturifyConfig
    {

        public static int Indent { get; set; }
        public static bool UseIndentation { get; set; }
        static PicturifyConfig()
        {
            Indent = 0;
            UseIndentation = true;
            ConfigureLogging(LoggingLevel.Information);
        }

        private static void ConfigureLogging(LoggingLevel loggingLevel)
        {
            Logger log;
            switch (loggingLevel)
            {
                case LoggingLevel.Verbose:
                    log = new LoggerConfiguration()
                        .MinimumLevel.Verbose()
                        .WriteTo.Console()
                        .CreateLogger();
                    break;
                case LoggingLevel.Debug:
                    log = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .CreateLogger();
                    break;
                case LoggingLevel.Warning:
                    log = new LoggerConfiguration()
                        .MinimumLevel.Warning()
                        .WriteTo.Console()
                        .CreateLogger();
                    break;
                case LoggingLevel.Error:
                    log = new LoggerConfiguration()
                        .MinimumLevel.Error()
                        .WriteTo.Console()
                        .CreateLogger();
                    break;
                case LoggingLevel.Fatal:
                    log = new LoggerConfiguration()
                        .MinimumLevel.Fatal()
                        .WriteTo.Console()
                        .CreateLogger();
                    break;
                default:
                    log = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console()
                        .CreateLogger();
                    break;
            }
            Log.Logger = log;
        }

        public enum LoggingLevel
        {
            Verbose,
            Debug,
            Information,
            Warning,
            Error,
            Fatal
        }

        public static void SetLoggingLevel(LoggingLevel loggingLevel)
        {
            ConfigureLogging(loggingLevel);
        }
    
        public static void LogDebug(string message)
        {
            Log.Debug($"{GetIndent()}{message}");
        }

        public static void LogInfo(string message)
        {
            Log.Information($"{GetIndent()}{message}");
        }
    
        public static void LogWarn(string message)
        {
            Log.Warning($"{GetIndent()}{message}");
        }
    
        public static void LogError(string message)
        {
            Log.Error($"{GetIndent()}{message}");
        }

        public static void LogTimeDebug(string name, long timeMs)
        {
            Log.Debug($"{GetIndent()}{name} took {timeMs} ms.");
        }
        
        public static void LogTime(string name, long timeMs)
        {
            Log.Information($"{GetIndent()}{name} took {timeMs} ms.");
        }

        private static string GetIndent()
        {
            if (!UseIndentation) return string.Empty;
            var sb = new StringBuilder();
            for (var i = 0; i < Indent; i++)
            {
                sb.Append("|\t");
            }

            return sb.ToString();
        }
    }
}