using FFMPEGWrapper.Enums;
using System;
using System.Diagnostics;

namespace FFMPEGWrapper
{
    public class FFMPEG
    {
        private string executable;
        public EventHandler ConvertionDone { get; set; }
        public DataReceivedEventHandler OutputReceived { get; set; }
        public DataReceivedEventHandler ErrorReceived { get; set; }

        public FFMPEG(string executablePath)
        {
            this.executable = executablePath;
        }

        // Events
        public void OnConvertionTerminated(object sender, EventArgs e) => ConvertionDone?.Invoke(sender, e);
        public void OnOutputReceived(object sender, DataReceivedEventArgs d) => OutputReceived?.Invoke(sender, d);
        public void OnErrorReceived(object sender, DataReceivedEventArgs d) => ErrorReceived?.Invoke(sender, d);
        
        private ProcessStartInfo GetProcessInfo(string arguments)
        {
            return new ProcessStartInfo()
            {
                FileName = this.executable,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }

        public int Convert(Profile profile, string inputFilePath, string outputFilePath)
        {
            return Convert(Arguments.GetFromProfile(profile, inputFilePath, outputFilePath));
        }

        public int Convert(Arguments arguments) => Convert(arguments.Parse());

        public int Convert(string arguments)
        {
            using (var process = new Process())
            {
                process.StartInfo = GetProcessInfo(arguments);

                process.Exited += OnConvertionTerminated;
                process.OutputDataReceived += OnOutputReceived;
                process.ErrorDataReceived += OnErrorReceived;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                return process.ExitCode;
            }
        }
    }
}
