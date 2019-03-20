using System.Runtime.InteropServices;

namespace FFMPEGWrapper.Arguments
{
    public class InputFilePath : IArgument
    {
        private string value;

        public InputFilePath(string value)
        {
            this.value = value;
        }

        public string ToStringRepresentation()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            return string.Format(" -i {0}", isWindows ? $"\"{value}\"" : value);
        }
    }
}
