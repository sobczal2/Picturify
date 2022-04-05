using System.Collections.Generic;
using System.Threading;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class MultipleConvolutionProcessorF : BaseProcessor<MultipleConvolutionParams, FastImageF>
    {
        private List<ConvolutionProcessorF> _convolutionProcessors;
        public MultipleConvolutionProcessorF(MultipleConvolutionParams processorParams) : base(processorParams)
        {
            _convolutionProcessors = new List<ConvolutionProcessorF>();
            foreach (var matrix in processorParams.ConvolutionMatrixes)
            {
                _convolutionProcessors.Add(new ConvolutionProcessorF(new ConvolutionParams(
                    processorParams.ChannelSelector, matrix, processorParams.EdgeBehaviourType,
                    processorParams.WorkingArea)));
            }
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            foreach (var processor in _convolutionProcessors)
            {
                fastImage.ExecuteProcessor(processor);
            }
            return base.Process(fastImage, cancellationToken);
        }
    }
}