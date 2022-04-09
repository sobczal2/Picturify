using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Data
{
    /// <summary>
    /// Static class to create <see cref="IFastImage"/> objects (<see cref="FastImageB"/> and <see cref="FastImageF"/>).
    /// </summary>
    public static class FastImageFactory
    {
        /// <summary>
        /// Used to define whether <see cref="FastImageB"/> or <see cref="FastImageF"/> should be initialized.
        /// </summary>
        public enum Version
        {
            Float,
            Byte
        }
        
        /// <summary>
        /// Creates empty <see cref="IFastImage"/> object (either <see cref="FastImageB"/> or <see cref="FastImageF"/>
        /// depending on <see cref="version"/>). Defaults to <see cref="FastImageF"/>.
        /// </summary>
        /// <param name="pSize">Size of <see cref="IFastImage"/> to create.</param>
        /// <param name="version">Defines if <see cref="FastImageB"/> or <see cref="FastImageF"/> is created.</param>
        /// <returns>Created <see cref="IFastImage"/>.</returns>
        public static IFastImage Empty(PSize pSize, Version version = Version.Float)
        {
            PicturifyConfig.LogInfo($"FastImage.{MethodBase.GetCurrentMethod().Name}");
            switch (version)
            {
                case Version.Byte:
                    return new FastImageB(pSize);
                default:
                    return new FastImageF(pSize);
            }
        }
        
        /// <summary>
        /// Creates <see cref="IFastImage"/> object (either <see cref="FastImageB"/> or <see cref="FastImageF"/>
        /// depending on <see cref="version"/>) from <see cref="Image"/>. Defaults to <see cref="FastImageF"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to create from.</param>
        /// <param name="version">Defines if <see cref="FastImageB"/> or <see cref="FastImageF"/> is created.</param>
        /// <returns>Created <see cref="IFastImage"/>.</returns>
        public static IFastImage FromImage(Image image, Version version = Version.Float)
        {
            PicturifyConfig.LogInfo($"FastImage.{MethodBase.GetCurrentMethod().Name}");
            switch (version)
            {
                case Version.Byte:
                    return new FastImageB(image);
                default:
                    return new FastImageF(image);
            }
        }
        
        /// <summary>
        /// Creates <see cref="IFastImage"/> object (either <see cref="FastImageB"/> or <see cref="FastImageF"/>
        /// depending on <see cref="version"/>) from file with <see cref="path"/>. Defaults to <see cref="FastImageF"/>.
        /// </summary>
        /// <param name="path">Path to file to create from.</param>
        /// <param name="version">Defines if <see cref="FastImageB"/> or <see cref="FastImageF"/> is created.</param>
        /// <returns>Created <see cref="IFastImage"/>.</returns>
        public static IFastImage FromFile(string path, Version version = Version.Float)
        {
            PicturifyConfig.LogInfo($"FastImage.{MethodBase.GetCurrentMethod().Name}");
            switch (version)
            {
                case Version.Byte:
                    return new FastImageB(path);
                default:
                    return new FastImageF(path);
            }
        }

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IFastImage Random(PSize size, Random rand)
        {
            PicturifyConfig.LogInfo($"FastImage.{MethodBase.GetCurrentMethod().Name}");
            IFastImage fastImage = new FastImageF(size);
            var fieldInfo = typeof(FastImageF).GetField("Pixels", BindingFlags.NonPublic | BindingFlags.Instance);
            var actualArray = (float[,,]) fieldInfo.GetValue(fastImage);
            for (var i = 0; i < actualArray.GetLength(0); i++)
            {
                for (var j = 0; j < actualArray.GetLength(1); j++)
                {
                    for (var k = 0; k < actualArray.GetLength(2); k++)
                    {
                        actualArray[i, j, k] = (float) rand.NextDouble();
                    }
                }
            }

            return fastImage;
        }
        
        public static IFastImage FromStream(Stream stream, Version version = Version.Float)
        {
            PicturifyConfig.LogInfo($"FastImage.{MethodBase.GetCurrentMethod().Name}");
            switch (version)
            {
                case Version.Byte:
                    return new FastImageB(stream);
                default:
                    return new FastImageF(stream);
            }
        }
    }
}