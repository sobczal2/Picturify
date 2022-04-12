using System.CommandLine;

namespace Sobczal.Picturify.CLI.Core.Processors
{
    public abstract class CliProcessor
    {
        protected Command Command;
        protected Argument<string> InputArgument;
        protected Argument<string> OutputArgument;
        protected Option<bool> ToGrayscaleOpt;

        protected CliProcessor(string name, string description)
        {
            Command = new Command(name, description);
            InputArgument = new Argument<string>("input", "input file");
            OutputArgument = new Argument<string>("output", "output file");
            Command.AddArgument(InputArgument);
            Command.AddArgument(OutputArgument);
            ToGrayscaleOpt = new Option<bool>(new[] {"-gs", "--grayscale"}, "Toggle grayscale.");
            ToGrayscaleOpt.SetDefaultValue(false);
            Command.AddOption(ToGrayscaleOpt);
        }
        public abstract Command GetCommand();
    }
}