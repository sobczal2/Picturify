using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Sobczal.Picturify.CLI.Core;
using Sobczal.Picturify.Core;

namespace Sobczal.Picturify.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var packageCommands = new List<Command>();
            packageCommands.Add(CoreCli.GetCoreCommand());
            packageCommands.Add(new Command("Movie"));

            var rootCommand = new RootCommand("PicturifyCli is a command line interface for Picturify. Have fun!");
            foreach (var command in packageCommands)
            {
                rootCommand.AddCommand(command);
            }

            var _ = new CommandLineBuilder(rootCommand)
                .UseVersionOption()
                .UseHelp()
                .UseEnvironmentVariableDirective()
                .UseParseDirective()
                .UseSuggestDirective()
                .RegisterWithDotnetSuggest()
                .UseTypoCorrections()
                .UseParseErrorReporting()
                .CancelOnProcessTermination()
                .UseExceptionHandler((exception, context) =>
                {
                    PicturifyConfig.LogError(exception.Message);
                    context.ExitCode = 1;
                })
                .Build()
                .Invoke(args);
        }
    }
}