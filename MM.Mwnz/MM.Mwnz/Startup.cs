using Microsoft.OpenApi.Models;
using MM.Mwnz.Services;
using Mwnz.Client.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace MM.Mwnz
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: Is swagger output, and potential api testing page required?
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MMMwnz", Version = "v1" });
            });

            ConfigureMwnzClient(services, Configuration);

            // TODO: If more services are added, move to separate method
            services.AddTransient<IMwnzClientService, MwnzClientService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/plain; charset=us-ascii";

                        await context.Response.WriteAsync(context.Response.StatusCode + ": Unexpected error\r\n");
                        await context.Response.WriteAsync(new string(' ', 512)); // IE padding
                    });
                });
            }

            app.UseStatusCodePages();

            app.UseSwagger();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MMMwnz V1");
            });

            app.UseMvc();
        }

        private static void ConfigureMwnzClient(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<MwnzClient>();

            services.AddTransient(provider =>
            {
                var httpClient = provider.GetRequiredService<MwnzClient>();

                httpClient.BaseUrl = configuration.GetValue<string>("MwnzClientBaseUrl");

                return httpClient;
            });
        }
    }
}
