using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing
{
    /// <summary>
    /// Base class for all processor params.
    /// </summary>
    public abstract class ProcessorParams
    {
        /// <summary>
        /// Starting working area for editing <see cref="FastImage{T}"/>.
        /// When null is passed, it is automatically set to edit the whole <see cref="FastImage{T}"/>.
        /// If passing null throws an error report this issue.
        /// </summary>
        public IAreaSelector WorkingArea { get; set; }
        
        /// <summary>
        /// Defines on what channels processor is going to work on.
        /// Most of the time defaults to RGB.
        /// </summary>
        public ChannelSelector ChannelSelector { get; set; }

        public ProcessorParams(IAreaSelector workingArea, ChannelSelector channelSelector)
        {
            WorkingArea = workingArea;
            ChannelSelector = channelSelector;
        }
    }
}