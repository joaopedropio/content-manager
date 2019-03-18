using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace MediaManagerLib.Deployer
{
    internal class SFTPMediaDeployer
    {
        private ConnectionInfo connectionInfo;

        internal Action<ulong> ProgressStatus;

        internal SFTPMediaDeployer(string host, string username, string password, int port = 22)
        {
            var authenticationMethod = new PasswordAuthenticationMethod(username, password);
            this.connectionInfo = new ConnectionInfo(host, port, username, authenticationMethod);
        }

        internal Stream DownloadMedia(string remotePath)
        {
            var media = new MemoryStream();
            CreateConnection((client) =>
            {
                client.DownloadFile(remotePath, media);
            });
            return media;
        }

        internal void RemoveMedia(string remotePath)
        {
            CreateConnection((client) =>
            {
                client.DeleteFile(remotePath);
            });
        }

        internal void UploadMedia(Stream media, string remotePath)
        {
            CreateConnection((client) =>
            {
                client.UploadFile(media, remotePath, ProgressStatus);
            });
        }

        internal void UploadDirectory(string localPath, string remotePath)
        {
            CreateConnection((client) =>
            {
                UploadDirectory(client, localPath, remotePath);
            });
        }

        private void UploadDirectory(SftpClient client, string localPath, string remotePath)
        {
            Console.WriteLine("Uploading directory {0} to {1}", localPath, remotePath);

            IEnumerable<FileSystemInfo> infos = new DirectoryInfo(localPath).EnumerateFileSystemInfos();
            foreach (FileSystemInfo info in infos)
            {
                if (info.Attributes.HasFlag(FileAttributes.Directory))
                {
                    string subPath = remotePath + "/" + info.Name;
                    if (!client.Exists(subPath))
                    {
                        client.CreateDirectory(subPath);
                    }
                    UploadDirectory(client, info.FullName, remotePath + "/" + info.Name);
                }
                else
                {
                    using (Stream fileStream = new FileStream(info.FullName, FileMode.Open))
                    {
                        Console.WriteLine( "Uploading {0} ({1:N0} bytes)", info.FullName, ((FileInfo)info).Length);

                        client.UploadFile(fileStream, remotePath + "/" + info.Name);
                    }
                }
            }
        }

        private void CreateConnection(Action<SftpClient> action)
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

        private string GetMediaFolderName(string mediaFolder) =>
            mediaFolder.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1];

        internal void UploadDashMedia(Stream dashzip, string remotePath)
        {
            using (var zip = new ZipArchive(dashzip))
            {
                CreateConnection((client) =>
                {
                    var mediaFolderName = GetMediaFolderName(remotePath);
                    if (client.Exists(remotePath))
                        throw new Exception(string.Format("Directory already exists: {0}.", remotePath));

                    client.CreateDirectory(mediaFolderName);

                    foreach (var zipEntry in zip.Entries)
                    {
                        var stream = zipEntry.Open();
                        var fullFilePath = remotePath + zipEntry.Name;

                        if (client.Exists(fullFilePath))
                            throw new Exception(string.Format("File already exists: {0}.", fullFilePath));

                        client.UploadFile(stream, fullFilePath);
                    }
                });
            }
        }
    }
}
