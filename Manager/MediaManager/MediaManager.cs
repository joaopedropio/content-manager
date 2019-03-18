using MediaManagerLib.Converter;
using MediaManagerLib.Deployer;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MediaManagerLib
{
    public class MediaManager : IMediaManager
    {
        private readonly MediaConverter converter;
        private readonly SFTPMediaDeployer deployer;

        // Events
        public EventHandler ConvertionDone { get; set; }
        public DataReceivedEventHandler OutputReceived { get; set; }
        public DataReceivedEventHandler ErrorReceived { get; set; }

        // Callback
        public Action<ulong> ProgressStatus { get; set; }

        public MediaManager(string ffmpegExecutablePath, string mp4boxExecutablePath, string host, string username, string password, int port)
        {
            this.converter = new MediaConverter(ffmpegExecutablePath, mp4boxExecutablePath);
            this.deployer = new SFTPMediaDeployer(host, username, password, port);

            // Converter Events
            this.ConvertionDone += this.converter.ConvertionDone;
            this.OutputReceived += this.converter.OutputReceived;
            this.ErrorReceived += this.converter.ErrorReceived;

            // Deployer Callback
            this.deployer.ProgressStatus = this.ProgressStatus;
        }

        public async Task<int> ConvertToMP4DashFormat(string inputFilePath, string outputFilePath)
        {
            return await converter.ConvertToMP4DashFormat(inputFilePath, outputFilePath);
        }

        public Stream DownloadMedia(string remotePath)
        {
            return deployer.DownloadMedia(remotePath);
        }

        public void RemoveMedia(string remotePath)
        {
            deployer.RemoveMedia(remotePath);
        }

        public async Task<int> SegmentToDashFormat(string inputFilePath, string outputFilePath)
        {
            return await converter.SegmentToDashFormat(inputFilePath, outputFilePath);
        }

        public void UploadDashMedia(Stream dashzip, string remotePath)
        {
            deployer.UploadDashMedia(dashzip, remotePath);
        }

        public void UploadDirectory(string localPath, string remotePath)
        {
            deployer.UploadDirectory(localPath, remotePath);
        }

        public void UploadMedia(Stream media, string remotePath)
        {
            deployer.UploadMedia(media, remotePath);
        }

        public async Task<int> ConvertToMP4Format(string inputFilePath, string outputFilePath)
        {
            return await converter.ConvertToMP4Format(inputFilePath, outputFilePath);
        }
    }
}
