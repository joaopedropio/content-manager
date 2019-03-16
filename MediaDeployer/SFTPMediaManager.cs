using Renci.SshNet;
using System;
using System.IO;

namespace MediaManager
{
    public class SFTPMediaManager : IMediaManager
    {
        private ConnectionInfo connectionInfo;

        public SFTPMediaManager(string host, string username, string password, int port = 22)
        {
            var authenticationMethod = new PasswordAuthenticationMethod(username, password);
            this.connectionInfo = new ConnectionInfo(host, port, username, authenticationMethod);
        }

        public Stream DownloadMedia(string path)
        {
            var media = new MemoryStream();
            CreateConnection((client) =>
            {
                client.DownloadFile(path, media);
            });
            return media;
        }

        public void RemodeMedia(string path)
        {
            CreateConnection((client) =>
            {
                client.DeleteFile(path);
            });
        }

        public void UploadMedia(Stream media, string path)
        {
            CreateConnection((client) =>
            {
                client.UploadFile(media, path);
            });
        }

        public void CreateConnection(Action<SftpClient> action)
        {
            using (var client = new SftpClient(this.connectionInfo))
            {
                try
                {
                    client.Connect();
                    action.Invoke(client);
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    // log here
                    throw ex;
                }
            }
        }
    }
}
