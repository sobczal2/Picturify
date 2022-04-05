using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Standard.Util;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class TwoChannelConvolutionProcessor : BaseProcessor<TwoChannelConvolutionParams, FastImageF>
    {
        private MultipleConvolutionProcessor _processorChannel1;
        private MultipleConvolutionProcessor _processorChannel2;
        public TwoChannelConvolutionProcessor(TwoChannelConvolutionParams processorParams) : base(processorParams)
        {
            _processorChannel1 = new MultipleConvolutionProcessor(new MultipleConvolutionParams(
                processorParams.ChannelSelector, processorParams.ConvolutionMatrixesChannel1,
                processorParams.EdgeBehaviourType, processorParams.WorkingArea));
            _processorChannel2 = new MultipleConvolutionProcessor(new MultipleConvolutionParams(
                processorParams.ChannelSelector, processorParams.ConvolutionMatrixesChannel2,
                processorParams.EdgeBehaviourType, processorParams.WorkingArea));
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var tempFastImage = fastImage.GetCopy();
            tempFastImage.ExecuteProcessor(_processorChannel1);
            fastImage.ExecuteProcessor(_processorChannel2);
            fastImage.ExecuteProcessor(new MergeProcessor(new MergeParams(ProcessorParams.ChannelSelector,
                ProcessorParams.MergingFunc, tempFastImage, ProcessorParams.WorkingArea)));
            return base.Process(fastImage, cancellationToken);
        }
    }
}