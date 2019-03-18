using MediaManagerLib;
using MediaManagerLib.Deployer;
using MediaManagerTests.Helpers;
using NUnit.Framework;
using System.IO;

namespace MediaManagerTests
{
    public class SFTPMediaManagerTests
    {
        private SFTPMediaDeployer sftpClient;
        private Stream AVIMedia;
        private Stream DashMedia;

        public SFTPMediaManagerTests()
        {
            var host = "socialmovie.minivps.info";
            var port = 2222;
            var username = "content";
            var password = "password";

            this.sftpClient = new SFTPMediaDeployer(host, username, password, port);
            this.AVIMedia = FileHelper.GetInputFile("media.avi");
            this.DashMedia = FileHelper.GetInputFile("dashvideo.zip");
        }
        
        [Test]
        public void Should_UploadMedia()
        {
            sftpClient.UploadMedia(AVIMedia, "/content/media.avi");
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
            sftpClient.RemoveMedia("/content/media.avi");
        }

        [Test]
        public void Should_DashMedia()
        {
            // dash media must be zipped and have just files, no directories
            sftpClient.UploadDashMedia(DashMedia, "/content/test/");
        }
    }
}
