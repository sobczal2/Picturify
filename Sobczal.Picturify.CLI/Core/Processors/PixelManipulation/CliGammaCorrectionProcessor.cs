using System.CommandLine;
using Sobczal.Picturify.CLI.Util;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.PixelManipulation;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.CLI.Core.Processors.PixelManipulation
{
    public class CliGammaCorrectionProcessor : CliProcessor
    {
        public CliGammaCorrectionProcessor() : base("Gamma", "Gamma correction.")
        {
        }


        public override Command GetCommand()
        {
            var channelOpt = CommonOptions.GetChannelOption();
            Command.AddOption(channelOpt);
            var gammaOpt = new Option<float>(new[] {"-g", "--gamma"},
                "Gamma value");
            gammaOpt.IsRequired = true;
            Command.AddOption(gammaOpt);
            Command.SetHandler((string input, string output, ChannelSelector channelSelector, float gamma, bool toGrayscale) =>
                {
                    var fastImage = FastImageFactory.FromFile(input);
                    if (toGrayscale) fastImage = fastImage.ToGrayscale();
                    fastImage.ExecuteProcessor(
                        new GammaCorrectionProcessor(new GammaCorrectionParams(channelSelector, gamma)));
                    fastImage.Save(output);
                }, InputArgument, OutputArgument, channelOpt, gammaOpt, ToGrayscaleOpt);
            return Command;
        }
    }
}