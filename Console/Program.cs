using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing;
using Sobczal.Picturify.Core.Processing.Filters.EdgeBehaviour;
using Sobczal.Picturify.Core.Processing.Testing;
using Sobczal.Picturify.Core.Utils;

namespace Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var fastImage = FastImageFactory.FromFile(@"D:\dev\dotnet\libraries\images\PicturifyExamples\mountain.jpg");
            fastImage.GetCopy().ToByteRepresentation()
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output1.jpg");
            fastImage.GetCopy().ToByteRepresentation().ToGrayscale().Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output2.jpg");

        }
    }
}