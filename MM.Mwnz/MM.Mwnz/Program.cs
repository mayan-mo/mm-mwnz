using Microsoft.AspNetCore;

namespace MM.Mwnz
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            TimeSpan requestTimeOut = config.GetValue<TimeSpan>("RequestTimeOut");

            WebHost.CreateDefaultBuilder(args)
                // TODO: SSL, authentication requirements are probably required for a production service in use, more configuration to be added
                .UseUrls("http://*:5201")
                .UseStartup<Startup>()
                .UseKestrel(o =>
                {
                    o.Limits.KeepAliveTimeout = requestTimeOut;
                })
                // TODO: Logging requirements might be different for a production service in use, if so will need additional configuration
                .Build()
                .Run();
        }
    }
}


