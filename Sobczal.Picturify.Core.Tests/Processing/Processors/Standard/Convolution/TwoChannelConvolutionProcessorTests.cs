using System.Collections.Generic;
using Sobczal.Picturify.Core.Processing.Standard;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.Standard.Convolution
{
    public class TwoChannelConvolutionProcessorTests : AbstractProcessorTests<TwoChannelConvolutionProcessor>
    {
        protected override void PopulateWorkingAreaCheckProcessors()
        {
            var convolutionMatrix = new float[,] {{0.1f, 0.1f, 0.1f}, {0.1f, 0.2f, 0.1f}, {0.1f, 0.1f, 0.1f}};
            WorkingAreaCheckProcessor = new TwoChannelConvolutionProcessor(new TwoChannelConvolutionParams(
                ChannelSelector.ARGB, new List<float[,]>() {convolutionMatrix},
                new List<float[,]>() {convolutionMatrix, convolutionMatrix}, (in1, in2, channel) => (in1 + in2) * 0.5f, false,
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateChannelSelectorCheckProcessors()
        {
            var convolutionMatrix = new float[,] {{0.1f, 0.1f, 0.1f}, {0.1f, 0.2f, 0.1f}, {0.1f, 0.1f, 0.1f}};
            ChannelSelectorCheckProcessor = new TwoChannelConvolutionProcessor(new TwoChannelConvolutionParams(
                ChannelSelector.ARGB, new List<float[,]>() {convolutionMatrix},
                new List<float[,]>() {convolutionMatrix, convolutionMatrix}, (in1, in2, channel) => (in1 + in2) * 0.5f, false,
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateSizeCheckProcessors()
        {
            var convolutionMatrix = new float[,] {{0.1f, 0.1f, 0.1f}, {0.1f, 0.2f, 0.1f}, {0.1f, 0.1f, 0.1f}};
            SizeCheckProcessors.Add(new TwoChannelConvolutionProcessor(new TwoChannelConvolutionParams(
                ChannelSelector.A, new List<float[,]>() {convolutionMatrix},
                new List<float[,]>() {convolutionMatrix, convolutionMatrix}, (in1, in2, channel) => (in1 + in2) * 0.5f, false,
                EdgeBehaviourSelector.Type.Constant, null)));
            SizeCheckProcessors.Add(new TwoChannelConvolutionProcessor(new TwoChannelConvolutionParams(
                ChannelSelector.RGB, new List<float[,]>() {convolutionMatrix},
                new List<float[,]>() {convolutionMatrix, convolutionMatrix}, (in1, in2, channel) => (in1 + in2) * 0.5f, false,
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(2, 8, 2, 8))));
            SizeCheckProcessors.Add(new TwoChannelConvolutionProcessor(new TwoChannelConvolutionParams(
                ChannelSelector.ARGB, new List<float[,]>() {convolutionMatrix},
                new List<float[,]>() {convolutionMatrix, convolutionMatrix}, (in1, in2, channel) => (in1 + in2) * 0.5f, false,
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(3, 7, 3, 7))));
        }
    }
}