using System;
using App.Metrics;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;
using App.Metrics.Health;
using App.Metrics.Health.Checks.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImplementandoHealthcheck.Api
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
            var metrics = new MetricsBuilder()
                                .Configuration.Configure(
                                    options =>
                                    {
                                        options.AddServerTag();
                                        options.AddEnvTag();
                                        options.AddAppTag();
                                    })
                                .OutputMetrics.AsPrometheusPlainText()                                                                
                                .Build();

            services.AddMetrics(metrics);

            var health = AppMetricsHealth.CreateDefaultBuilder()//.Configuration.Configure(new HealthOptions() { Enabled = true, ApplicationName = "http://health.local.com", ReportingEnabled = true })
            .Report.ToMetrics(metrics)
            .HealthChecks.AddSqlCachedCheck("Teste de conexão com o banco.", () => new MySql.Data.MySqlClient.MySqlConnection("Server=mysql;Database=healthcheck;Uid=healthcheckuser;Pwd=healthcheckpws;SslMode=none;"),
                                          TimeSpan.FromSeconds(10),
                                          TimeSpan.FromMinutes(1))
            .HealthChecks.AddPingCheck("Google Ping", "google.com", TimeSpan.FromSeconds(10))
            .HealthChecks.AddHttpGetCheck("GitHub", new Uri("https://github.com"), TimeSpan.FromSeconds(10))
            .BuildAndAddTo(services);

            services.AddHealth(health);

            services.AddMetricsEndpoints();
            services.AddHealthEndpoints();
            services.AddMetricsTrackingMiddleware();
            services.AddMvc().AddMetrics();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // To add all supported endpoints
            app.UseHealthAllEndpoints();
            app.UseMetricsAllEndpoints();

            app.UseMetricsAllMiddleware();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
