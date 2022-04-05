using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Processing.Standard;
using Sobczal.Picturify.Core.Processing.Standard.Util;
using Sobczal.Picturify.Core.Processing.Testing;
using Sobczal.Picturify.Core.Utils;

namespace Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var fastImage = FastImageFactory.FromFile(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\mountain.jpg");
            // var beforeB = new BeforeProcessorB(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Crop, new PSize(500, 500)));
            // var beforeF = new BeforeProcessorF(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Crop, new PSize(500, 500)));
            // fastImage.GetCopy().ExecuteProcessor(beforeB).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputB.jpg");
            // fastImage.GetCopy().ExecuteProcessor(beforeF).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputF.jpg");
            // var procParams = new TwoChannelConvolutionParams(ChannelSelector.RGB,
            //     new List<float[,]>() {new float[,] {{-1f, 0f, 1f}}, new float[,] {{1f}, {2f}, {1f}}},
            //     new List<float[,]>() {new float[,] {{-1f}, {0f}, {1f}}, new float[,] {{1f, 2f, 1f}}},
            //     ((in1, in2, channel) => (float) in1 * in1 + in2 * in2 > 0.5f ? 1.0f : 0f));
            // (await fastImage.ExecuteProcessorAsync(new TwoChannelConvolutionProcessorF(procParams), CancellationToken.None))
            //     .Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\output.jpg");

            // fastImage.ExecuteProcessor(new MaxProcessor(new MaxParams(ChannelSelector.RGB, new PSize(1, 1))))
            //     .Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\output.jpg");
            var procParams =
                new ConvolutionParams(ChannelSelector.RGB, new float[,] {{-1, -1, -1}, {-1, 8, -1}, {-1, -1, -1}});
            fastImage.ExecuteProcessor(new ConvolutionProcessor(procParams)).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\output.jpg");
        }
    }
}