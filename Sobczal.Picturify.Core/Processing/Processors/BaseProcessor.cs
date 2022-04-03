using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Filters;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing
{
    public abstract class BaseProcessor<T, V> : IBaseProcessor where T : ProcessorParams where V : IFastImage
    {
        protected List<BaseFilter> _filters;
        protected T ProcessorParams;

        public BaseProcessor(T processorParams)
        {
            _filters = new List<BaseFilter>();
            ProcessorParams = processorParams;
        }
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
                    new SquareAreaSelector(0, 0, fastImage.PSize.Width, fastImage.PSize.Height);
            for (var i = 0; i < _filters.Count; i++)
            {
                fastImage = _filters[i].Before(fastImage, ProcessorParams, cancellationToken);
            }
            return fastImage;
        }

        public virtual IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            return fastImage;
        }

        public virtual IFastImage After(IFastImage fastImage, CancellationToken cancellationToken)
        {
            for (var i = _filters.Count - 1; i >= 0; i--)
            {
                fastImage = _filters[i].After(fastImage, ProcessorParams, cancellationToken);
            }
            return fastImage;
        }

        public IBaseProcessor AddFilter(BaseFilter filter)
        {
            _filters.Add(filter);
            return this;
        }

        public IBaseProcessor ClearFilters()
        {
            _filters.Clear();
            return this;
        }
    }
}