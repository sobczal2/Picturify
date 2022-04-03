using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Testing
{
    public class EmptyProcessorF : BaseProcessor<EmptyProcessorParams, FastImageF>
    {
        public EmptyProcessorF(EmptyProcessorParams processorParams) : base(processorParams)
        {
        }
    }
}