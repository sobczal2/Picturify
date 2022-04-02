using System.Drawing;
using System.Reflection;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Data
{
    public static class FastImageFactory
    {
        public enum Version
        {
            Float,
            Byte
        }
        
        public static IFastImage Empty(PSize pSize, Version version = Version.Float)
        {
            PicturifyConfig.LogInfo($"FastImage{MethodBase.GetCurrentMethod().Name}");
            switch (version)
            {
                case Version.Byte:
                    return new FastImageB(pSize);
                default:
                    return new FastImageF(pSize);
            }
        }
        
        public static IFastImage FromImage(Image image, Version version = Version.Float)
        {
            PicturifyConfig.LogInfo($"FastImage{MethodBase.GetCurrentMethod().Name}");
            switch (version)
            {
                case Version.Byte:
                    return new FastImageB(image);
                default:
                    return new FastImageF(image);
            }
        }
        
        public static IFastImage FromFile(string path, Version version = Version.Float)
        {
            PicturifyConfig.LogInfo($"FastImage{MethodBase.GetCurrentMethod().Name}");
            switch (version)
            {
                case Version.Byte:
                    return new FastImageB(path);
                default:
                    return new FastImageF(path);
            }
        }
    }
}