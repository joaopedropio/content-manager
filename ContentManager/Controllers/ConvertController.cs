using FFMPEGWrapper;
using FFMPEGWrapper.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.IO.Compression;
using static Helper.FileNameExtensions;

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

        public ConvertController(Configuration config)
        {
            this.FfmpegExecutablePath = config.FfmpegExecutablePath;
            this.MP4BoxExecutablePath = config.MP4BoxExecutablePath;
            this.tempFolder = config.TempFolder;
            this.convertedFolder = config.ConvertedFolder;
            this.compresssedFolder = config.CompresssedFolder;
            this.ffmpeg = new FFMPEG(FfmpegExecutablePath);
        }

        [Route("mp4")]
        [HttpPost]
        public IActionResult ConvertVideo()
        {
            HandleDirectories(convertedFolder, compresssedFolder);

            var file = GetFileFromRequest(Request);
            var name = GetNameFromFileName(file.FileName);
            var inputFilePath = tempFolder + file.FileName;
            var outputFilePath = tempFolder + name + ".mp4";
            var outputFileName = name + ".mp4";

            SaveFile(inputFilePath, file);

            ffmpeg.Convert(Profile.SimpleMP4, inputFilePath, outputFilePath);

            return UploadFile(outputFilePath, outputFileName);
        }

        [Route("dash")]
        [HttpPost]
        public IActionResult SegmentVideo()
        {
            HandleDirectories(convertedFolder, compresssedFolder);

            var file = GetFileFromRequest(Request);
            var filePath = tempFolder + file.FileName;
            var name = GetNameFromFileName(file.FileName);
            var compressedFile = BuildFilePath(compresssedFolder, name + ".zip");

            SaveFile(filePath, file);

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

        private void SaveFile(string filePath, IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
        }
    }
}
