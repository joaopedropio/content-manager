using FFMPEGWrapper;
using FFMPEGWrapper.Enums;
using MP4BoxWrapper;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MediaManagerLib.Converter
{
    internal class MediaConverter
    {
        // Executables
        private FFMPEG ffmpeg;
        private MP4Box mp4box;

        // Events
        internal EventHandler ConvertionDone { get; set; }
        internal DataReceivedEventHandler OutputReceived { get; set; }
        internal DataReceivedEventHandler ErrorReceived { get; set; }

        //public async void OnErrorReceived(object sender, DataReceivedEventArgs d)
        //{
        //    Console.WriteLine(d.Data);
        //    await webSocket.SendMessageToAllAsync(d.Data);
        //}

        //public async void OnConvertionDone(object sender, EventArgs e)
        //{
        //    Console.WriteLine("Done!");
        //}

        internal MediaConverter(string ffmpegExecutablePath, string  mp4boxExecutablePath)
        {
            // Working folders
            //this.tempFolder = tempFolder;
            //this.convertedFolder = convertedFolder;
            //this.compresssedFolder = compressedFolder;

            // Contruct FFMPEG
            this.ffmpeg = new FFMPEG(ffmpegExecutablePath);
            this.ffmpeg.ErrorReceived += ErrorReceived;
            this.ffmpeg.ConvertionDone += ConvertionDone;

            //Construct MP4BOX
            this.mp4box = new MP4Box(mp4boxExecutablePath);
            this.mp4box.ErrorReceived += ErrorReceived;
            this.mp4box.ConvertionDone += ConvertionDone;
        }
        internal async Task<int> ConvertToMP4Format(string inputFilePath, string outputFilePath)
        {
            //PathHelper.CleanDirectories(tempFolder);

            //var name = FileHelper.GetNameFromFileName(formFile.FileName);
            //var inputFilePath = tempFolder + formFile.FileName;
            //var outputFilePath = tempFolder + name + ".mp4";
            //var outputFileName = name + ".mp4";

            //formFile.Save(inputFilePath);

            var exitStatus = await ffmpeg.Convert(Profile.SimpleMP4, inputFilePath, outputFilePath);
            return exitStatus;

            //ffmpeg.Convert(Profile.EspecificForDash, inputFilePath, outputFilePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionInfo"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="username">The username.</param>
        /// <param name="authenticationMethods">The authentication methods.</param>
        /// <exception cref="ArgumentNullException"><paramref name="host"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="host"/> is a zero-length string.</exception>
        /// <exception cref="ArgumentException"><paramref name="username" /> is <c>null</c>, a zero-length string or contains only whitespace characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="authenticationMethods"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">No <paramref name="authenticationMethods"/> specified.</exception>
        /// <returns>The buffer.</returns>
        internal async Task<int> ConvertToMP4DashFormat(string inputFilePath, string outputFilePath)
        {
            return await ffmpeg.Convert(Profile.SimpleDash, inputFilePath, outputFilePath);
        }

        internal async Task<int> SegmentToDashFormat(string inputFilePath, string outputFilePath)
        {
            return await mp4box.Dashify(inputFilePath, outputFilePath);
        }
    }
}
