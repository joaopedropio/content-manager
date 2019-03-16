using FFMPEGWrapper.Enums;
using Helper;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FFMPEGWrapper
{
    public class FFMPEG
    {
        public EventHandler ConvertionDone { get; set; }
        public DataReceivedEventHandler OutputReceived { get; set; }
        public DataReceivedEventHandler ErrorReceived { get; set; }

        private string executable;

        public FFMPEG(string executablePath)
        {
            this.executable = executablePath;
        }

        public async Task<int> Convert(Profile profile, string inputFilePath, string outputFilePath)
        {
            return await Convert(Arguments.GetFromProfile(profile, inputFilePath, outputFilePath));
        }

        public async Task<int> Convert(Arguments arguments)
        {
            return await Convert(arguments.Parse());
        }

        public async Task<int> Convert(string arguments)
        {
            return await Convert(arguments, this.ErrorReceived, this.OutputReceived, this.ConvertionDone);
        }

        public async Task<int> Convert(string arguments, DataReceivedEventHandler onErrorDataReceived, DataReceivedEventHandler onOutputDataReceived, EventHandler onExit)
        {
            return await ProcessFactory.Execute(this.executable, arguments, onErrorDataReceived, onOutputDataReceived, onExit);
        }
    }
}
