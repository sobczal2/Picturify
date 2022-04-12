using System;
using System.Collections.Generic;
using System.CommandLine;
using Sobczal.Picturify.CLI.Core;

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

            rootCommand.Invoke(args);
        }
    }
}