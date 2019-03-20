namespace FFMPEGWrapper.Arguments
{
    public class OverRightOutputFiles : IArgument
    {
        private bool value;

        public OverRightOutputFiles(bool value)
        {
            this.value = value;
        }

        public string ToStringRepresentation()
        {
            return value ? " -y" : string.Empty;
        }
    }
}
