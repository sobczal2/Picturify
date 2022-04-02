using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Testing
{
    public class EmptyProcessor : BaseProcessor<EmptyProcessorParams, FastImageB>
    {
        public EmptyProcessor(EmptyProcessorParams processorParams) : base(processorParams)
        {
        }
    }
}