using System.CommandLine;
using Sobczal.Picturify.CLI.Util;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.CLI.Core.Processors.Blur
{
    public class CliMedianBlurProcessor : CliProcessor
    {
        public CliMedianBlurProcessor() : base("MedianBlur", "Median blur. Sets value of a pixel to median of surrounding pixels in range.")
        {
        }


        public override Command GetCommand()
        {
            var pSizeOpt = CommonOptions.GetPSizeOption(new PSize(100, 100), new[] {"-ks", "--kernel-size"},
                "Set size of kernel(range).", new PSize(7,7));
            Command.AddOption(pSizeOpt);
            var channelOpt = CommonOptions.GetChannelOption();
            Command.AddOption(channelOpt);
            var edgeOpt = CommonOptions.GetEdgeOption();
            Command.AddOption(edgeOpt);
            Command.SetHandler((string input, string output, PSize psize, ChannelSelector channelSelector,
                EdgeBehaviourSelector.Type edgeBehaviour, bool toGrayscale) =>
            {
                var fastImage = FastImageFactory.FromFile(input);
                if (toGrayscale) fastImage = fastImage.ToGrayscale();
                fastImage = fastImage.ExecuteProcessor(
                    new MedianBlurProcessor(new MedianBlurParams(channelSelector, psize, edgeBehaviour)));
                fastImage.Save(output);
            }, InputArgument, OutputArgument, pSizeOpt, channelOpt, edgeOpt, ToGrayscaleOpt);
            return Command;
        }
    }
}