using System.CommandLine;
using Sobczal.Picturify.CLI.Util;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Data.Operators.EdgeDetection;
using Sobczal.Picturify.Core.Processing.EdgeDetection;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.CLI.Core.Processors.EdgeDetection
{
    public class CliScharr3Processor : CliProcessor
    {
        public CliScharr3Processor() : base("Scharr3", "Edge detection using scharr operator 3x3 with optional non-maximum suppression.")
        {
        }


        public override Command GetCommand()
        {
            var channelOpt = CommonOptions.GetChannelOption();
            Command.AddOption(channelOpt);
            var edgeOpt = CommonOptions.GetEdgeOption();
            Command.AddOption(edgeOpt);
            var mappingFuncOpt = new Option<OperatorBeforeNormalizationFunc>(new[] {"-mf", "--mapping-function"},
                "Function used just before normalization.");
            mappingFuncOpt.SetDefaultValue(OperatorBeforeNormalizationFunc.Log);
            Command.AddOption(mappingFuncOpt);
            var lowerBoundOpt = new Option<float>(new[] {"-lb", "--lower-bound"},
                "Lower bound on normalisation. Value of 0 is highly recommended.");
            lowerBoundOpt.SetDefaultValue(0f);
            Command.AddOption(lowerBoundOpt);
            var upperBoundOpt = new Option<float>(new[] {"-ub", "--upper-bound"},
                "Upper bound on normalisation. Value of 1 is highly recommended.");
            upperBoundOpt.SetDefaultValue(1f);
            Command.AddOption(upperBoundOpt);
            var useNonMaxSuppressionOpt = new Option<bool>(new[] {"-nms", "--non-max-suppression"},
                "Toggle non-maximum suppression. Set to true if you want all your lines to be 1px width.");
            useNonMaxSuppressionOpt.SetDefaultValue(false);
            Command.AddOption(useNonMaxSuppressionOpt);
            Command.SetHandler((string input, string output, ChannelSelector channelSelector,
                    EdgeBehaviourSelector.Type edgeBehaviour, OperatorBeforeNormalizationFunc mappingFunc,
                    float lowerBound,
                    float upperBound,
                    bool useNonMaxSuppression, bool toGrayscale) =>
                {
                    var fastImage = FastImageFactory.FromFile(input);
                    if (toGrayscale) fastImage = fastImage.ToGrayscale();
                    fastImage = fastImage.ExecuteProcessor(
                        new DualOperatorProcessor(new DualOperatorParams(channelSelector, new ScharrOperator3(),
                            mappingFunc, lowerBound, upperBound, useNonMaxSuppression, edgeBehaviour)));
                    fastImage.Save(output);
                }, InputArgument, OutputArgument, channelOpt, edgeOpt, mappingFuncOpt, lowerBoundOpt,
                upperBoundOpt, useNonMaxSuppressionOpt, ToGrayscaleOpt);
            return Command;
        }
    }
}