using System.Threading;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Testing
{
    public class BeforeProcessorB : BaseProcessor<EmptyProcessorParams, FastImageB>
    {
        public BeforeProcessorB(EmptyProcessorParams processorParams) : base(processorParams)
        {
        }

        public override IFastImage After(IFastImage fastImage, CancellationToken cancellationToken)
        {
            return fastImage;
        }
    }
}