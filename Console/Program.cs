using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Data.Operators.EdgeDetection;
using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Processing.EdgeDetection;
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
            var fastImage = FastImageFactory.FromFile(@"D:\dev\dotnet\libraries\images\PicturifyExamples\sea.jpg");
            // var beforeB = new BeforeProcessorB(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Crop, new PSize(500, 500)));
            // var beforeF = new BeforeProcessorF(new EmptyProcessorParams()).AddFilter(
            //     EdgeBehaviourSelector.GetFilter(EdgeBehaviourSelector.Type.Crop, new PSize(500, 500)));
            // fastImage.GetCopy().ExecuteProcessor(beforeB).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputB.jpg");
            // fastImage.GetCopy().ExecuteProcessor(beforeF).Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\outputF.jpg");
            var sw = new Stopwatch();
            sw.Start();
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                new SobelOperator3(), OperatorBeforeNormalizationFunc.Linear)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\linear.jpg");
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator3(), OperatorBeforeNormalizationFunc.Root2)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\rt2.jpg");
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator3(), OperatorBeforeNormalizationFunc.Root3)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\rt3.jpg");
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator3(), OperatorBeforeNormalizationFunc.Root4)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\rt4.jpg");
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator3(), OperatorBeforeNormalizationFunc.Root5)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\rt5.jpg");
            sw.Stop();
            System.Console.WriteLine($"Ellapsed: {sw.ElapsedMilliseconds} ms.");

        }
    }
}