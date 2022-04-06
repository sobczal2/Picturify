using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Exceptions;
using Sobczal.Picturify.Core.Processing.Filters;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing
{
    /// <summary>
    /// Base for all processors in Picturify. Feel free to derive from this class and write a processor yourself!
    /// Processors may be reused, but don't run <see cref="Process"/> on the same object in parallel.
    /// </summary>
    /// <typeparam name="T">Type derived from ProcessorParams. Specifies data type passed to processor on init.</typeparam>
    /// <typeparam name="V"><see cref="FastImageB"/> or <see cref="FastImageF"/> depending on what type of <see cref="FastImage{T}"/> you want to use.</typeparam>
    public abstract class BaseProcessor<T, V> : IBaseProcessor where T : ProcessorParams where V : IFastImage
    {
        /// <summary>
        /// List of filters to be used on <see cref="Before"/> and <see cref="After"/>.
        /// More are added using <see cref="AddFilter"/>. This is cleared after calling <see cref="ClearFilters"/>.
        /// </summary>
        protected List<BaseFilter> _filters;
        
        /// <summary>
        /// Stores data passed to processor on init.
        /// </summary>
        protected T ProcessorParams;

        /// <summary>
        /// Basic constructor, does minor check for <see cref="ProcessorParams"/> and initializes processor.
        /// </summary>
        /// <param name="processorParams">Params of processing.</param>
        /// <exception cref="ArgumentException"></exception>
        public BaseProcessor(T processorParams)
        {
            if (processorParams.ChannelSelector is null)
                throw new ParamsArgumentException(nameof(processorParams.ChannelSelector), "can't be null");
            _filters = new List<BaseFilter>();
            ProcessorParams = processorParams;
        }
        
        /// <summary>
        /// This method gets called first(before <see cref="Process"/>) in <see cref="FastImageExtensions.ExecuteProcessor"/>
        /// and <see cref="FastImageExtensions.ExecuteProcessorAsync"/>. Conversion of <see cref="IFastImage"/> to
        /// correct version (<see cref="FastImageB"/> or <see cref="FastImageF"/>) specified by <see cref="V"/>, executing
        /// all of <see cref="BaseFilter.Before"/> added (in order of addition) is done here.
        /// Call it only from overrides of this method, preferably at the beginning.
        /// </summary>
        /// <param name="fastImage"><see cref="FastImage{T}"/> to work on.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> passed down to filters.</param>
        /// <returns><see cref="FastImage{T}"/> after operations listed above.</returns>
        /// <exception cref="NotSupportedException">Thrown when <see cref="V"/> is not <see cref="FastImageF"/> or <see cref="FastImageB"/></exception>
        public virtual IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var imageType = typeof(V);
            if (imageType == typeof(FastImageB))
                fastImage = fastImage.ToByteRepresentation();
            else if (imageType == typeof(FastImageF))
                fastImage = fastImage.ToFloatRepresentation();
            else throw new NotSupportedException($"Image of type {imageType} is not supported.");
            if (ProcessorParams.WorkingArea is null)
                ProcessorParams.WorkingArea =
                    new SquareAreaSelector(0, fastImage.PSize.Width, 0, fastImage.PSize.Height);
            else
            {
                ProcessorParams.WorkingArea.Validate(fastImage.PSize);
            }
            for (var i = 0; i < _filters.Count; i++)
            {
                fastImage = _filters[i].Before(fastImage, ProcessorParams, cancellationToken);
            }
            return fastImage;
        }

        /// <summary>
        /// This method gets called second (after <see cref="Before"/>) in <see cref="FastImageExtensions.ExecuteProcessor"/>
        /// and <see cref="FastImageExtensions.ExecuteProcessorAsync"/>. Override it to process image.
        /// Don't call this method yourself.
        /// </summary>
        /// <param name="fastImage"><see cref="FastImage{T}"/> to work on.</param>
        /// <param name="cancellationToken">Can be used in overrides to cancel processing.</param>
        /// <returns>Processed <see cref="FastImage{T}"/></returns>
        public virtual IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            return fastImage;
        }

        /// <summary>
        /// This method gets called last (after <see cref="Process"/>). It executes
        /// all of <see cref="BaseFilter.After"/> added (in reversed order of addition).
        /// Call it only from overrides of this method, preferably at the beginning.
        /// </summary>
        /// <param name="fastImage"><see cref="FastImage{T}"/> to work on.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> passed down to filters.</param>
        /// <returns></returns>
        public virtual IFastImage After(IFastImage fastImage, CancellationToken cancellationToken)
        {
            for (var i = _filters.Count - 1; i >= 0; i--)
            {
                fastImage = _filters[i].After(fastImage, ProcessorParams, cancellationToken);
            }
            return fastImage;
        }

        /// <summary>
        /// Adds a Filter to this processor. This base filter will be executed in <see cref="Before"/>
        /// and <see cref="After"/>.
        /// </summary>
        /// <param name="filter">Filter to add.</param>
        /// <returns>This <see cref="IBaseProcessor"/>.</returns>
        public IBaseProcessor AddFilter(BaseFilter filter)
        {
            _filters.Add(filter);
            return this;
        }

        /// <summary>
        /// Clears all filters added to this processor.
        /// </summary>
        /// <returns>This <see cref="IBaseProcessor"/></returns>
        public IBaseProcessor ClearFilters()
        {
            _filters.Clear();
            return this;
        }
    }
}