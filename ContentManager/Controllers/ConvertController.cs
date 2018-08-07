using FFMPEGWrapper;
using FFMPEGWrapper.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using static Helper.FileNameExtensions;
using System.Diagnostics;
using ContentManager.WebSocketHelpers;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace ContentManager.Controllers
{
    [Route("/convert")]
    public class ConvertController : Controller
    {
        private string FfmpegExecutablePath;
        private string MP4BoxExecutablePath;
        private string tempFolder;
        private string convertedFolder;
        private string compresssedFolder;
        private FFMPEG ffmpeg;
        private ProcessMessageHandler webSocket;

        public async void OnErrorReceived(object sender, DataReceivedEventArgs d)
        {
            Console.WriteLine(d.Data);
            await webSocket.SendMessageToAllAsync(d.Data);
        }

        public async void OnConvertionDone(object sender, EventArgs e)
        {
            Console.WriteLine("Done!");
        }

        public ConvertController(Configuration config, ProcessMessageHandler webSocket)
        {
            this.FfmpegExecutablePath = config.FfmpegExecutablePath;
            this.MP4BoxExecutablePath = config.MP4BoxExecutablePath;
            this.tempFolder = config.TempFolder;
            this.convertedFolder = config.ConvertedFolder;
            this.compresssedFolder = config.CompresssedFolder;
            this.ffmpeg = new FFMPEG(FfmpegExecutablePath);
            this.ffmpeg.ErrorReceived += OnErrorReceived;
            this.ffmpeg.ConvertionDone += OnConvertionDone;
            this.webSocket = webSocket;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("mp4")]
        [HttpPost]
        public async Task<IActionResult> ConvertVideo()
        {
            HandleDirectories(tempFolder);

            var file = GetFileFromRequest(Request);
            var name = GetNameFromFileName(file.FileName);
            var inputFilePath = tempFolder + file.FileName;
            var outputFilePath = tempFolder + name + ".mp4";
            var outputFileName = name + ".mp4";

            file.Save(inputFilePath);

            await ffmpeg.Convert(Profile.SimpleMP4, inputFilePath, outputFilePath);

            //ffmpeg.Convert(Profile.EspecificForDash, inputFilePath, outputFilePath);

            return UploadFile(outputFilePath, outputFileName);
        }

        [Route("dash")]
        [HttpPost]
        public IActionResult SegmentVideo()
        {
            HandleDirectories(convertedFolder, compresssedFolder);

            var file = GetFileFromRequest(Request);
            var filePath = tempFolder + file.FileName;
            file.Save(filePath);
            var name = GetNameFromFileName(file.FileName);
            var compressedFile = BuildFilePath(compresssedFolder, name + ".zip");
            
            //mp4box.Dashify();

            //var compressedFile = CompressFiles(convertedFolder, file.FileName, compresssedFolder);

            ZipFile.CreateFromDirectory(convertedFolder, compresssedFolder);


            return UploadCompressedFile(compressedFile);
        }

        private IFormFile GetFileFromRequest(HttpRequest request) => request.Form.Files[0];

        private void HandleDirectories(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                Directory.CreateDirectory(path);
            }
        }

        private IActionResult UploadFile(string file, string fileName)
        {
            var bytes = System.IO.File.ReadAllBytes(file);
            var stream = new MemoryStream(bytes);
            return File(stream, "application/octet-stream", fileName);
        }

        private IActionResult UploadCompressedFile(string file)
        {
            var bytes = System.IO.File.ReadAllBytes(file);
            var stream = new MemoryStream(bytes);
            return File(stream, "application/octet-stream", "video.zip");
        }
    }
}
