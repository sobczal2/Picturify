using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard.Util
{
    public class NormalisationProcessor : BaseProcessor<NormalisationParams, FastImageF>
    {
        private float minVal;
        private float maxVal;
        private float multiplyByVal;
        public NormalisationProcessor(NormalisationParams processorParams) : base(processorParams)
        {
            if (ProcessorParams.PreNormalisationFunction is null)
                ProcessorParams.PreNormalisationFunction = x => x;
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            ((FastImageF) fastImage).Process(ProcessingFunction, cancellationToken);
            var pmp = new PointManipulationProcessor(new PointManipulationParams(ProcessorParams.ChannelSelector,
                PointManipulationFunction, ProcessorParams.WorkingArea));
            multiplyByVal = 1f / (maxVal - minVal);
            fastImage = fastImage.ExecuteProcessor(pmp);
            return base.Process(fastImage, cancellationToken);
        }

        private float[,,] ProcessingFunction(float[,,] pixels, CancellationToken cancellationToken)
        {
            minVal = float.MaxValue;
            maxVal = float.MinValue;
            var depth = pixels.GetLength(2);
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            Parallel.For(ProcessorParams.WorkingArea.LeftInclusive, ProcessorParams.WorkingArea.RightExclusive, po, i =>
            {
                for (var j = ProcessorParams.WorkingArea.BotInclusive;
                     j < ProcessorParams.WorkingArea.TopExclusive;
                     j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        if(!ProcessorParams.ChannelSelector.Used(k) || !ProcessorParams.WorkingArea.ShouldEdit(i, j)) continue;
                        if (pixels[i, j, k] > maxVal) maxVal = pixels[i, j, k];
                        else if (pixels[i, j, k] < minVal) minVal = pixels[i, j, k];
                    }
                }
            });
            return pixels;
        }

        private (float a, float r, float g, float b) PointManipulationFunction(float a, float r, float g, float b, int x,
            int y, ChannelSelector channelSelector)
        {
            if (channelSelector.UseAlpha) a = (a - minVal) * multiplyByVal;
            if (channelSelector.UseRed) r = (r - minVal) * multiplyByVal;
            if (channelSelector.UseGreen) g = (g - minVal) * multiplyByVal;
            if (channelSelector.UseBlue) b = (b - minVal) * multiplyByVal;
            return (a, r, g, b);
        }
    }
}