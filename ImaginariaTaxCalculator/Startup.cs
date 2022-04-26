using System.Linq;
using FluentValidation.AspNetCore;
using ImaginariaTaxCalculator.Helpers;
using ImaginariaTaxCalculator.Helpers.AmountFormatters;
using ImaginariaTaxCalculator.Helpers.Calculators;
using ImaginariaTaxCalculator.Interfaces;
using ImaginariaTaxCalculator.Interfaces.Calculators;
using ImaginariaTaxCalculator.Interfaces.Validators;
using ImaginariaTaxCalculator.Models;
using ImaginariaTaxCalculator.Services.Calculators;
using ImaginariaTaxCalculator.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ImaginariaTaxCalculator
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaxCalculator", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<TaxPayerValidator>());

            services.AddMemoryCache();

            services.AddScoped<ITaxCalculator<TaxCalculator>, TaxCalculator>();
            services.AddScoped<ITotalTaxCalculator, TotalTaxCalculator>();
            services.AddScoped<INetIncomeCalculator, NetIncomeCalculator>();

            services.AddScoped<IAmountFormatter<CharityAmountFormatter>, CharityAmountFormatter>();
            services.AddScoped<IAmountFormatter<GrossAmountFormatter>, GrossAmountFormatter>();
            services.AddScoped<IAmountFormatter<SocialTaxAmountFormatter>, SocialTaxAmountFormatter>();

            services.AddScoped<ITaxesCacheHelper, TaxesCacheHelper>();

            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<IValidationHelper<RegexValidationHelper>, RegexValidationHelper>();

            services.AddOptions<Settings>().Bind(Configuration.GetSection("Settings"));

            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaxCalculator v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseResponseCaching();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}