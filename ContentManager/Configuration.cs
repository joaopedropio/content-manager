using Microsoft.Extensions.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace ContentManager
{
    public class Configuration
    {
        public bool isWindowns { get { return RuntimeInformation.IsOSPlatform(OSPlatform.Windows); } }
        public string Port { get; }
        public string Domain { get; }
        public string URL { get; }
        public string FfmpegExecutablePath { get; }
        public string MP4BoxExecutablePath { get; }
        public string TempFolder { get; }
        public string ConvertedFolder { get; }
        public string CompresssedFolder { get; }
        public string SFTPHost { get; }
        public string SFTPUsername { get; }
        public string SFTPPassword { get; }
        public int SFTPPort { get; }

        public Configuration() : this(new ConfigurationBuilder().AddEnvironmentVariables().Build()) { }

        public Configuration(IConfigurationRoot configuration)
        {
            // Url
            this.Domain = configuration.GetValue<string>("API_DOMAIN") ?? "*";
            this.Port = configuration.GetValue<string>("API_PORT") ?? "5000";
            this.URL = string.Format($"http://{this.Domain}:{this.Port}");

            // Executable Paths
            this.FfmpegExecutablePath = configuration.GetValue<string>("FFMPEG_PATH") ?? @"\ffmpeg.exe";
            this.MP4BoxExecutablePath = configuration.GetValue<string>("MP4BOX_PATH") ?? @"\mp4box.exe";

            // Directories
            this.TempFolder = Path.GetTempPath() + (configuration.GetValue<string>("TEMP_FOLDER") ?? @"content-converter\");
            this.ConvertedFolder = this.TempFolder + (configuration.GetValue<string>("CONVERTED_FOLDER") ?? @"converted\");
            this.CompresssedFolder = this.TempFolder + (configuration.GetValue<string>("COMPRESSED_FOLDER") ?? @"compressed\");

            // Content Server SFTP
            this.SFTPHost = configuration.GetValue<string>("SFTP_HOST") ?? "http://content-server";
            var sftpPort = configuration.GetValue<string>("SFTP_PORT") ?? "2222";
            this.SFTPPort = int.Parse(sftpPort);
            this.SFTPUsername = configuration.GetValue<string>("SFTP_USERNAME") ?? "content";
            this.SFTPPassword = configuration.GetValue<string>("SFTP_PASSWORD") ?? "password";
        }
    }
}
