using System.CommandLine;
using Sobczal.Picturify.CLI.Util;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.PixelManipulation;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.CLI.Core.Processors.PixelManipulation
{
    public class CliNegativeProcessor : CliProcessor
    {
        public CliNegativeProcessor() : base("Negate", "Negative of an image.")
        {
        }


        public override Command GetCommand()
        {
            var channelOpt = CommonOptions.GetChannelOption();
            Command.AddOption(channelOpt);
            Command.SetHandler((string input, string output, ChannelSelector channelSelector, bool toGrayscale) =>
            {
                var fastImage = FastImageFactory.FromFile(input);
                if (toGrayscale) fastImage = fastImage.ToGrayscale();
                fastImage.ExecuteProcessor(new NegativeProcessor(new NegativeParams(channelSelector)));
                fastImage.Save(output);
            }, InputArgument, OutputArgument, channelOpt, ToGrayscaleOpt);
            return Command;
        }
    }
}