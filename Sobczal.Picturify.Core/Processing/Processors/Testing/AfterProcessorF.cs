using System.Threading;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Testing
{
    public class AfterProcessorF : BaseProcessor<EmptyProcessorParams, FastImageF>
    {
        public AfterProcessorF(EmptyProcessorParams processorParams) : base(processorParams)
        {
        }

        public override IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            return fastImage;
        }
    }
}