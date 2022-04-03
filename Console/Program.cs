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
            fastImage = fastImage.ExecuteProcessor(
                new MedianProcessor(new MedianParams
                {
                    ChannelSelector = ChannelSelector.RGB, EdgeBehaviourType = EdgeBehaviourSelector.Type.Extend,
                    PSize = new PSize(25,25)
                }));
            sw.Stop();
            fastImage.Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output.jpg");
            System.Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}