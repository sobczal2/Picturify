using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing;
using Sobczal.Picturify.Core.Utils;

namespace Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var fastImage = FastImage.FromFile(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output.jpg");
            fastImage.GetCopy().Crop(new SquareAreaSelector(100, 200, 800, 600)).Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output2.jpg");
            fastImage.GetCopy().ToGrayscale().Crop(new SquareAreaSelector(100, 200, 800, 600)).Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output3.jpg");
        }
    }
}