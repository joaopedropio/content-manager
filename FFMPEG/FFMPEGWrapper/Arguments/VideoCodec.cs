using FFMPEGWrapper.Enums;

namespace FFMPEGWrapper.Arguments
{
    public class VideoCodec : IArgument
    {
        private VideoCodecEnum value;

        public VideoCodec(VideoCodecEnum value)
        {
            this.value = value;
        }

        public string ToStringRepresentation()
        {
            return $" -c:v {this.value.ToString().ToLower()}";
        }
    }
}
