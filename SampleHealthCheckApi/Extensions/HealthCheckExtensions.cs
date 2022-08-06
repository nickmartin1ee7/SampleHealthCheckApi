using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using SampleHealthCheckApi.Controllers;

namespace SampleHealthCheckApi.Extensions
{
    internal static class HealthCheckExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            services.AddTransient<WeatherForecastController>();

            services.AddHealthChecks()
                .AddCheck("Alive", () => HealthCheckResult.Healthy())
                .AddCheck<WeatherForecastHealthCheck>("Weather Forecast");

            return services;
        }
    }

    internal class WeatherForecastHealthCheck : IHealthCheck
    {
        private readonly WeatherForecastController _forecastController;

        public WeatherForecastHealthCheck(WeatherForecastController forecastController)
        {
            _forecastController = forecastController;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_forecastController.Get().Any()
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy());
        }
    }
}
