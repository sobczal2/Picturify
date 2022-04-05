using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Filters.EdgeBehaviour
{
    public class ConstantEdgeFilter : BaseFilter
    {
        private readonly PSize _range;
        private readonly PixelColor _pixelColor;

        public ConstantEdgeFilter(PSize range, PixelColor pixelColor)
        {
            if (pixelColor is null) pixelColor = new PixelColor(0, 0, 0, 0);
            _range = range;
            _pixelColor = pixelColor;
        }
        public override IFastImage Before(IFastImage fastImage, ProcessorParams processorParams, CancellationToken cancellationToken)
        {
            switch (fastImage)
            {
                case FastImageB fastImageB:
                    fastImage = fastImageB.Process(BeforeProcessingFunctionB, cancellationToken);
                    break;
                case FastImageF fastImageF:
                    fastImage = fastImageF.Process(BeforeProcessingFunctionF, cancellationToken);
                    break;
            }
            processorParams.WorkingArea.Resize(_range.Width, _range.Height);
            return base.Before(fastImage, processorParams, cancellationToken);
        }

        private float[,,] BeforeProcessingFunctionF(float[,,] pixels, CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();
            var rangeX = _range.Width;
            var rangeY = _range.Height;
            var arr = new float[pixels.GetLength(0) + 2 * rangeX, pixels.GetLength(1) + 2 * rangeY,
                pixels.GetLength(2)];
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            var widthArr = arr.GetLength(0);
            var heightArr = arr.GetLength(1);
            var depthArr = arr.GetLength(2);
            var widthPx = pixels.GetLength(0);
            var heightPx = pixels.GetLength(1);
            var depthPx = pixels.GetLength(2);
            //image
            Parallel.For(0, widthPx, po, i =>
            {
                for (var j = 0; j < heightPx; j++)
                {
                    for (var k = 0; k < depthPx; k++)
                    {
                        arr[i + rangeX, j + rangeY, k] = pixels[i, j, k];
                    }
                }
            });

            //corners
            for (var i = 0; i < rangeX; i++)
            {
                for (var j = 0; j < rangeY; j++)
                {
                    for (var k = 0; k < depthArr; k++)
                    {
                        arr[i, j, k] = _pixelColor.GetChannelF(k);
                        arr[i, heightArr - j - 1, k] = _pixelColor.GetChannelF(k);
                        arr[widthArr - i - 1, j, k] = _pixelColor.GetChannelF(k);
                        arr[widthArr - i - 1, heightArr - j - 1, k] = _pixelColor.GetChannelF(k);
                    }
                }
            }


            //horizontal edges
            for (var i = rangeX; i < widthArr - rangeX; i++)
            {
                for (var j = 0; j < rangeY; j++)
                {
                    for (var k = 0; k < depthArr; k++)
                    {
                        arr[i, j, k] = _pixelColor.GetChannelF(k);
                        arr[i, heightArr - 1 - j, k] = _pixelColor.GetChannelF(k);
                    }
                }
            }

            //vertical edges
            for (var i = 0; i < rangeX; i++)
            {
                for (var j = rangeY; j < heightArr - rangeY; j++)
                {
                    for (var k = 0; k < depthArr; k++)
                    {
                        arr[i, j, k] = _pixelColor.GetChannelF(k);
                        arr[widthArr - i - 1, j, k] = _pixelColor.GetChannelF(k);
                    }
                }
            }
            sw.Stop();
            PicturifyConfig.LogTimeDebug($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", sw.ElapsedMilliseconds);
            return arr;
        }
        
        private byte[,,] BeforeProcessingFunctionB(byte[,,] pixels, CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();
            var rangeX = _range.Width;
            var rangeY = _range.Height;
            var arr = new byte[pixels.GetLength(0) + 2 * rangeX, pixels.GetLength(1) + 2 * rangeY,
                pixels.GetLength(2)];
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            var widthArr = arr.GetLength(0);
            var heightArr = arr.GetLength(1);
            var depthArr = arr.GetLength(2);
            var widthPx = pixels.GetLength(0);
            var heightPx = pixels.GetLength(1);
            var depthPx = pixels.GetLength(2);
            //image
            Parallel.For(0, widthPx, po, i =>
            {
                for (var j = 0; j < heightPx; j++)
                {
                    for (var k = 0; k < depthPx; k++)
                    {
                        arr[i + rangeX, j + rangeY, k] = pixels[i, j, k];
                    }
                }
            });

            //corners
            for (var i = 0; i < rangeX; i++)
            {
                for (var j = 0; j < rangeY; j++)
                {
                    for (var k = 0; k < depthArr; k++)
                    {
                        arr[i, j, k] = _pixelColor.GetChannelB(k);
                        arr[i, heightArr - 1 - j, k] = _pixelColor.GetChannelB(k);
                        arr[widthArr - i - 1, j, k] = _pixelColor.GetChannelB(k);
                        arr[widthArr - i - 1, heightArr - j - 1, k] = _pixelColor.GetChannelB(k);
                    }
                }
            }


            //horizontal edges
            for (var i = rangeX; i < widthArr - rangeX; i++)
            {
                for (var j = 0; j < rangeY; j++)
                {
                    for (var k = 0; k < depthArr; k++)
                    {
                        arr[i, j, k] = _pixelColor.GetChannelB(k);
                        arr[i, heightArr - 1 - j, k] = _pixelColor.GetChannelB(k);
                    }
                }
            }

            //vertical edges
            for (var i = 0; i < rangeX; i++)
            {
                for (var j = rangeY; j < heightArr - rangeY; j++)
                {
                    for (var k = 0; k < depthArr; k++)
                    {
                        arr[i, j, k] = _pixelColor.GetChannelB(k);
                        arr[widthArr - i - 1, j, k] = _pixelColor.GetChannelB(k);
                    }
                }
            }
            sw.Stop();
            PicturifyConfig.LogTimeDebug($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", sw.ElapsedMilliseconds);
            return arr;
        }

        public override IFastImage After(IFastImage fastImage, ProcessorParams processorParams, CancellationToken cancellationToken)
        {
            processorParams.WorkingArea.Resize(-_range.Width, -_range.Height);
            fastImage.Crop(new SquareAreaSelector(_range.Width, fastImage.PSize.Width - _range.Width, _range.Height,
                fastImage.PSize.Height - _range.Height));
            return base.After(fastImage, processorParams, cancellationToken);
        }
    }
}