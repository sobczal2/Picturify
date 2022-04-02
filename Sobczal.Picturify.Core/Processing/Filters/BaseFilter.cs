﻿using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Filters
{
    public class BaseFilter
    {
        public virtual Task<IFastImage> Before(IFastImage fastImage, ProcessorParams processorParams, CancellationToken cancellationToken)
        {
            return Task.FromResult(fastImage);
        }

        public virtual Task<IFastImage> After(IFastImage fastImage, ProcessorParams processorParams, CancellationToken cancellationToken)
        {
            return Task.FromResult(fastImage);
        }
    }
}