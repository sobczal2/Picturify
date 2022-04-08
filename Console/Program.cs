using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core;
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
        public static void Main(string[] args)
        {
            var fastImage =
                FastImageFactory.FromFile(@"D:\dev\dotnet\libraries\images\PicturifyExamples\phineas.jpg");
            fastImage.Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output.jpg");
            var sw = new Stopwatch();
            sw.Start();
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                new SobelOperator5(), OperatorBeforeNormalizationFunc.Linear, workingArea: new SquareAreaSelector(100, 600, 100, 600))))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\linear.jpg");
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator5(), OperatorBeforeNormalizationFunc.Root2)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\rt2.jpg");
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator5(), OperatorBeforeNormalizationFunc.Root3)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\rt3.jpg");
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator5(), OperatorBeforeNormalizationFunc.Root4)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\rt4.jpg");
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator5(), OperatorBeforeNormalizationFunc.Root5)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\rt5.jpg");
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator5(), OperatorBeforeNormalizationFunc.Log2)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\log2.jpg");
            fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator5(), OperatorBeforeNormalizationFunc.Log10)))
                .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\log10.jpg");
            sw.Stop();
            System.Console.WriteLine($"Ellapsed: {sw.ElapsedMilliseconds} ms.");
        }
    }
}