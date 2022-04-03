using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Testing
{
    public class EmptyProcessorB : BaseProcessor<EmptyProcessorParams, FastImageB>
    {
        public EmptyProcessorB(EmptyProcessorParams processorParams) : base(processorParams)
        {
        }
    }
}