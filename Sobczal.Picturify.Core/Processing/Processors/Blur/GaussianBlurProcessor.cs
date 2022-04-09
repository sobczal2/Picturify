using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Standard;
using Sobczal.Picturify.Core.Processing.Standard.Util;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    public class GaussianBlurProcessor : BaseProcessor<GaussianBlurParams, FastImageF>
    {
        public GaussianBlurProcessor(GaussianBlurParams processorParams) : base(processorParams)
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
            var sigma2 = ProcessorParams.Sigma * ProcessorParams.Sigma;
            for (var i = 0; i < rangeX * 2 + 1; i++)
            {
                kernelX[i, 0] = (float) (1f / Math.Sqrt(2f * Math.PI * sigma2) *
                                         Math.Exp(-(i - rangeX) * (i - rangeX) / (2 * sigma2)));
            }

            var sum = kernelX.Cast<float>().Sum(x => x);
            var multiplyVal = 1f / sum;
            for (var i = 0; i < rangeX * 2 + 1; i++)
            {
                kernelX[i, 0] *= multiplyVal;
            }
            return kernelX;
        }
        
        private float[,] GenKernelY()
        {
            var rangeY = ProcessorParams.Range.Height;
            var kernelY = new float[1, rangeY * 2 + 1];
            var sigma2 = ProcessorParams.Sigma * ProcessorParams.Sigma;
            for (var i = 0; i < rangeY * 2 + 1; i++)
            {
                kernelY[0, i] = (float) (1f / Math.Sqrt(2f * Math.PI * sigma2) *
                                         Math.Exp(-(i - rangeY) * (i - rangeY) / (2 * sigma2)));
            }
            var sum = kernelY.Cast<float>().Sum(x => x);
            var multiplyVal = 1f / sum;
            for (var i = 0; i < rangeY * 2 + 1; i++)
            {
                kernelY[0, i] *= multiplyVal;
            }
            return kernelY;
        }
    }
}