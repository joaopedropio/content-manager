using FFMPEGWrapper.Arguments;
using FFMPEGWrapper.Enums;
using System.Collections.Generic;

namespace FFMPEGWrapper.Profiles
{
    public class SimpleDash : IProfile
    {
        private IList<IArgument> arguments;

        public SimpleDash(InputFilePath input, OutputFilePath output)
        {
            this.arguments = new List<IArgument>()
            {
                input, output,
                new OverRightOutputFiles(true),
                new AudioCodec(AudioCodecEnum.Aac),
                new AudioChannels(2),
                new AudioBitrate(128_000),
                new VideoCodec(VideoCodecEnum.Libx264),
                new X264Params()
                {
                    KeyInt = 24,
                    MinKeyInt = 24,
                    NoSceneCut = true
                },
                new VideoBitrate(1_500_000),
                new MaxRate(1_500_000),
                new BufferSize(1_000_000),
                new VideoFilters("scale=-1:720")
            }
        }

        public IList<IArgument> GetArguments()
        {
            throw new System.NotImplementedException();
        }
    }
}
