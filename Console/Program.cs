using System.Diagnostics;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;

var sw = new Stopwatch();
sw.Start();
var image = FastImageFactory.FromFile(@"D:\dev\dotnet\libraries\images\PicturifyExamples\temp\frame00001.jpg", ImageType.ARGB);
image.AsGreyscale().Save(@"D:\dev\dotnet\libraries\images\PicturifyExamples\output.jpg");

sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds);