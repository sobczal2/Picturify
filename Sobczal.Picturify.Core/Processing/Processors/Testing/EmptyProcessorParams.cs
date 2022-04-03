using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Testing
{
    public class EmptyProcessorParams : ProcessorParams
    {
        public EmptyProcessorParams() : base(null, ChannelSelector.RGB)
        {
        }
    }
}