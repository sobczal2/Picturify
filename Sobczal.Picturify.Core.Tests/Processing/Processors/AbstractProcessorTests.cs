using System;
using System.Collections.Generic;
using System.Reflection;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing;
using Sobczal.Picturify.Core.Utils;
using Xunit;
using Xunit.Sdk;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors
{
    /// <summary>
    /// Create class which derives from this to run some basic tests on concrete processor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Collection("ProcessorTests")]
    public abstract class AbstractProcessorTests<T> where T : IBaseProcessor
    {
        public List<T> SizeCheckProcessors { get; set; }
        public T WorkingAreaCheckProcessor { get; set; }
        public T ChannelSelectorCheckProcessor { get; set; }

        public bool UseExactNumersInPixels { get; set; }

        public AbstractProcessorTests()
        {
            SizeCheckProcessors = new List<T>();
            UseExactNumersInPixels = true;
            PopulateSizeCheckProcessors();
            PopulateChannelSelectorCheckProcessors();
            PopulateWorkingAreaCheckProcessors();
        }

        protected abstract void PopulateWorkingAreaCheckProcessors();

        protected abstract void PopulateChannelSelectorCheckProcessors();

        protected abstract void PopulateSizeCheckProcessors();

        [Theory]
        [InlineData(10, 10)]
        [InlineData(50, 50)]
        [InlineData(100, 100)]
        public void ExecuteProcessor_ShouldNotChangeImageSizeArgb(int imgWidth, int imgHeight)
        {
            var imgSize = new PSize(imgWidth, imgHeight);
            foreach (var processor in SizeCheckProcessors)
            {
                var fastImage = FastImageFactory.Empty(imgSize);
                fastImage.ExecuteProcessor(processor);
                
                var expected = imgSize;
                var actual = fastImage.PSize;
                
                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(50, 50)]
        [InlineData(100, 100)]
        public void ExecuteProcessor_ShouldNotChangeImageSizeGrayscale(int imgWidth, int imgHeight)
        {
            var imgSize = new PSize(imgWidth, imgHeight);
            foreach (var processor in SizeCheckProcessors)
            {
                var fastImage = FastImageFactory.Empty(imgSize).ToGrayscale();
                fastImage.ExecuteProcessor(processor);
                
                var expected = imgSize;
                var actual = fastImage.PSize;
                
                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ExecuteProcessor_ShouldOnlyUseChannelOnArgbImage(int channel)
        {
            var fastImage = FastImageFactory.Random(new PSize(10, 10), new Random(1));
            var fastImageChanged = fastImage.GetCopy();
            
            var paramsFieldInfo = typeof(T).GetField("ProcessorParams", BindingFlags.NonPublic | BindingFlags.Instance);
            var procesorParams = (ProcessorParams) paramsFieldInfo.GetValue(ChannelSelectorCheckProcessor);
            switch (channel)
            {
                case 0:
                    procesorParams.ChannelSelector = ChannelSelector.A;
                    break;
                case 1:
                    procesorParams.ChannelSelector = ChannelSelector.R;
                    break;
                case 2:
                    procesorParams.ChannelSelector = ChannelSelector.G;
                    break;
                case 3:
                    procesorParams.ChannelSelector = ChannelSelector.B;
                    break;
                default:
                    throw new TestClassException("invalid channel");
            }
            
            fastImageChanged = fastImageChanged.ExecuteProcessor(ChannelSelectorCheckProcessor);
            fastImageChanged = fastImageChanged.ToFloatRepresentation();
            
            var imageFieldInfo = typeof(FastImageF).GetField("Pixels", BindingFlags.NonPublic | BindingFlags.Instance);
            var pixels = (float[,,]) imageFieldInfo.GetValue(fastImage);
            var pixelsChanged = (float[,,]) imageFieldInfo.GetValue(fastImageChanged);

            var original = new float[4][,];
            for (var i = 0; i < 4; i++)
            {
                original[i] = GetChannelFromArr(pixels, i);
            }
            var changed = new float[4][,];
            for (var i = 0; i < 4; i++)
            {
                changed[i] = GetChannelFromArr(pixelsChanged, i);
            }

            for (var i = 0; i < 4; i++)
            {
                if (i == channel)
                {
                    if(UseExactNumersInPixels)
                        Assert.NotEqual(original[i], changed[i]);
                    else
                    {
                        Assert.NotEqual(original[i], changed[i], new Float2DimArrComparer());
                    }
                }
                else
                {
                    if(UseExactNumersInPixels)
                        Assert.Equal(original[i], changed[i]);
                    else
                    {
                        Assert.Equal(original[i], changed[i], new Float2DimArrComparer());
                    }
                }
            }
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ExecuteProcessor_ShouldPerformOnGrayscaleImageNoMatterWhatChannelSelector(int channel)
        {
            var fastImage = FastImageFactory.Random(new PSize(10, 10), new Random(1)).ToGrayscale();
            var fastImageChanged = fastImage.GetCopy();
            
            var paramsFieldInfo = typeof(T).GetField("ProcessorParams", BindingFlags.NonPublic | BindingFlags.Instance);
            var procesorParams = (ProcessorParams) paramsFieldInfo.GetValue(ChannelSelectorCheckProcessor);
            switch (channel)
            {
                case 0:
                    procesorParams.ChannelSelector = ChannelSelector.A;
                    break;
                case 1:
                    procesorParams.ChannelSelector = ChannelSelector.R;
                    break;
                case 2:
                    procesorParams.ChannelSelector = ChannelSelector.G;
                    break;
                case 3:
                    procesorParams.ChannelSelector = ChannelSelector.B;
                    break;
                default:
                    throw new TestClassException("invalid channel");
            }

            fastImageChanged = fastImageChanged.ExecuteProcessor(ChannelSelectorCheckProcessor);
            fastImageChanged = fastImageChanged.ToFloatRepresentation();
            
            var imageFieldInfo = typeof(FastImageF).GetField("Pixels", BindingFlags.NonPublic | BindingFlags.Instance);
            var pixels = (float[,,]) imageFieldInfo.GetValue(fastImage);
            var pixelsChanged = (float[,,]) imageFieldInfo.GetValue(fastImageChanged);
            
            var original = GetChannelFromArr(pixels, 0);
            var changed = GetChannelFromArr(pixelsChanged, 0);

            if (UseExactNumersInPixels)
            {
                Assert.NotEqual(original, changed);
            }
            else
            {
                Assert.NotEqual(original, changed, new Float2DimArrComparer());
            }
        }

        [Fact]
        public void ShouldExecuteProcessor_OnlyOnPixelsSpecifiedByAreaSelectorOnArgbImage()
        {
            var fastImage = FastImageFactory.Random(new PSize(5, 5), new Random(1));
            var areaSelector = new TestAreaSelector();
            var fastImageChanged = fastImage.GetCopy();
            
            var paramsFieldInfo = typeof(T).GetField("ProcessorParams", BindingFlags.NonPublic | BindingFlags.Instance);
            var procesorParams = (ProcessorParams) paramsFieldInfo.GetValue(ChannelSelectorCheckProcessor);
            procesorParams.WorkingArea = areaSelector;
            
            fastImageChanged = fastImageChanged.ExecuteProcessor(ChannelSelectorCheckProcessor);
            fastImageChanged = fastImageChanged.ToFloatRepresentation();
            
            var imageFieldInfo = typeof(FastImageF).GetField("Pixels", BindingFlags.NonPublic | BindingFlags.Instance);
            var pixels = (float[,,]) imageFieldInfo.GetValue(fastImage);
            var pixelsChanged = (float[,,]) imageFieldInfo.GetValue(fastImageChanged);
            for (var i = 0; i < pixels.GetLength(0); i++)
            {
                for (var j = 0; j < pixels.GetLength(1); j++)
                {
                    if (i == 0 && j == 0)
                    {
                        if (UseExactNumersInPixels)
                        {
                            Assert.NotEqual(GetPixelValue(pixels, i, j), GetPixelValue(pixelsChanged, i, j));
                        }
                        else
                        {
                            Assert.NotEqual(GetPixelValue(pixels, i, j), GetPixelValue(pixelsChanged, i, j), new Float1DimArrComparer());
                        }
                    }
                    else
                    {
                        if (UseExactNumersInPixels)
                        {
                            Assert.Equal(GetPixelValue(pixels, i, j), GetPixelValue(pixelsChanged, i, j));
                        }
                        else
                        {
                            Assert.Equal(GetPixelValue(pixels, i, j), GetPixelValue(pixelsChanged, i, j), new Float1DimArrComparer());
                        }
                    }
                }
            }
        }

        private float[,] GetChannelFromArr(float[,,] array, int channel)
        {
            var newArr = new float[array.GetLength(0),array.GetLength(1)];
            for (var i = 0; i < newArr.GetLength(0); i++)
            {
                for (var j = 0; j < newArr.GetLength(1); j++)
                {
                    newArr[i, j] = array[i, j, channel];
                }
            }

            return newArr;
        }

        private float[] GetPixelValue(float[,,] array, int x, int y)
        {
            var newArr = new float[array.GetLength(2)];
            for (var i = 0; i < array.GetLength(2); i++)
            {
                newArr[i] = array[x, y, i];
            }

            return newArr;
        }

        private class Float2DimArrComparer : IEqualityComparer<float[,]>
        {
            public bool Equals(float[,] x, float[,] y)
            {
                if (x.GetLength(0) != y.GetLength(0)) return false;
                if (x.GetLength(1) != y.GetLength(1)) return false;
                for (var i = 0; i < x.GetLength(0); i++)
                {
                    for (var j = 0; j < x.GetLength(1); j++)
                    {
                        if (Math.Abs(x[i, j] - y[i, j]) > 0.03) return false;
                    }
                }

                return true;
            }

            public int GetHashCode(float[,] obj)
            {
                return obj.GetHashCode();
            }
        }
        
        private class Float1DimArrComparer : IEqualityComparer<float[]>
        {
            public bool Equals(float[] x, float[] y)
            {
                if (x.GetLength(0) != y.GetLength(0)) return false;
                for (var i = 0; i < x.GetLength(0); i++)
                {
                    if (Math.Abs(x[i] - y[i]) > 0.03) return false;
                }

                return true;
            }

            public int GetHashCode(float[] obj)
            {
                return obj.GetHashCode();
            }
        }

        private class TestAreaSelector : IAreaSelector
        {
            public int LeftInclusive { get; set; }
            public int RightExclusive { get; set; }
            public int BotInclusive { get; set; }
            public int TopExclusive { get; set; }

            public TestAreaSelector()
            {
                LeftInclusive = 0;
                RightExclusive = 5;
                BotInclusive = 0;
                TopExclusive = 5;
            }
            public bool ShouldEdit(int x, int y)
            {
                return x == LeftInclusive && y == BotInclusive;
            }

            public void Resize(int left, int right, int bot, int top)
            {
                if (left + LeftInclusive < 0 || right + RightExclusive < 0 || bot + BotInclusive < 0 ||
                    top + TopExclusive < 0)
                    throw new ArgumentException("Argumants invalid.");
                LeftInclusive += left;
                RightExclusive += right;
                BotInclusive += bot;
                TopExclusive += top;
            }

            public void Resize(int horizontal, int vertical)
            {
                Resize(horizontal, horizontal, vertical, vertical);
            }

            public SquareAreaSelector AsSquareAreaSelector()
            {
                throw new NotImplementedException();
            }

            public void Validate(PSize pSize)
            {
                LeftInclusive = Math.Max(0, LeftInclusive);
                BotInclusive = Math.Max(0, BotInclusive);
                RightExclusive = Math.Min(pSize.Width, RightExclusive);
                TopExclusive = Math.Min(pSize.Height, TopExclusive);
                if (LeftInclusive > RightExclusive || BotInclusive > TopExclusive)
                    throw new ArgumentException("Invalid area selector.");
            }
        }
    }
}

