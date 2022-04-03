using System.Threading;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Testing
{
    public class AfterProcessorB : BaseProcessor<EmptyProcessorParams, FastImageB>
    {
        public AfterProcessorB(EmptyProcessorParams processorParams) : base(processorParams)
        {
        }

        public override IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            return fastImage;
        }
    }
}