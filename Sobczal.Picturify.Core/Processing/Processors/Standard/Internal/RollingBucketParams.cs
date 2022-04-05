using System;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class RollingBucketParams : ProcessorParams
    {
        public PSize Range { get; set; }
        /// <summary>
        /// ushort[] table of neighboring numbers of pixels of size 256, channel
        /// </summary>
        public Func<ushort[,], byte, byte> CalculateOneFunc { get; set; }
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }

        public RollingBucketParams(ChannelSelector channelSelector, PSize range,
            Func<ushort[,], byte, byte> calculateOneFunc, EdgeBehaviourSelector.Type edgeBehaviourType,
            IAreaSelector workingArea) : base(workingArea, channelSelector)
        {
            CalculateOneFunc = calculateOneFunc;
            EdgeBehaviourType = edgeBehaviourType;
            ChannelSelector = channelSelector;
            Range = range;
        }
    }
}