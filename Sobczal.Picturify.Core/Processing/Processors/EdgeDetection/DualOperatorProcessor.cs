using System;
using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Data.Operators.EdgeDetection;
using Sobczal.Picturify.Core.Processing.Standard;
using Sobczal.Picturify.Core.Processing.Standard.Util;

namespace Sobczal.Picturify.Core.Processing.EdgeDetection
{
    public class DualOperatorProcessor : BaseProcessor<DualOperatorParams, FastImageF>
    {
        private TwoChannelConvolutionProcessor _twoChannelConvolutionProcessor;
        private NormalisationProcessor _normalisationProcessor;
        public DualOperatorProcessor(DualOperatorParams processorParams) : base(processorParams)
        {
        }

        public override IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            fastImage = base.Before(fastImage, cancellationToken);

            MergeParams.MergingFunc mergingFunc;
            switch (ProcessorParams.MappingFunc)
            {
                case OperatorBeforeNormalizationFunc.Root2:
                    mergingFunc = (in1, in2, channel) =>
                        (float) Math.Pow(in1 * in1 + in2 * in2, 0.5);
                    break;
                case OperatorBeforeNormalizationFunc.Root3:
                    mergingFunc = (in1, in2, channel) =>
                        (float) Math.Pow(in1 * in1 + in2 * in2, 0.33333);
                    break;
                case OperatorBeforeNormalizationFunc.Root4:
                    mergingFunc = (in1, in2, channel) =>
                        (float) Math.Pow(in1 * in1 + in2 * in2, 0.25);
                    break;
                case OperatorBeforeNormalizationFunc.Root5:
                    mergingFunc = (in1, in2, channel) =>
                        (float) Math.Pow(in1 * in1 + in2 * in2, 0.2);
                    break;
                default:
                    mergingFunc = (in1, in2, channel) =>
                        in1 * in1 + in2 * in2;
                    break;
            }

            _twoChannelConvolutionProcessor = new TwoChannelConvolutionProcessor(
                new TwoChannelConvolutionParams(ProcessorParams.ChannelSelector,
                    ProcessorParams.ConvolutionOperator.GetX(), ProcessorParams.ConvolutionOperator.GetY(), mergingFunc,
                    ProcessorParams.EdgeBehaviourType, ProcessorParams.WorkingArea));
            _normalisationProcessor = new NormalisationProcessor(new NormalisationParams(
                ProcessorParams.ChannelSelector, null, ProcessorParams.LowerBoundForNormalisation,
                ProcessorParams.UpperBoundForNormalisation, ProcessorParams.WorkingArea));
            return fastImage;
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            fastImage = fastImage.ExecuteProcessor(_twoChannelConvolutionProcessor).ExecuteProcessor(_normalisationProcessor);
            return base.Process(fastImage, cancellationToken);
        }
    }
}