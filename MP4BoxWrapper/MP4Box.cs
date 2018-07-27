using System;
using System.Diagnostics;
using static Helper.FileNameExtensions;

namespace MP4BoxWrapper
{
    public class MP4Box
    {
        private string executable;
        private string outputPath;
        private string inputFilePath;

        public EventHandler ConvertionDone { get; set; }
        public DataReceivedEventHandler OutputReceived { get; set; }
        public DataReceivedEventHandler ErrorReceived { get; set; }

        public MP4Box(string executable, string inputFilePath, string outputPath)
        {
            this.executable = executable;
            this.outputPath = outputPath;
            this.inputFilePath = inputFilePath;
        }

        // Events
        public void OnConvertionTerminated(object sender, EventArgs e) => ConvertionDone?.Invoke(sender, e);
        public void OnOutputReceived(object sender, DataReceivedEventArgs d) => OutputReceived?.Invoke(sender, d);
        public void OnErrorReceived(object sender, DataReceivedEventArgs d) => ErrorReceived?.Invoke(sender, d);


        public int Dashify()
        {
            var filename = GetFileNameFromFilePath(this.inputFilePath);
            var name = GetNameFromFileName(filename);
            var defaultDashArguments = $" -dash 2000 -rap-frag -rap -profile onDemand -out {name}.mpd {filename}#video {filename}#audio";
            return Dashify(defaultDashArguments);
        }

        public int Dashify(string arguments)
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
    }
}