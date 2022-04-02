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
            var fastImage = FastImageFactory.FromFile(@"C:\Users\lukas\Downloads\heightmaps\heightmaps\BGIMGimg.png");
            sw.Start();
            fastImage = await fastImage.ToGrayscale().ExecuteProcessorAsync(
                new MedianProcessor(new MedianParams
                {
                    ChannelSelector = ChannelSelector.RGB, EdgeBehaviourType = EdgeBehaviourSelector.Type.Extend,
                    PSize = new PSize(25,25)
                }), CancellationToken.None);
            sw.Stop();
            fastImage.Save(@"C:\Users\lukas\Downloads\heightmaps\heightmaps\output.png");
            System.Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}