using Investimentos.Domain.Interfaces.Clients;
using Investimentos.Infra.Clients.Fundos;
using Investimentos.Infra.Clients.RendaFixa;
using Investimentos.Infra.Clients.TesouroDireto;
using Investimentos.Infra.Util;
using Investimentos.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Investimentos.Api
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
            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

            services.AddMemoryCache();

            services.AddControllers().AddMetrics();

            services.AddHttpClient();
            services.AddSingleton<ApiJsonSerializer>();
            services.AddSingleton<IFundosClient, FundosClient>();
            services.AddSingleton<IRendaFixaClient, RendaFixaClient>();
            services.AddSingleton<ITesouroDiretoClient, TesouroDiretoClient>();
            services.AddTransient<InvestimentoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
