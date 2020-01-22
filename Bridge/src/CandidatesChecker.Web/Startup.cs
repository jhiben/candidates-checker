using CandidatesChecker.Web.Check.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CandidatesChecker.Web
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
            services.AddCors();
            services.AddControllers();

            RegisterFileSystemCheck(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseCors(b =>
            {
                b.AllowAnyOrigin();
                b.AllowAnyMethod();
                b.AllowAnyHeader();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterFileSystemCheck(IServiceCollection services)
        {
            string? folder = null;
            do
            {
                Console.Write("Folder to check: ");
                folder = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(folder));

            services.AddSingleton<IFileSystemCheck>(_ => new FileSystemCheck(folder));
        }
    }
}
