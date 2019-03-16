using MediaManager;
using MediaManagerTests.Helpers;
using NUnit.Framework;
using System.IO;

namespace MediaManagerTests
{
    public class SFTPMediaManagerTests
    {
        private SFTPMediaManager sftpClient;
        private Stream media;

        public SFTPMediaManagerTests()
        {
            var host = "socialmovie.minivps.info";
            var port = 2222;
            var username = "content";
            var password = "password";
            var fileName = "media.avi";

            this.sftpClient = new SFTPMediaManager(host, username, password, port);
            this.media = FileHelper.GetInputFile(fileName);
        }
        
        [Test]
        public void Should_UploadMedia()
        {
            sftpClient.UploadMedia(media, "/content/media.avi");
        }

        [Test]
        public void Should_DownloadMedia()
        {
            var filePath = "C:\\medias\\output.avi";
            var media = sftpClient.DownloadMedia("/content/media.avi");
            FileHelper.SaveFile(media, filePath);
        }

        [Test]
        public void Should_RemoveMedia()
        {
            sftpClient.RemodeMedia("/content/media.avi");
        }
    }
}
