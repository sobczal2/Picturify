using Sobczal.Picturify.Core.Processing.Exceptions;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    /// <summary>
    /// Params class for <see cref="MinBlurProcessor"/>.
    /// </summary>
    public class MinBlurParams : ProcessorParams
    {
        /// <summary>
        /// Range of analyzed pixels. Kernel size is set as [2 * <see cref="Range"/> + 1, 2 * <see cref="Range"/> + 1]
        /// <example>
        /// Width=2, Height=2 means that 5x5 area around currently changed pixel will be analyzed.
        /// </example>
        /// </summary>
        public PSize Range { get; set; }
        
        /// <summary>
        /// This filter uses kernel so you need to define behaviour on edges.
        /// <see cref="EdgeBehaviourSelector.Type.Extend"/> by default.
        /// </summary>
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
        /// <summary>
        /// Constructor initializing params object.
        /// </summary>
        /// <param name="channelSelector">Defines what channels should processor work on.</param>
        /// <param name="range">Defines range of internal kernel <see cref="Range"/>.</param>
        /// <param name="edgeBehaviourType">Defines behaviour on edges (necessary when internal kernel is bigger than 1x1). See <see cref="EdgeBehaviourType"/>.</param>
        /// <param name="workingArea">Defines starting working area(this may be changed internally by filters or processor itself).</param>
        /// <exception cref="ParamsArgumentException">Thrown when range is &lt; 0 on any dimension.</exception>
        public MinBlurParams(ChannelSelector channelSelector, PSize range, EdgeBehaviourSelector.Type edgeBehaviourType = EdgeBehaviourSelector.Type.Extend, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            if (range.Width <= 0 || range.Height <= 0)
                throw new ParamsArgumentException(nameof(range), "can't be negative or 0");
            Range = range;
            EdgeBehaviourType = edgeBehaviourType;
        }
    }
}