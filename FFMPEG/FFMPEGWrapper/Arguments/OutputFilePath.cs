using System.Runtime.InteropServices;

namespace FFMPEGWrapper.Arguments
{
    public class OutputFilePath : IArgument
    {
        private string value;

        public OutputFilePath(string value)
        {
            this.value = value;
        }

        public string ToStringRepresentation()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            return string.Format(" {0}", isWindows ? $"\"{value}\"" : value);
        }
    }
}
