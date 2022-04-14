using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.EdgeDetection;
using Sobczal.Picturify.Core.Processing.Standard.Util;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class TwoChannelConvolutionProcessor : BaseProcessor<TwoChannelConvolutionParams, FastImageF>
    {
        private MultipleConvolutionProcessor _processorChannel1;
        private MultipleConvolutionProcessor _processorChannel2;
        public TwoChannelConvolutionProcessor(TwoChannelConvolutionParams processorParams) : base(processorParams)
        {
        }

        public override IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            base.Before(fastImage, cancellationToken);
            _processorChannel1 = new MultipleConvolutionProcessor(new MultipleConvolutionParams(
                ProcessorParams.ChannelSelector, ProcessorParams.ConvolutionMatrixesChannel1,
                ProcessorParams.EdgeBehaviourType, ProcessorParams.WorkingArea));
            _processorChannel2 = new MultipleConvolutionProcessor(new MultipleConvolutionParams(
                ProcessorParams.ChannelSelector, ProcessorParams.ConvolutionMatrixesChannel2,
                ProcessorParams.EdgeBehaviourType, ProcessorParams.WorkingArea.GetCopy()));
            return fastImage;
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var tempFastImage = fastImage.GetCopy().ToFloatRepresentation();
            tempFastImage.ExecuteProcessor(_processorChannel1);
            fastImage.ExecuteProcessor(_processorChannel2);
            if (ProcessorParams.UseNonMaximumSuppression)
                fastImage.ExecuteProcessor(new NonMaximumGradientSuppressionProcessor(
                    new NonMaximumGradientSuppressionParams(ProcessorParams.WorkingArea,
                        ProcessorParams.ChannelSelector, tempFastImage, ProcessorParams.EdgeBehaviourType)));
            fastImage.ExecuteProcessor(new MergeProcessor(new MergeParams(ProcessorParams.ChannelSelector,
                ProcessorParams.MergingFunc, tempFastImage, ProcessorParams.WorkingArea)));
            return base.Process(fastImage, cancellationToken);
        }
    }
}