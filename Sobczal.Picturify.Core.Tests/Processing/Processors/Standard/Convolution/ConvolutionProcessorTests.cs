using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Processing.Standard;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.Standard.Convolution
{
    public class ConvolutionProcessorTests : AbstractProcessorTests<ConvolutionProcessor>
    {
        protected override void PopulateWorkingAreaCheckProcessors()
        {
            var convolutionMatrix = new float[,] {{0.1f, 0.1f, 0.1f}, {0.1f, 0.2f, 0.1f}, {0.1f, 0.1f, 0.1f},};
            WorkingAreaCheckProcessor = new ConvolutionProcessor(new ConvolutionParams(ChannelSelector.ARGB, convolutionMatrix,
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateChannelSelectorCheckProcessors()
        {
            var convolutionMatrix = new float[,] {{0.1f, 0.1f, 0.1f}, {0.1f, 0.2f, 0.1f}, {0.1f, 0.1f, 0.1f},};
            ChannelSelectorCheckProcessor = new ConvolutionProcessor(new ConvolutionParams(ChannelSelector.ARGB, convolutionMatrix,
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateSizeCheckProcessors()
        {
            var convolutionMatrix = new float[,] {{0.1f, 0.1f, 0.1f}, {0.1f, 0.2f, 0.1f}, {0.1f, 0.1f, 0.1f},};
            SizeCheckProcessors.Add(new ConvolutionProcessor(new ConvolutionParams(ChannelSelector.A, convolutionMatrix,
                EdgeBehaviourSelector.Type.Constant, null)));
            SizeCheckProcessors.Add(new ConvolutionProcessor(new ConvolutionParams(ChannelSelector.RGB, convolutionMatrix,
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(2, 8, 2, 8))));
            SizeCheckProcessors.Add(new ConvolutionProcessor(new ConvolutionParams(ChannelSelector.ARGB, convolutionMatrix,
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(3, 7, 3, 7))));
        }
    }
}