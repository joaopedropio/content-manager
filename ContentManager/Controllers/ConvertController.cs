using ContentManager.WebSocketHelpers;
using MediaManagerLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using static Helper.FileHelper;

namespace ContentManager.Controllers
{
    [Route("/convert")]
    public class ConvertController : Controller
    {
        private readonly ProcessMessageHandler webSocket;
        private readonly IMediaManager mediaManager;
        private readonly string tempFolder;
        private readonly string convertedFolder;
        private readonly string compresssedFolder;

        public async void OnErrorReceived(object sender, DataReceivedEventArgs d)
        {
            Console.WriteLine(d.Data);
            await webSocket.SendMessageToAllAsync(d.Data);
        }

        public async void OnOutputReceived(object sender, DataReceivedEventArgs e)
        {
            await Task.Run(() => Console.WriteLine("Done!"));
        }

        public async void OnConvertionDone(object sender, EventArgs e)
        {
            await Task.Run(() => Console.WriteLine("Done!"));
        }

        public async void ProgressStatus(ulong percentage)
        {
            await Task.Run(() => Console.WriteLine(percentage));
        }

        public ConvertController(IMediaManager mediaManager, ProcessMessageHandler webSocket)
        {
            var config = new Configuration();

            this.tempFolder = config.TempFolder;
            this.convertedFolder = config.ConvertedFolder;
            this.compresssedFolder = config.CompresssedFolder;
            this.mediaManager = mediaManager;
            this.webSocket = webSocket;

            this.mediaManager.ConvertionDone = this.OnConvertionDone;
            this.mediaManager.ErrorReceived = this.OnErrorReceived;
            this.mediaManager.OutputReceived = this.OnOutputReceived;
            this.mediaManager.ProgressStatus = this.ProgressStatus;
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
            HandleDirectories(this.tempFolder);

            var file = GetFileFromRequest(Request);
            var name = GetNameFromFileName(file.FileName);
            var inputFilePath = this.tempFolder + file.FileName;
            var outputFilePath = this.tempFolder + name + ".mp4";
            var outputFileName = name + ".mp4";

            file.Save(inputFilePath);

            await mediaManager.ConvertToMP4Format(inputFilePath, outputFilePath);

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
