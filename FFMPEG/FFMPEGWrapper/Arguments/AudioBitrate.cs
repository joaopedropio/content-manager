namespace FFMPEGWrapper.Arguments
{
    public class AudioBitrate : IArgument
    {
        private int value;

        public AudioBitrate(int value)
        {
            this.value = value;
        }

        public string ToStringRepresentation()
        {
            return $" -ab {value}";
        }
    }
}
