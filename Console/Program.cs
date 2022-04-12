using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Data.Operators.EdgeDetection;
using Sobczal.Picturify.Core.Processing;
using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Processing.EdgeDetection;
using Sobczal.Picturify.Core.Processing.Standard;
using Sobczal.Picturify.Core.Processing.Standard.Util;
using Sobczal.Picturify.Core.Processing.Testing;
using Sobczal.Picturify.Core.Utils;
using Sobczal.Picturify.Movie;
using Sobczal.Picturify.Movie.Transforms;

namespace Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                //         new SobelOperator5(), OperatorBeforeNormalizationFunc.Root2, useNonMaximumSuppression: true)))
                //     .ExecuteProcessor(new MaxBlurProcessor(new MaxBlurParams(ChannelSelector.RGB, new PSize(2, 2))))
                //     .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\rt2_supr.png");
                // fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                //         new SobelOperator5(), OperatorBeforeNormalizationFunc.Root2, useNonMaximumSuppression: false)))
                //     .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\rt2_nosupr.png");
                // fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                //         new SobelOperator5(), OperatorBeforeNormalizationFunc.Root3)))
                //     .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\rt3.jpg");
                // fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                //         new SobelOperator5(), OperatorBeforeNormalizationFunc.Root4)))
                //     .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\rt4.jpg");
                // fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                //         new SobelOperator5(), OperatorBeforeNormalizationFunc.Root5)))
                //     .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\rt5.jpg");
                // fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                //         new SobelOperator5(), OperatorBeforeNormalizationFunc.Log, useNonMaximumSuppression: true)))
                //     .ExecuteProcessor(new MaxBlurProcessor(new MaxBlurParams(ChannelSelector.RGB, new PSize(2, 2))))
                //     .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\log_supr.png");
                // fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                //         new SobelOperator5(), OperatorBeforeNormalizationFunc.Log, useNonMaximumSuppression: false)))
                //     .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\log_nosupr.png");
                // sw.Stop();
                // System.Console.WriteLine($"Ellapsed: {sw.ElapsedMilliseconds} ms.");

                // var sw = new Stopwatch();
                // sw.Start();
                // var files = Directory.EnumerateFiles(@"D:\dev\dotnet\libraries\images\PicturifyExamples\temp");
                // PicturifyConfig.SetLoggingLevel(PicturifyConfig.LoggingLevel.Error);
                // var i = 0;
                // foreach (var file in files)
                // {
                //     PicturifyConfig.LogError($"Done {(float) i / files.Count() * 100f} %");
                //     var fastImage = FastImageFactory.FromFile(file);
                //     fastImage.ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                //         new SobelOperator5(), OperatorBeforeNormalizationFunc.Log)));
                //     fastImage.Save(file);
                //     i++;
                // }
                // sw.Stop();
                // System.Console.WriteLine($"Ellapsed: {sw.ElapsedMilliseconds}" );

                // PicturifyConfig.SetLoggingLevel(PicturifyConfig.LoggingLevel.Fatal);
                var sobel = new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                    new SobelOperator5(), OperatorBeforeNormalizationFunc.Log, useNonMaximumSuppression: true, lowerBoundForNormalisation: -50, upperBoundForNormalisation: 100));
                var maxBlur = new GaussianBlurProcessor(new GaussianBlurParams(ChannelSelector.RGB, new PSize(2, 2), 2.4f));
                var normalisation = new NormalisationProcessor(new NormalisationParams(ChannelSelector.RGB));
                var threshold = new PointManipulationProcessor(new PointManipulationParams(ChannelSelector.RGB,
                    (a, r, g, b, x, y, selector) =>
                    {
                        if (selector.UseAlpha) a = a > 0.3f ? a : 0f;
                        if (selector.UseRed) r = r > 0.3f ? r : 0f;
                        if (selector.UseGreen) g = g > 0.3f ? g : 0f;
                        if (selector.UseBlue) b = b > 0.3f ? b : 0f;
                        return (a, r, g, b);
                    }));
                var fastImage =
                    FastImageFactory.FromFile(
                        @"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\cyber.jpg").ToGrayscale();
                fastImage.ExecuteProcessor(new MeanBlurProcessor(new MeanBlurParams(ChannelSelector.RGB, new PSize(7, 7))));
                fastImage.Save(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\output2.jpg");
                // MovieIO.MovieToMovie(@"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\videos\2055.mp4",
                // @"C:\dev\dotnet\libs\DataAndAlgorithms\Image\PicturifyExamples\videos\output.mp4",
                // new MultipleProcessorTransform(new List<IBaseProcessor>{sobel}), new PSize(1280, 720), 24, useSound: true, crfQuality: 18);
        }
    }
}