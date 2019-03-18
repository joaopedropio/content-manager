using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static Helper.FileHelper;

namespace MP4BoxWrapper
{
    public class MP4Box
    {
        private string executable;

        public EventHandler ConvertionDone { get; set; }
        public DataReceivedEventHandler OutputReceived { get; set; }
        public DataReceivedEventHandler ErrorReceived { get; set; }

        public MP4Box(string executable)
        {
            this.executable = executable;
        }

        public Task<int> Dashify(string inputFilePath, string outputPath)
        {
            var filename = GetFileNameFromFilePath(inputFilePath);
            var name = GetNameFromFileName(filename);
            var defaultDashArguments = $" -dash 2000 -rap-frag -rap -profile onDemand -out {name}.mpd {filename}#video {filename}#audio";
            return Dashify(defaultDashArguments);
        }

        public async Task<int> Dashify(string arguments)
        {
            return await Dashify(this.executable, arguments, this.ConvertionDone, this.OutputReceived, this.ErrorReceived);
        }

        private async Task<int> Dashify(string executable, string arguments, EventHandler ConvertionDone, DataReceivedEventHandler OutputReceived, DataReceivedEventHandler ErrorReceived)
        {
            return await Helper.ProcessHelper.Execute(executable, arguments, ErrorReceived, OutputReceived, ConvertionDone);
        }
    }
}