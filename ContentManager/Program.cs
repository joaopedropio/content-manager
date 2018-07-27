using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ContentManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new Configuration();

            var web = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options => options.Limits.MaxRequestBodySize = null)
                .UseUrls(config.URL)
                .Build();

            web.Run();
        }
    }
}
