using ContentManager;
using MediaManagerLib;

namespace ContentManagerWeb.Helpers
{
    public static class MediaManagerFactory
    {
        public static MediaManager GetInstance()
        {
            var config = new Configuration();

            return new MediaManager(
                config.FfmpegExecutablePath,
                config.MP4BoxExecutablePath,
                config.SFTPHost,
                config.SFTPUsername,
                config.SFTPPassword,
                config.SFTPPort);
        }
    }
}
