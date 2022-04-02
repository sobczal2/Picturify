using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Filters.EdgeBehaviour;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class RollingBucketProcessor : BaseProcessor<RollingBucketParams, FastImageB>
    {
        public RollingBucketProcessor(RollingBucketParams processorParams) : base(processorParams)
        {
            if (processorParams.ChannelSelector is null || processorParams.CalculateOneFunc is null)
                throw new ArgumentNullException(nameof(processorParams), "Processor param members can't be null.");
            AddFilter(EdgeBehaviourSelector.GetFilter(processorParams.EdgeBehaviourType, processorParams.PSize));
        }

        public override Task<IFastImage> Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            ((FastImageB) fastImage).Process(ProcessingFunction, cancellationToken);
            return base.Process(fastImage, cancellationToken);
        }

        private byte[,,] ProcessingFunction(byte[,,] pixels, CancellationToken cancellationToken)
        {
            var width = pixels.GetLength(0);
            var height = pixels.GetLength(1);
            var depth = pixels.GetLength(2);
            var buckets = new ThreadLocal<ushort[,]>();
            var tempArr = new byte[width, height, depth];
            Array.Copy(pixels, tempArr, pixels.Length);
            var po = new ParallelOptions
            {
                CancellationToken = cancellationToken
            };
            var rangeX = ProcessorParams.PSize.Width / 2;
            var rangeY = ProcessorParams.PSize.Height / 2;
            if(depth == 1) ProcessorParams.ChannelSelector = ChannelSelector.A;
            Parallel.For(rangeX, width - rangeX, po, i =>
            {
                if (buckets.Value is null) buckets.Value = new ushort[256, depth];
                for (var j = 0; j < 256; j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        buckets.Value[j, k] = 0;
                    }
                }

                var sum = 0;
                for (var l = -rangeX; l <= rangeX; l++)
                {
                    for (var j = 0; j < rangeY; j++)
                    {
                        for (var c = 0; c < depth; c++)
                        {
                            if (!ProcessorParams.ChannelSelector.Used(c)) continue;
                            buckets.Value[pixels[i + l, j, c], c]++;
                            sum++;
                        }
                    }
                }
                for (var l = -rangeX; l <= rangeX; l++)
                {
                    for (var j = 0; j <= rangeY; j++)
                    {
                        for (var c = 0; c < depth; c++)
                        {
                            if (!ProcessorParams.ChannelSelector.Used(c)) continue;
                            buckets.Value[pixels[i + l, j + rangeY, c], c]++;
                            sum++;
                        }
                    }
                }

                for (byte c = 0; c < depth; c++)
                {
                    if (!ProcessorParams.ChannelSelector.Used(c)) continue;
                    tempArr[i, rangeY, c] = ProcessorParams.CalculateOneFunc(buckets.Value, c);
                }
                for (var j = rangeY + 1; j < height - rangeY; j++)
                {
                    for (var l = -rangeX; l <= rangeX; l++)
                    {
                        for (var c = 0; c < depth; c++)
                        {
                            //TODO bench
                            if (!ProcessorParams.ChannelSelector.Used(c)) continue;
                            buckets.Value[pixels[i + l, j + rangeY, c], c]++;
                            buckets.Value[pixels[i + l, j - rangeY - 1, c], c]--;
                        }
                    }

                    for (byte c = 0; c < depth; c++)
                    {
                        if (!ProcessorParams.ChannelSelector.Used(c)) continue;
                        tempArr[i, j, c] = ProcessorParams.CalculateOneFunc(buckets.Value, c);
                    }
                }

            });
            return tempArr;
        }
    }
}