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
            if (processorParams.CalculateOneFunc is null)
                throw new ArgumentNullException(nameof(processorParams), "Processor param members can't be null.");
            AddFilter(EdgeBehaviourSelector.GetFilter(processorParams.EdgeBehaviourType, processorParams.Range));
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
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
            var rangeX = ProcessorParams.Range.Width;
            var rangeY = ProcessorParams.Range.Height;
            if(depth == 1) ProcessorParams.ChannelSelector = ChannelSelector.A;
            var area = ProcessorParams.WorkingArea;
            Parallel.For(area.LeftInclusive, area.RightExclusive, po, i =>
            {
                if (buckets.Value is null) buckets.Value = new ushort[256, depth];
                for (var j = 0; j < 256; j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        buckets.Value[j, k] = 0;
                    }
                }
                
                for (var l = -rangeX; l <= rangeX; l++)
                {
                    for (var j = area.BotInclusive - rangeY; j < area.BotInclusive; j++)
                    {
                        for (var c = 0; c < depth; c++)
                        {
                            if (!ProcessorParams.ChannelSelector.Used(c)) continue;
                            buckets.Value[pixels[i + l, j, c], c]++;
                        }
                    }
                }
                for (var l = -rangeX; l <= rangeX; l++)
                {
                    for (var j = area.BotInclusive - rangeY; j <= area.BotInclusive; j++)
                    {
                        for (var c = 0; c < depth; c++)
                        {
                            if (!ProcessorParams.ChannelSelector.Used(c)) continue;
                            buckets.Value[pixels[i + l, j + rangeY, c], c]++;
                        }
                    }
                }

                for (byte c = 0; c < depth; c++)
                {
                    if (!ProcessorParams.ChannelSelector.Used(c) || !ProcessorParams.WorkingArea.ShouldEdit(i, rangeY)) continue;
                    tempArr[i, area.BotInclusive, c] = ProcessorParams.CalculateOneFunc(buckets.Value, c);
                }
                for (var j = area.BotInclusive + 1; j < area.TopExclusive; j++)
                {
                    for (var l = -rangeX; l <= rangeX; l++)
                    {
                        for (var c = 0; c < depth; c++)
                        {
                            if (!ProcessorParams.ChannelSelector.Used(c)) continue;
                            buckets.Value[pixels[i + l, j + rangeY, c], c]++;
                            buckets.Value[pixels[i + l, j - rangeY - 1, c], c]--;
                        }
                    }

                    for (byte c = 0; c < depth; c++)
                    {
                        if (!ProcessorParams.ChannelSelector.Used(c) || !ProcessorParams.WorkingArea.ShouldEdit(i, j)) continue;
                        tempArr[i, j, c] = ProcessorParams.CalculateOneFunc(buckets.Value, c);
                    }
                }

            });
            return tempArr;
        }
    }
}