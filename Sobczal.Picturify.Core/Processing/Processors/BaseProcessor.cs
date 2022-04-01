using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Filters;

namespace Sobczal.Picturify.Core.Processing
{
    public abstract class BaseProcessor<T> : IBaseProcessor where T : IProcessorParams
    {
        protected List<BaseFilter> _filters;
        protected T ProcessorParams;

        public BaseProcessor(T processorParams)
        {
            _filters = new List<BaseFilter>();
            ProcessorParams = processorParams;
        }
        public virtual async Task<IFastImage> Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            foreach (var filter in _filters)
            {
                fastImage = await filter.Before(fastImage, ProcessorParams, cancellationToken);
            }
            PicturifyConfig.LogInfo($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} executed.");
            return fastImage;
        }

        public virtual Task<IFastImage> Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            PicturifyConfig.LogInfo($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} executed.");
            return Task.FromResult(fastImage);
        }

        public virtual async Task<IFastImage> After(IFastImage fastImage, CancellationToken cancellationToken)
        {
            foreach (var filter in _filters)
            {
                fastImage = await filter.After(fastImage, ProcessorParams, cancellationToken);
            }
            PicturifyConfig.LogInfo($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} executed.");
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