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
        public List<T> WorkingAreaCheckProcessors { get; set; }
        public T ChannelSelectorCheckProcessor { get; set; }

        public bool UseExactNumersInPixels { get; set; }

        public AbstractProcessorTests()
        {
            WorkingAreaCheckProcessors = new List<T>();
            SizeCheckProcessors = new List<T>();
            UseExactNumersInPixels = true;
            PopulateSizeCheckProcessors();
            PopulateChannelSelectorCheckProcessors();
        }

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
                        Assert.NotEqual(original[i], changed[i], new FloatArrComparer());
                    }
                }
                else
                {
                    if(UseExactNumersInPixels)
                        Assert.Equal(original[i], changed[i]);
                    else
                    {
                        Assert.Equal(original[i], changed[i], new FloatArrComparer());
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

        private class FloatArrComparer : IEqualityComparer<float[,]>
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
    }
}

