using CandidatesChecker.Web.Check.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CandidatesChecker.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            RegisterFileSystemCheck(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "By convention")]
        public void Configure(IApplicationBuilder app)
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
            if (Environment.IsDevelopment())
            {
                folder = @$"{Environment.ContentRootPath}\..\..\fake_documents";
            }
            else
            {
                do
                {
                    Console.Write("Folder to check: ");
                    folder = Console.ReadLine();
                }
                while (string.IsNullOrWhiteSpace(folder));
            }

            services.AddSingleton<IFileSystemCheck>(_ => new FileSystemCheck(folder));
        }
    }
}
