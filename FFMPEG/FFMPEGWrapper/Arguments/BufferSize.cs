namespace FFMPEGWrapper.Arguments
{
    public class BufferSize : IArgument
    {
        private int value;

        public BufferSize(int value)
        {
            this.value = value;
        }

        public string ToStringRepresentation()
        {
            return $" -bufsize {value}";
        }
    }
}
