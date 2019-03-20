namespace FFMPEGWrapper.Arguments
{
    public class VideoFilters : IArgument
    {
        private string value;

        public VideoFilters(string value)
        {
            this.value = value;
        }

        public string ToStringRepresentation()
        {
            return $" -vf {value}";
        }
    }
}
