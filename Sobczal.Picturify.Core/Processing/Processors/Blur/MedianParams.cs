using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    public class MedianParams : ProcessorParams
    {
        public PSize PSize { get; set; } = new PSize (7, 7);
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
    }
}