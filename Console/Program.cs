using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Blur;
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
            fastImage = await fastImage.ExecuteProcessorAsync(new MaxProcessor(new MaxParams{ChannelSelector = ChannelSelector.RGB, EdgeBehaviourType = EdgeBehaviourSelector.Type.Extend, PSize = new PSize{Width = 25,Height = 25}, WorkingArea = new SquareAreaSelector(500, 500, 1500, 1500)}), CancellationToken.None);
            sw.Stop();
            fastImage.Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output1.jpg");
            System.Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}