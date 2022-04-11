using System;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Standard;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    /// <summary>
    /// <see cref="MaxBlurProcessor"/> implements maximum filter on <see cref="FastImageB"/>.
    /// This implementation operates on <see cref="FastImageB"/> instead of <see cref="FastImageF"/>,
    /// because it uses <see cref="RollingBucketProcessor"/> which can't operate on <see cref="FastImageF"/> internally which is
    /// much more performant than naive implementation. If you need floating point precision consider writing processor yourself ;).
    /// </summary>
    public class MaxBlurProcessor : BaseProcessor<MaxBlurParams, FastImageB>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="processorParams">params to get used in this processor. Don't use same object of <see cref="MaxBlurParams"/>
        /// on two different 
        /// </param>
        public MaxBlurProcessor(MaxBlurParams processorParams) : base(processorParams)
        {
        }
        
        /// <summary>
        /// Processing is delegated to <see cref="RollingBucketProcessor"/> with function to calculate maximum.
        /// </summary>
        /// <param name="fastImage"><see cref="IFastImage"/> to work on.</param>
        /// <param name="cancellationToken">Used to cancel processing.</param>
        /// <returns>Processed <see cref="IFastImage"/></returns>
        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var rbp = new RollingBucketProcessor(new RollingBucketParams(ProcessorParams.ChannelSelector,
                ProcessorParams.Range, ProcessCalculateOne, ProcessorParams.EdgeBehaviourType,
                ProcessorParams.WorkingArea));
            fastImage.ExecuteProcessor(rbp);
            return base.Process(fastImage, cancellationToken);
        }

        /// <summary>
        /// Simple method to find maximum in buckets returned from <see cref="RollingBucketProcessor"/>
        /// </summary>
        /// <param name="buckets">Buckets with numbers of occurrences of pixel with color of that index.</param>
        /// <param name="c">Current channel. A - 0, R - 1, G - 2, B - 3</param>
        /// <returns>Index of last not empty bucket.</returns>
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