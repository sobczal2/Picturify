using System;
using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Data.Operators.EdgeDetection;
using Sobczal.Picturify.Core.Processing.Standard;
using Sobczal.Picturify.Core.Processing.Standard.Util;

namespace Sobczal.Picturify.Core.Processing.EdgeDetection
{
    public class QuadOperatorProcessor : BaseProcessor<QuadOperatorParams, FastImageF>
    {
        private TwoChannelConvolutionProcessor _hvTwoChannelConvolutionProcessor;
        private TwoChannelConvolutionProcessor _diagTwoChannelConvolutionProcessor;
        private NormalisationProcessor _normalisationProcessor;
        public QuadOperatorProcessor(QuadOperatorParams processorParams) : base(processorParams)
        {
        }
        
        public override IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            fastImage = base.Before(fastImage, cancellationToken);

            _hvTwoChannelConvolutionProcessor = new TwoChannelConvolutionProcessor(
                new TwoChannelConvolutionParams(ProcessorParams.ChannelSelector,
                    ProcessorParams.ConvolutionOperator.GetX(), ProcessorParams.ConvolutionOperator.GetY(),
                    (in1, in2, channel) => in1 * in1 + in2 * in2,
                    ProcessorParams.EdgeBehaviourType, ProcessorParams.WorkingArea));
            _diagTwoChannelConvolutionProcessor = new TwoChannelConvolutionProcessor(
                new TwoChannelConvolutionParams(ProcessorParams.ChannelSelector,
                    ProcessorParams.ConvolutionOperator.GetDiag1(), ProcessorParams.ConvolutionOperator.GetDiag2(),
                    (in1, in2, channel) => in1 * in1 + in2 * in2,
                    ProcessorParams.EdgeBehaviourType, ProcessorParams.WorkingArea));
            _normalisationProcessor = new NormalisationProcessor(new NormalisationParams(
                ProcessorParams.ChannelSelector, null, ProcessorParams.LowerBoundForNormalisation,
                ProcessorParams.UpperBoundForNormalisation, ProcessorParams.WorkingArea));
            return fastImage;
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var diagChan = fastImage.GetCopy();
            fastImage = fastImage.ExecuteProcessor(_hvTwoChannelConvolutionProcessor);
            diagChan = diagChan.ExecuteProcessor(_diagTwoChannelConvolutionProcessor);
            MergeParams.MergingFunc mergingFunc;
            switch (ProcessorParams.MappingFunc)
            {
                case OperatorBeforeNormalizationFunc.Root2:
                    mergingFunc = (in1, in2, channel) =>
                        (float) Math.Pow(in1 + in2, 0.5);
                    break;
                case OperatorBeforeNormalizationFunc.Root3:
                    mergingFunc = (in1, in2, channel) =>
                        (float) Math.Pow(in1 + in2, 0.33333);
                    break;
                case OperatorBeforeNormalizationFunc.Root4:
                    mergingFunc = (in1, in2, channel) =>
                        (float) Math.Pow(in1 + in2, 0.25);
                    break;
                case OperatorBeforeNormalizationFunc.Root5:
                    mergingFunc = (in1, in2, channel) =>
                        (float) Math.Pow(in1 + in2, 0.2);
                    break;
                case OperatorBeforeNormalizationFunc.Log:
                    mergingFunc = (in1, in2, channel) =>
                        (float) Math.Log(in1 * in1 + in2 * in2 + 1, 2);
                    break;
                default:
                    mergingFunc = (in1, in2, channel) =>
                        in1 + in2;
                    break;
            }

            fastImage = fastImage.ExecuteProcessor(new MergeProcessor(new MergeParams(ProcessorParams.ChannelSelector, mergingFunc,
                diagChan, ProcessorParams.WorkingArea)));
            fastImage = fastImage.ExecuteProcessor(_normalisationProcessor);
            return base.Process(fastImage, cancellationToken);
        }
    }
}