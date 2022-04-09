using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Exceptions;
using Sobczal.Picturify.Core.Utils;
using Sobczal.Picturify.Movie.Transforms;

namespace Sobczal.Picturify.Movie
{
    public static class MovieIO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="movieTransform"></param>
        /// <param name="size"></param>
        /// <param name="outputFramerate"></param>
        /// <param name="useSound"></param>
        /// <param name="crfQuality">Quality setting - 2 best quality, 31 - worst quality, 15-18 recommended</param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ParamsArgumentException"></exception>
        public static void MovieToMovie(string inputFile, string outputFile, IMovieTransform movieTransform, PSize size, float outputFramerate, bool useSound = true, int crfQuality = 15)
        {
            if (size.Width <= 0 || size.Height <= 0)
                throw new ArgumentException("can't be negative or zero", nameof(size));
            if(crfQuality < 2 || crfQuality > 31)
                throw new ArgumentException("must be in rage 2-31", nameof(crfQuality));
            var readProcess = new Process();
            readProcess.StartInfo = new ProcessStartInfo
            {
                Arguments = $@"-hide_banner -v panic -i {inputFile} -s {size.Width}x{size.Height} -c:v bmp -f image2pipe -",
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = "ffmpeg.exe",
                RedirectStandardOutput = true,
            };
            
            var writeProcess = new Process();
            var soundArg = useSound ? $"-i {inputFile}" : String.Empty;
            var soundArg2 = useSound ? "-map 1:a" : string.Empty;
            writeProcess.StartInfo = new ProcessStartInfo
            {
                Arguments = $@"-y -r {outputFramerate} -f jpeg_pipe -s {size.Width}x{size.Height} -i - {soundArg} -crf {crfQuality} -map 0:v {soundArg2} -shortest {outputFile}",
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = "ffmpeg.exe",
                RedirectStandardInput = true,
            };
            
            readProcess.Start();
            writeProcess.Start();
            try
            {
                using (var stream = new MemoryStream())
                {
                    while (!readProcess.HasExited)
                    {
                        var startingBytes = new byte[6];
                        for (var i = 0; i < 2; i++)
                        {
                            startingBytes[i] = (byte) readProcess.StandardOutput.BaseStream.ReadByte();
                        }

                        for (var i = 2; i < 6; i++)
                        {
                            startingBytes[i] = (byte) readProcess.StandardOutput.BaseStream.ReadByte();
                        }

                        var imgSize = BitConverter.ToInt32(startingBytes, 2);

                        stream.Position = 0;
                        for (var i = 0; i < 6; i++)
                        {
                            stream.WriteByte(startingBytes[i]);
                        }

                        for (var i = 6; i < imgSize; i++)
                        {
                            stream.WriteByte((byte) readProcess.StandardOutput.BaseStream.ReadByte());
                        }

                        var fastImage = FastImageFactory.FromStream(stream);

                        fastImage = movieTransform.GetNext(fastImage);

                        if (fastImage.PSize.Width != size.Width || fastImage.PSize.Height != size.Height)
                            throw new Exception("Image can't change size in transformation.");
                        fastImage.Save(writeProcess.StandardInput.BaseStream, ImageFormat.Jpeg);
                    }
                }
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(
                    "File not found or ffmpeg.exe not found(try adding it to path)");
            }
            finally
            {
                readProcess.Close();
                writeProcess.Close();
            }
        }
    }
}