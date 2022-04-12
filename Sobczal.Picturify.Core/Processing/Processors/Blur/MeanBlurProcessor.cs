using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Standard;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    public class MeanBlurProcessor : BaseProcessor<MeanBlurParams, FastImageF>
    {
        public MeanBlurProcessor(MeanBlurParams processorParams) : base(processorParams)
        {
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var kernelX = GenKernelX();
            var kernelY = GenKernelY();
            var mcp = new MultipleConvolutionProcessor(new MultipleConvolutionParams(ChannelSelector.RGB,
                new List<float[,]> {kernelX, kernelY}, ProcessorParams.EdgeBehaviourType, ProcessorParams.WorkingArea));
            fastImage.ExecuteProcessor(mcp);
            return base.Process(fastImage, cancellationToken);
        }

        private float[,] GenKernelX()
        {
            var rangeX = ProcessorParams.Range.Width;
            var kernelX = new float[rangeX * 2 + 1, 1];
            var value = 1.0f / (2 * rangeX + 1);
            for (var i = 0; i < rangeX * 2 + 1; i++)
            {
                kernelX[i, 0] = value;
            }
            return kernelX;
        }
        
        private float[,] GenKernelY()
        {
            var rangeY = ProcessorParams.Range.Height;
            var kernelY = new float[1, rangeY * 2 + 1];
            var value = 1.0f / (2 * rangeY + 1);
            for (var i = 0; i < rangeY * 2 + 1; i++)
            {
                kernelY[0, i] = value;
            }
            return kernelY;
        }
    }
}