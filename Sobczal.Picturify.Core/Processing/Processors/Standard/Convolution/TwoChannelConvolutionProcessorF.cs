using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Standard.Util;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class TwoChannelConvolutionProcessorF : BaseProcessor<TwoChannelConvolutionParams, FastImageF>
    {
        private MultipleConvolutionProcessorF _processorFChannel1;
        private MultipleConvolutionProcessorF _processorFChannel2;
        public TwoChannelConvolutionProcessorF(TwoChannelConvolutionParams processorParams) : base(processorParams)
        {
            _processorFChannel1 = new MultipleConvolutionProcessorF(new MultipleConvolutionParams(
                processorParams.ChannelSelector, processorParams.ConvolutionMatrixesChannel1,
                processorParams.EdgeBehaviourType, processorParams.WorkingArea));
            _processorFChannel2 = new MultipleConvolutionProcessorF(new MultipleConvolutionParams(
                processorParams.ChannelSelector, processorParams.ConvolutionMatrixesChannel2,
                processorParams.EdgeBehaviourType, processorParams.WorkingArea));
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var tempFastImage = fastImage.GetCopy();
            tempFastImage.ExecuteProcessor(_processorFChannel1);
            fastImage.ExecuteProcessor(_processorFChannel2);
            fastImage.ExecuteProcessor(new MergeProcessor(new MergeParams(ProcessorParams.ChannelSelector,
                ProcessorParams.MergingFunc, tempFastImage, ProcessorParams.WorkingArea)));
            return base.Process(fastImage, cancellationToken);
        }
    }
}