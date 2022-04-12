using System;
using System.CommandLine;
using System.Linq;
using System.Reflection;
using Sobczal.Picturify.CLI.Core.Processors;

namespace Sobczal.Picturify.CLI.Core
{
    public static class CoreCli
    {
        public static Command GetCoreCommand()
        {
            var coreCommand = new Command("Core", "Encapsulates all commands from Core package");
            foreach (var type in 
                     Assembly.GetAssembly(typeof(CliProcessor)).GetTypes()
                         .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(CliProcessor))))
            {
                coreCommand.AddCommand(((CliProcessor)Activator.CreateInstance(type)).GetCommand());
            }
            return coreCommand;
        }
    }
}