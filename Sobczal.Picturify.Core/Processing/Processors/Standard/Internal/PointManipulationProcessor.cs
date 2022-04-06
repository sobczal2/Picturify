using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Standard.Util
{
    public class PointManipulationProcessor : BaseProcessor<PointManipulationParams, FastImageF>
    {
        public PointManipulationProcessor(PointManipulationParams processorParams) : base(processorParams)
        {
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            ((FastImageF) fastImage).Process(ProcessingFunction, cancellationToken);
            return base.Process(fastImage, cancellationToken);
        }

        private float[,,] ProcessingFunction(float[,,] pixels, CancellationToken cancellationToken)
        {
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            var depth = pixels.GetLength(2);
            Parallel.For(ProcessorParams.WorkingArea.LeftInclusive, ProcessorParams.WorkingArea.RightExclusive, po, i =>
            {
                for (var j = ProcessorParams.WorkingArea.BotInclusive;
                     j < ProcessorParams.WorkingArea.TopExclusive;
                     j++)
                {
                    if (depth == 4)
                    {
                        if(!ProcessorParams.WorkingArea.ShouldEdit(i, j)) continue;
                        (pixels[i, j, 0], pixels[i, j, 1], pixels[i, j, 2], pixels[i, j, 3]) =
                            ProcessorParams.PointManipulationFunction(pixels[i, j, 0], pixels[i, j, 1], pixels[i, j, 2],
                                pixels[i, j, 3], i, j, ProcessorParams.ChannelSelector);
                    }
                    else
                    {
                        if(!ProcessorParams.WorkingArea.ShouldEdit(i, j)) continue;
                        (pixels[i, j, 0], _, _, _) =
                            ProcessorParams.PointManipulationFunction(pixels[i, j, 0], 0f, 0f,
                                0f, i, j, ProcessorParams.ChannelSelector);
                    }

                }
            });
            return pixels;
        }
    }
}