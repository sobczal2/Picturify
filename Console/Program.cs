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
            var fastImage = FastImageFactory.FromFile(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\sobel.png", FastImageFactory.Version.Byte).ToGrayscale();
            sw.Start();
            // var beforeB = new BeforeProcessorB(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Mirror, new PSize(50, 50)));
            // var beforeF = new BeforeProcessorF(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Mirror, new PSize(50, 50)));
            // fastImage.GetCopy().ExecuteProcessor(beforeB).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputB.jpg");
            // fastImage.GetCopy().ExecuteProcessor(beforeF).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputF.jpg");
            (await fastImage.ExecuteProcessorAsync(new MinProcessor(new MinParams(ChannelSelector.RGB, new PSize(50, 50), EdgeBehaviourSelector.Type.Mirror)), CancellationToken.None))
                .Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\output.jpg");
            sw.Stop();
            System.Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}