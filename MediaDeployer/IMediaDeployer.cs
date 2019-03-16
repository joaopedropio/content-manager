using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediaManager
{
    public interface IMediaManager
    {
        void UploadMedia(Stream media, string path);
        Stream DownloadMedia(string path);
        void RemodeMedia(string path);
    }
}
