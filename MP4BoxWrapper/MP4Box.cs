using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

        public Task<int> Dashify()
        {
            var filename = GetFileNameFromFilePath(this.inputFilePath);
            var name = GetNameFromFileName(filename);
            var defaultDashArguments = $" -dash 2000 -rap-frag -rap -profile onDemand -out {name}.mpd {filename}#video {filename}#audio";
            return Dashify(this.executable, defaultDashArguments);
        }

        public Task<int> Dashify(string executable, string arguments)
        {
            return Dashify(executable, arguments, ConvertionDone, OutputReceived, ErrorReceived);
        }

        public async Task<int> Dashify(string executable, string arguments, EventHandler ConvertionDone, DataReceivedEventHandler OutputReceived, DataReceivedEventHandler ErrorReceived)
        {
            return await Helper.ProcessFactory.Execute(executable, arguments, ErrorReceived, OutputReceived, ConvertionDone);
        }
    }
}