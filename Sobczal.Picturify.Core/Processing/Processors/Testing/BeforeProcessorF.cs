using System.Threading;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Testing
{
    public class BeforeProcessorF : BaseProcessor<EmptyProcessorParams, FastImageF>
    {
        public BeforeProcessorF(EmptyProcessorParams processorParams) : base(processorParams)
        {
        }

        public override IFastImage After(IFastImage fastImage, CancellationToken cancellationToken)
        {
            return fastImage;
        }
    }
}