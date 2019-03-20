namespace FFMPEGWrapper.Arguments
{
    public class MaxRate : IArgument
    {
        private int value;

        public MaxRate(int value)
        {
            this.value = value;
        }

        public string ToStringRepresentation()
        {
            return $" -maxrate {value}";
        }
    }
}
