using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediaManager
{
    public interface IMediaManager
    {
        void UploadMedia(Stream media, string remotePath);
        Stream DownloadMedia(string remotePath);
        void RemoveMedia(string remotePath);
        void UploadDirectory(string localPath, string remotePath);
        void UploadDashMedia(Stream dashzip, string remotePath);
    }
}
