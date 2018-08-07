using FFMPEGWrapper.Enums;
using System;
using System.Runtime.InteropServices;

namespace FFMPEGWrapper
{
    public class Arguments
    {
        public bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public bool OverRightOutputFiles { get; set; }
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
        public int? AudioChannels { get; set; }
        public AudioCodec AudioCodec { get; set; }
        public VideoCodec VideoCodec { get; set; }
        public int? AudioBitrate { get; set; }
        public X264Params X264Params { get; set; }
        public int? VideoBitrate { get; set; }
        public int? MaxRate { get; set; }
        public int? BufferSize { get; set; }
        public string VideoFilters { get; set; }

        private string ParseOverRightOutputFiles() => OverRightOutputFiles ? " -y" : "";
        private string ParseInputFilePath() => string.Format(" -i {0}", IsWindows ? $"\"{InputFilePath}\"" : InputFilePath);
        private string ParseOutputFilePath() => string.Format(" {0}", IsWindows ? $"\"{OutputFilePath}\"" : OutputFilePath);
        private string ParseAudioChannels() => AudioChannels == null ? "" : $" -ac {AudioChannels}";
        private string ParseAudioCodec() => $" -c:a {this.AudioCodec.ToString().ToLower()}";
        private string ParseVideoCodec() => $" -c:v {this.VideoCodec.ToString().ToLower()}";
        private string ParseAudioBitrate() => AudioBitrate == null ? "" : $" -ab {AudioBitrate}";
        private string ParseX264Params() => X264Params == null ? "" : $" -x264-params {X264Params.ToString()}";
        private string ParseVideoBitrate() => VideoBitrate == null ? "" : $" -b:v {VideoBitrate}";
        private string ParseMaxRate() => MaxRate == null ? "" : $" -maxrate {MaxRate}";
        private string ParseBufferSize() => BufferSize == null ? "" : $" -bufsize {BufferSize}";
        private string ParseVideoFilters() => VideoFilters == null? "" : $" -vf {VideoFilters}";

        public string Parse()
        {
            return string.Concat(
                ParseOverRightOutputFiles(), ParseInputFilePath(), ParseAudioChannels(), ParseAudioCodec(), ParseVideoCodec(), ParseAudioBitrate(),
                ParseX264Params(), ParseVideoBitrate(), ParseMaxRate(), ParseBufferSize(), ParseVideoFilters(), ParseOutputFilePath()
            );
        }

        public static Arguments GetFromProfile(Profile profile, string inputFilePath, string outputFilePath)
        {
            switch (profile)
            {
                case Profile.SimpleMP4:
                    return new Arguments()
                    {
                        InputFilePath = inputFilePath,
                        OutputFilePath = outputFilePath,
                        VideoCodec = VideoCodec.Libx264,
                        AudioCodec = AudioCodec.Aac,
                    };
                case Profile.EspecificForDash:
                    return new Arguments()
                    {
                        OverRightOutputFiles = true,
                        InputFilePath = inputFilePath,
                        OutputFilePath = outputFilePath,
                        AudioCodec = AudioCodec.Aac,
                        AudioChannels = 2,
                        AudioBitrate = 128_000,
                        VideoCodec = VideoCodec.Libx264,
                        X264Params = new X264Params()
                        {
                            KeyInt = 24,
                            MinKeyInt = 24,
                            NoSceneCut = true
                        },
                        VideoBitrate = 1_500_000,
                        MaxRate = 1_500_000,
                        BufferSize = 1_000_000,
                        VideoFilters = "scale=-1:720"
                    };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
