using System.Drawing;
using System.Reflection;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;
using Xunit;

namespace Sobczal.Picturify.Core.Tests.Data;

public class FastImageFactoryTests
{
    [Theory]
    [InlineData(10, 10)]
    [InlineData(100, 100)]
    [InlineData(1000, 1000)]
    [InlineData(10, 1000)]
    [InlineData(1000, 10)]
    public void Empty_ByteVersion_ShouldReturnInstanceOfCorrectSize(int width, int height)
    {
        var expected = new PSize(width, height);
        var fastImage = FastImageFactory.Empty(expected, FastImageFactory.Version.Byte);

        var actual = fastImage.PSize;

        Assert.IsType<FastImageB>(fastImage);
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(10, 10)]
    [InlineData(100, 100)]
    [InlineData(1000, 1000)]
    [InlineData(10, 1000)]
    [InlineData(1000, 10)]
    public void Empty_FloatVersion_ShouldReturnInstanceOfCorrectSize(int width, int height)
    {
        var expected = new PSize(width, height);
        var fastImage = FastImageFactory.Empty(expected, FastImageFactory.Version.Float);

        var actual = fastImage.PSize;

        Assert.IsType<FastImageF>(fastImage);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(10, 10)]
    [InlineData(100, 100)]
    [InlineData(1000, 1000)]
    [InlineData(10, 1000)]
    [InlineData(1000, 10)]
    public void FromImage_ByteVersion_ShouldReturnInstanceOfCorrectSize(int width, int height)
    {
        var bitmap = CreateEmptyBitmap(width, height);

        var expectedArray = new byte[] {255, 210, 105, 30};

        bitmap.SetPixel(width / 2, height / 2, Color.Chocolate);

        var fastImage = FastImageFactory.FromImage(bitmap, FastImageFactory.Version.Byte);
        var fieldInfo = typeof(FastImageB).GetField("Pixels", BindingFlags.NonPublic | BindingFlags.Instance);

        var actualArray = (byte[,,]) fieldInfo.GetValue(fastImage);
        
        Assert.IsType<FastImageB>(fastImage);
        Assert.NotNull(actualArray);
        if (actualArray != null)
        {
            Assert.Equal(expectedArray[0], actualArray[width / 2, height / 2, 0]);
            Assert.Equal(expectedArray[1], actualArray[width / 2, height / 2, 1]);
            Assert.Equal(expectedArray[2], actualArray[width / 2, height / 2, 2]);
            Assert.Equal(expectedArray[3], actualArray[width / 2, height / 2, 3]);
        }
    }
    
    [Theory]
    [InlineData(10, 10)]
    [InlineData(100, 100)]
    [InlineData(1000, 1000)]
    [InlineData(10, 1000)]
    [InlineData(1000, 10)]
    public void FromImage_FloatVersion_ShouldReturnInstanceOfCorrectSize(int width, int height)
    {
        var bitmap = CreateEmptyBitmap(width, height);

        var expectedArray = new float[] {255f/255f, 210f/255f, 105f/255f, 30f/255f};

        bitmap.SetPixel(width / 2, height / 2, Color.Chocolate);
        
        var fastImage = FastImageFactory.FromImage(bitmap, FastImageFactory.Version.Float);
        var fieldInfo = typeof(FastImageF).GetField("Pixels", BindingFlags.NonPublic | BindingFlags.Instance);

        var actualArray = (float[,,]) fieldInfo.GetValue(fastImage);
        
        Assert.IsType<FastImageF>(fastImage);
        Assert.NotNull(actualArray);
        if (actualArray != null)
        {
            Assert.Equal(expectedArray[0], actualArray[width / 2, height / 2, 0], 10);
            Assert.Equal(expectedArray[1], actualArray[width / 2, height / 2, 1], 10);
            Assert.Equal(expectedArray[2], actualArray[width / 2, height / 2, 2], 10);
            Assert.Equal(expectedArray[3], actualArray[width / 2, height / 2, 3], 10);
        }
    }

    private Bitmap CreateEmptyBitmap(int width, int height)
    {
        var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);
        var rect = new Rectangle(0, 0, width, height);
        graphics.FillRectangle(new SolidBrush(Color.FromArgb(0,0,0,0)), rect);
        return bitmap;
    }
}