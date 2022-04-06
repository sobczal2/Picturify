using System.Collections.Generic;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing;
using Sobczal.Picturify.Core.Utils;
using Xunit;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors
{
    /// <summary>
    /// Create class which derives from this to run some basic tests on concrete processor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractProcessorTests<T> where T : IBaseProcessor
    {
        public List<T> SizeCheckProcessors { get; set; }
        public List<T> WorkingAreaCheckProcessors { get; set; }

        public AbstractProcessorTests()
        {
            WorkingAreaCheckProcessors = new List<T>();
            SizeCheckProcessors = new List<T>();
        }
        [Theory]
        [InlineData(10, 10)]
        [InlineData(100, 10)]
        [InlineData(10, 100)]
        [InlineData(100, 100)]
        [InlineData(500, 500)]
        public void ExecuteProcessor_ShouldNotChangeImageSize(int imgWidth, int imgHeight)
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
    }
}

