using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Processing.Testing;
using Sobczal.Picturify.Core.Utils;

namespace Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var sw = new Stopwatch();
            var fastImage = FastImageFactory.FromFile(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\mountain.jpg", FastImageFactory.Version.Byte).Resize(new PSize(1280, 720));
            sw.Start();
            // var beforeB = new BeforeProcessorB(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Crop, new PSize(500, 500)));
            // var beforeF = new BeforeProcessorF(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Crop, new PSize(500, 500)));
            // fastImage.GetCopy().ExecuteProcessor(beforeB).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputB.jpg");
            // fastImage.GetCopy().ExecuteProcessor(beforeF).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputF.jpg");
            (await fastImage.ExecuteProcessorAsync(new MaxProcessor(new MaxParams(ChannelSelector.RGB, new PSize(10, 10), EdgeBehaviourSelector.Type.Crop, new SquareAreaSelector(100, 1000, 100, 1000))), CancellationToken.None))
                .Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\output.jpg");
            sw.Stop();
            System.Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}