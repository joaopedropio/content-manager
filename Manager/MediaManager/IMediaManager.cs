using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MediaManagerLib
{
    public interface IMediaManager
    {
        EventHandler ConvertionDone { get; set; }
        DataReceivedEventHandler OutputReceived { get; set; }
        DataReceivedEventHandler ErrorReceived { get; set; }
        void UploadMedia(Stream media, string remotePath);
        Stream DownloadMedia(string remotePath);
        void RemoveMedia(string remotePath);
        void UploadDirectory(string localPath, string remotePath);
        void UploadDashMedia(Stream dashzip, string remotePath);
        Action<ulong> ProgressStatus { get; set; }
        Task<int> ConvertToMP4Format(string inputFilePath, string outputFilePath);
        Task<int> SegmentToDashFormat(string inputFilePath, string outputFilePath);
        Task<int> ConvertToMP4DashFormat(string inputFilePath, string outputFilePath);
    }
}
