﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Standard;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    public class MaxProcessor : BaseProcessor<MaxParams, FastImageB>
    {
        public MaxProcessor(MaxParams processorParams) : base(processorParams)
        {
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var rbp = new RollingBucketProcessor(new RollingBucketParams
            {
                CalculateOneFunc = ProcessCalculateOne, ChannelSelector = ProcessorParams.ChannelSelector,
                PSize = ProcessorParams.PSize, EdgeBehaviourType = ProcessorParams.EdgeBehaviourType,
                WorkingArea = ProcessorParams.WorkingArea
            });
            fastImage.ExecuteProcessor(rbp);
            return base.Process(fastImage, cancellationToken);
        }

        private byte ProcessCalculateOne(ushort[,] buckets, byte c)
        {
            byte i = 255;
            while (true)
            {
                if (buckets[i, c] != 0) return i;
                i--;
            }
        }
    }
}