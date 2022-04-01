using System.Reflection;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Filters
{
    public class BaseFilter
    {
        public virtual FastImage Before(FastImage fastImage)
        {
            PicturifyConfig.LogInfo($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} executed.");
            return fastImage;
        }

        public virtual FastImage After(FastImage fastImage)
        {
            PicturifyConfig.LogInfo($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} executed.");
            return fastImage;
        }
    }
}