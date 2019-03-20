namespace FFMPEGWrapper.Arguments
{
    class AudioChannels : IArgument
    {
        private int value;

        public AudioChannels(int value)
        {
            this.value = value;
        }

        public string ToStringRepresentation()
        {
            return $" -ac {value}";
        }
    }
}
