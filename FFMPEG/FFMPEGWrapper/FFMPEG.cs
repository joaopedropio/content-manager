using FFMPEGWrapper.Arguments;
using Helper;
using System;
using System.Diagnostics;
using System.Linq;
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

        public async Task<int> Convert(IProfile profile, InputFilePath input, OutputFilePath output)
        {
            var profileArgs = profile.GetArguments().ToArray();
            var args = ArgumentHelper.CreateArgumentString(input, output, profileArgs);
            return await Convert(args);
        }

        public async Task<int> Convert(InputFilePath input, OutputFilePath output, params IArgument[] arguments)
        {
            var args = ArgumentHelper.CreateArgumentString(input, output, arguments);
            return await Convert(args);
        }

        public async Task<int> Convert(string arguments)
        {
            return await ProcessHelper.Execute(this.executable, arguments, this.ErrorReceived, this.OutputReceived, this.ConvertionDone);
        }
    }
}
