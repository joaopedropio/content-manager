using FFMPEGWrapper.Enums;

namespace FFMPEGWrapper.Arguments
{
    public class AudioCodec : IArgument
    {
        private AudioCodecEnum value;

        public AudioCodec(AudioCodecEnum value)
        {
            this.value = value;
        }

        public  string ToStringRepresentation()
        {
            return $" -c:a {this.value.ToString().ToLower()}";
        }
    }
}
