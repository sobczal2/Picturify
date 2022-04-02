using System;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class RollingBucketParams : IProcessorParams
    {
        public PSize PSize { get; set; } = new PSize {Width = 7, Height = 7};
        /// <summary>
        /// ushort[] table of neighboring numbers of pixels of size 256, channel
        /// </summary>
        public Func<ushort[,], byte, byte> CalculateOneFunc { get; set; }
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
        public ChannelSelector ChannelSelector { get; set; }

    }
}