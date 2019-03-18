using System.IO;
using System.Threading.Tasks;

namespace MediaManagerLib
{
    public interface IMediaManager
    {
        void UploadMedia(Stream media, string remotePath);
        Stream DownloadMedia(string remotePath);
        void RemoveMedia(string remotePath);
        void UploadDirectory(string localPath, string remotePath);
        void UploadDashMedia(Stream dashzip, string remotePath);
        Task<int> ConvertToMP4Format(string inputFilePath, string outputFilePath);
        Task<int> SegmentToDashFormat(string inputFilePath, string outputFilePath);
        Task<int> ConvertToMP4DashFormat(string inputFilePath, string outputFilePath);
    }
}
