using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Helper
{
    public static class ProcessFactory
    {
        private static ProcessStartInfo GetProcessInfo(string executable, string arguments)
        {
            return new ProcessStartInfo()
            {
                FileName = executable,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }

        public static async Task<int> Execute(string executable, string arguments, DataReceivedEventHandler onErrorDataReceived, DataReceivedEventHandler onOutputDataReceived, EventHandler onExit)
        {
            using (var process = new Process())
            {
                process.StartInfo = GetProcessInfo(executable, arguments);

                process.ErrorDataReceived += onErrorDataReceived;
                process.OutputDataReceived += onOutputDataReceived;
                process.Exited += onExit;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await Task.Run(() => process.WaitForExit());

                return process.ExitCode;
            }
        }
    }
}
