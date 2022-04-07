using System.Collections.Generic;
using Sobczal.Picturify.Core.Processing.Standard;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.Standard.Convolution
{
    public class MultipleConvolutionProcessorTests : AbstractProcessorTests<MultipleConvolutionProcessor>
    {
        protected override void PopulateChannelSelectorCheckProcessors()
        {
            var convolutionMatrix = new float[,] {{0.1f, 0.1f, 0.1f}, {0.1f, 0.2f, 0.1f}, {0.1f, 0.1f, 0.1f}};
            ChannelSelectorCheckProcessor = new MultipleConvolutionProcessor(new MultipleConvolutionParams(ChannelSelector.ARGB,
                new List<float[,]>() {convolutionMatrix, convolutionMatrix}, EdgeBehaviourSelector.Type.Constant,
                null));
        }

        protected override void PopulateSizeCheckProcessors()
        {
            var convolutionMatrix = new float[,] {{0.1f, 0.1f, 0.1f}, {0.1f, 0.2f, 0.1f}, {0.1f, 0.1f, 0.1f}};
            SizeCheckProcessors.Add(new MultipleConvolutionProcessor(new MultipleConvolutionParams(ChannelSelector.A,
                new List<float[,]>() {convolutionMatrix, convolutionMatrix}, EdgeBehaviourSelector.Type.Constant,
                null)));
            SizeCheckProcessors.Add(new MultipleConvolutionProcessor(new MultipleConvolutionParams(ChannelSelector.RGB,
                new List<float[,]>() {convolutionMatrix, convolutionMatrix}, EdgeBehaviourSelector.Type.Constant,
                new SquareAreaSelector(2, 8, 2, 8))));
            SizeCheckProcessors.Add(new MultipleConvolutionProcessor(new MultipleConvolutionParams(ChannelSelector.ARGB,
                new List<float[,]>() {convolutionMatrix, convolutionMatrix}, EdgeBehaviourSelector.Type.Constant,
                new SquareAreaSelector(3, 7, 3, 7))));
        }
    }
}