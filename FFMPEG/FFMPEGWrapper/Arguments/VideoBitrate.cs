namespace FFMPEGWrapper.Arguments
{
    public class VideoBitrate : IArgument
    {
        private int value;

        public VideoBitrate(int value)
        {
            this.value = value;
        }

        public string ToStringRepresentation()
        {
            return $" -b:v {value}";
        }
    }
}
