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
            var fastImage = FastImageFactory.FromFile(@"D:\dev\dotnet\libraries\images\PicturifyExamples\mountain.jpg");
            sw.Start();
            var beforeB = new BeforeProcessorB(new EmptyProcessorParams()).AddFilter(
                EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Wrap, new PSize(3841, 2161)));
            var beforeF = new BeforeProcessorF(new EmptyProcessorParams()).AddFilter(
                EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Wrap, new PSize(3841, 2161)));
            fastImage.ExecuteProcessor(beforeB).Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\outputB.jpg");
            fastImage.ExecuteProcessor(beforeF).Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\outputF.jpg");
            sw.Stop();
            fastImage.Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output.jpg");
            System.Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}