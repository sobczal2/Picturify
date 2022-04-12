using System.CommandLine;
using Sobczal.Picturify.CLI.Util;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.CLI.Core.Processors.Blur
{
    public class CliGaussianBlurProcessor : CliProcessor
    {
        public CliGaussianBlurProcessor() : base("GaussBlur",
            "Gaussian blur. Performs convolution on an image using gaussian kernel.")
        {
        }


        public override Command GetCommand()
        {
            var pSizeOpt = CommonOptions.GetPSizeOption(new PSize(100, 100), new[] {"-ks", "--kernel-size"},
                "Set size of kernel(range).", new PSize(7, 7));
            Command.AddOption(pSizeOpt);
            var channelOpt = CommonOptions.GetChannelOption();
            Command.AddOption(channelOpt);
            var edgeOpt = CommonOptions.GetEdgeOption();
            Command.AddOption(edgeOpt);
            var sigmaOpt = new Option<float>(new []{"-s", "--sigma"}, "Set sigma in gaussian kernel.");
            sigmaOpt.SetDefaultValue(1.4f);
            Command.AddOption(sigmaOpt);
            Command.SetHandler((string input, string output, PSize psize, ChannelSelector channelSelector,
                EdgeBehaviourSelector.Type edgeBehaviour, float sigma, bool toGrayscale) =>
            {
                var fastImage = FastImageFactory.FromFile(input);
                if (toGrayscale) fastImage = fastImage.ToGrayscale();
                fastImage = fastImage.ExecuteProcessor(
                    new GaussianBlurProcessor(new GaussianBlurParams(channelSelector, psize, sigma, edgeBehaviour)));
                fastImage.Save(output);
            }, InputArgument, OutputArgument, pSizeOpt, channelOpt, edgeOpt, sigmaOpt, ToGrayscaleOpt);
            return Command;
        }
    }
}