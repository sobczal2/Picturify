using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Filters.EdgeBehaviour
{
    public class ExtendEdgeFilter : BaseFilter
    {
        private readonly PSize _kernelPSize;

        public ExtendEdgeFilter(PSize kernelPSize)
        {
            if (kernelPSize.Width % 2 != 1 || kernelPSize.Height % 2 != 1)
                throw new ArgumentException("Kernel size must be 2n+1x2n+1.");
            _kernelPSize = kernelPSize;
        }
        public override async Task<IFastImage> Before(IFastImage fastImage, IProcessorParams processorParams, CancellationToken cancellationToken)
        {
            switch (fastImage)
            {
                case FastImageB fastImageB:
                    fastImage = await fastImageB.ProcessAsync(BeforeProcessingFunctionB, cancellationToken);
                    break;
                case FastImageF fastImageF:
                    fastImage = await fastImageF.ProcessAsync(BeforeProcessingFunctionF, cancellationToken);
                    break;
            }
            return await base.Before(fastImage, processorParams, cancellationToken);
        }

        private float[,,] BeforeProcessingFunctionF(float[,,] pixels, CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();
            var rangeX = _kernelPSize.Width / 2;
            var rangeY = _kernelPSize.Height / 2;
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
                        arr[i, j, k] = arr[rangeX, rangeY, k];
                        arr[i, heightArr - j - 1, k] = arr[rangeX, heightArr - 1 - rangeY, k];
                        arr[widthArr - i - 1, j, k] = arr[widthArr - 1 - rangeY, rangeY, k];
                        arr[widthArr - i - 1, heightArr - j - 1, k] = arr[widthArr - 1 - rangeY, heightArr - 1 - rangeY, k];
                    }
                }
            }


            //horizontal edges
            for (var i = rangeX; i < widthArr - 1 - rangeX; i++)
            {
                for (var j = 0; j < rangeY; j++)
                {
                    for (var k = 0; k < depthArr; k++)
                    {
                        arr[i, j, k] = arr[i, rangeY, k];
                        arr[i, heightArr - 1 - j, k] = arr[i, heightArr - 1 - rangeY, k];
                    }
                }
            }

            //vertical edges
            for (var i = 0; i < rangeX; i++)
            {
                for (var j = rangeY; j < heightArr - 1 - rangeY; j++)
                {
                    for (var k = 0; k < depthArr; k++)
                    {
                        arr[i, j, k] = arr[rangeX, j, k];
                        arr[widthArr - i - 1, j, k] = arr[widthArr - rangeX - 1, j, k];
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
            var rangeX = _kernelPSize.Width / 2;
            var rangeY = _kernelPSize.Height / 2;
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
                        arr[i, j, k] = arr[rangeX, rangeY, k];
                        arr[i, heightArr - j - 1, k] = arr[rangeX, heightArr - 1 - rangeY, k];
                        arr[widthArr - i - 1, j, k] = arr[widthArr - 1 - rangeY, rangeY, k];
                        arr[widthArr - i - 1, heightArr - j - 1, k] = arr[widthArr - 1 - rangeY, heightArr - 1 - rangeY, k];
                    }
                }
            }


            //horizontal edges
            for (var i = rangeX; i < widthArr - 1 - rangeX; i++)
            {
                for (var j = 0; j < rangeY; j++)
                {
                    for (var k = 0; k < depthArr; k++)
                    {
                        arr[i, j, k] = arr[i, rangeY, k];
                        arr[i, heightArr - 1 - j, k] = arr[i, heightArr - 1 - rangeY, k];
                    }
                }
            }

            //vertical edges
            for (var i = 0; i < rangeX; i++)
            {
                for (var j = rangeY; j < heightArr - 1 - rangeY; j++)
                {
                    for (var k = 0; k < depthArr; k++)
                    {
                        arr[i, j, k] = arr[rangeX, j, k];
                        arr[widthArr - i - 1, j, k] = arr[widthArr - rangeX - 1, j, k];
                    }
                }
            }
            sw.Stop();
            PicturifyConfig.LogTimeDebug($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", sw.ElapsedMilliseconds);
            return arr;
        }

        public override async Task<IFastImage> After(IFastImage fastImage, IProcessorParams processorParams, CancellationToken cancellationToken)
        {
            var areaSelector = new SquareAreaSelector(_kernelPSize.Width / 2, _kernelPSize.Height / 2,
                fastImage.PSize.Width - _kernelPSize.Width / 2, fastImage.PSize.Height - _kernelPSize.Height / 2);
            fastImage.Crop(areaSelector);
            return await base.After(fastImage, processorParams, cancellationToken);
        }
    }
}