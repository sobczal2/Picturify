using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Movie.Transforms;

namespace Sobczal.Picturify.Movie
{
    public static class MovieIO
    {
        public static void MovieToMovie(string inputFile, string outputFile, IMovieTransform movieTransform)
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                Arguments = $@"-i {inputFile} -c:v bmp -vf scale=1920:1080 -f rawvideo -",
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = "ffmpeg.exe",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            process.Start();
            using (var stream = new MemoryStream())
            {
                var j = 0;
                while (process.StandardOutput.BaseStream.CanRead)
                {
                    var startingBytes = new byte[6];
                    for (var i = 0; i < 2; i++)
                    {
                        startingBytes[i] = (byte) process.StandardOutput.BaseStream.ReadByte();
                    }

                    for (var i = 2; i < 6; i++)
                    {
                        startingBytes[i] = (byte) process.StandardOutput.BaseStream.ReadByte();
                    }

                    var imgSize = BitConverter.ToInt32(startingBytes, 2);

                    for (var i = 0; i < 6; i++)
                    {
                        stream.WriteByte(startingBytes[i]);
                    }

                    for (var i = 6; i < imgSize; i++)
                    {
                        stream.WriteByte((byte) process.StandardOutput.BaseStream.ReadByte());
                    }

                    stream.Position = 0;
                    FastImageFactory.FromStream(stream).Save($@"D:\dev\dotnet\libraries\images\PicturifyExamples\temp\output{j++}.jpg");
                    Console.WriteLine("position" + stream.Position);
                    Console.WriteLine("length" + stream.Length);
                }
            }

        }
    }
}