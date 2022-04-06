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
            var fastImage = FastImageFactory.FromFile(@"D:\dev\dotnet\libraries\images\PicturifyExamples\wtf.png");
            // var beforeB = new BeforeProcessorB(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Crop, new PSize(500, 500)));
            // var beforeF = new BeforeProcessorF(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Crop, new PSize(500, 500)));
            // fastImage.GetCopy().ExecuteProcessor(beforeB).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputB.jpg");
            // fastImage.GetCopy().ExecuteProcessor(beforeF).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputF.jpg");
            var procParams = new TwoChannelConvolutionParams(ChannelSelector.RGB,
                new List<float[,]>() {new float[,] {{-1f, 0f, 1f}}, new float[,] {{1f}, {2f}, {1f}}},
                new List<float[,]>() {new float[,] {{-1f}, {0f}, {1f}}, new float[,] {{1f, 2f, 1f}}},
                (in1, in2, channel) => (float) Math.Sqrt(in1 * in1 + in2 * in2));
            fastImage.ExecuteProcessor(new TwoChannelConvolutionProcessor(procParams))
                .ExecuteProcessor(new NormalisationProcessor(new NormalisationParams(ChannelSelector.RGB)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output.jpg");

            // fastImage.ExecuteProcessor(new MaxProcessor(new MaxParams(ChannelSelector.RGB, new PSize(1, 1))))
                // .Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\output.jpg");
            // var procParams =
                // new ConvolutionParams(ChannelSelector.RGB, new float[,] {{1, 1, 2, 1,1}, {1, 2, 4, 2, 1}, {2, 4, -44, 2, 4}, {1, 2, 4, 2, 1}, {1, 1, 2, 1,1}});
            // fastImage.ExecuteProcessor(new ConvolutionProcessor(procParams)).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\output.jpg");
        }
    }
}