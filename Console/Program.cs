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
            // var fastImage =
                // FastImageFactory.FromFile(@"D:\dev\dotnet\libraries\images\PicturifyExamples\phineas.jpg");
                // fastImage.Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output.jpg");
                // var sw = new Stopwatch();
                // sw.Start();
                // fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                //     new SobelOperator5(), OperatorBeforeNormalizationFunc.Linear)))
                //     .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\linear.jpg");
                // fastImage.GetCopy().ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB,
                //         new SobelOperator5(), OperatorBeforeNormalizationFunc.Root2)))
                //     .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\rt2.jpg");
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
                //         new SobelOperator5(), OperatorBeforeNormalizationFunc.Log)))
                //     .Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\Sobel\log_divided.jpg");
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

                MovieIO.MovieToMovie(@"D:\dev\dotnet\libraries\images\PicturifyExamples\createdVideos\zenitsu_sound.mp4",
                    String.Empty, null);
        }
    }
}