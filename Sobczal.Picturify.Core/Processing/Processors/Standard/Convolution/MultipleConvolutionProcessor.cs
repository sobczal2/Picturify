using System.Collections.Generic;
using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class MultipleConvolutionProcessor : BaseProcessor<MultipleConvolutionParams, FastImageF>
    {
        private List<ConvolutionProcessor> _convolutionProcessors;
        private readonly int _maxKernelX;
        private readonly int _maxKernelY;
        public MultipleConvolutionProcessor(MultipleConvolutionParams processorParams) : base(processorParams)
        {
            foreach (var matrix in ProcessorParams.ConvolutionMatrixes)
            {
                if (matrix.GetLength(0) > _maxKernelX) _maxKernelX = matrix.GetLength(0);
                if (matrix.GetLength(1) > _maxKernelY) _maxKernelY = matrix.GetLength(1);
            }
            _convolutionProcessors = new List<ConvolutionProcessor>();
            AddFilter(EdgeBehaviourSelector.GetFilter(processorParams.EdgeBehaviourType, new PSize(_maxKernelX / 2, _maxKernelY / 2)));
        }

        public override IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            base.Before(fastImage, cancellationToken);
            foreach (var matrix in ProcessorParams.ConvolutionMatrixes)
            {
                var procesor = new ConvolutionProcessor(new ConvolutionParams(
                    ProcessorParams.ChannelSelector, matrix, ProcessorParams.EdgeBehaviourType,
                    ProcessorParams.WorkingArea));
                procesor.ClearFilters();
                _convolutionProcessors.Add(procesor);
            }

            return fastImage;
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