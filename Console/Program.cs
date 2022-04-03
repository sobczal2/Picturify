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
            var fastImage = FastImageFactory.FromFile(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\mountain.jpg", FastImageFactory.Version.Float);
            sw.Start();
            // var beforeB = new BeforeProcessorB(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Wrap, new PSize(500, 500)));
            // var beforeF = new BeforeProcessorF(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Wrap, new PSize(500, 500)));
            // fastImage.ExecuteProcessor(beforeB).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputB.jpg");
            // fastImage.ExecuteProcessor(beforeF).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputF.jpg");
            fastImage.ExecuteProcessor(new MedianProcessor(new MedianParams(ChannelSelector.RGB, new PSize(50, 2), EdgeBehaviourSelector.Type.Wrap)))
                .Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\output.jpg");
            sw.Stop();
            System.Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}