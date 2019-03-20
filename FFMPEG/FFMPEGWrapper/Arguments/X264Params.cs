namespace FFMPEGWrapper.Arguments
{
    public class X264Params : IArgument
    {
        public int KeyInt { get; set; } = -1;
        public int MinKeyInt { get; set; } = -1;
        public bool NoSceneCut { get; set; }

        public string ToStringRepresentation()
        {
            var result = "'";

            if (KeyInt > 0) result += $"keyint={KeyInt}:";
            if (MinKeyInt > 0) result += $"min-keyint={MinKeyInt}:";
            if (NoSceneCut) result += $"no-scenecut:";

            result = result.TrimEnd(':');
            result += "'";

            return $" -x264-params {result}";
        }
    }
}
