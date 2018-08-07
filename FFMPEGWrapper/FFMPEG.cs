using FFMPEGWrapper.Enums;
using Helper;
using System;
using System.Diagnostics;


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

        public Process Convert(Profile profile, string inputFilePath, string outputFilePath)
        {
            return Convert(Arguments.GetFromProfile(profile, inputFilePath, outputFilePath));
        }

        public Process Convert(Arguments arguments)
        {
            return Convert(arguments.Parse());
        }

        public Process Convert(string arguments)
        {
            return Convert(arguments, this.ErrorReceived, this.OutputReceived, this.ConvertionDone);
        }

        public Process Convert(string arguments, DataReceivedEventHandler onErrorDataReceived, DataReceivedEventHandler onOutputDataReceived, EventHandler onExit)
        {
            return ProcessFactory.GetProcess(this.executable, arguments, onErrorDataReceived, onOutputDataReceived, onExit);
        }
    }
}
