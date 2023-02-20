using System;
using System.Net.Http;
using ChurnZero.Sdk.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChurnZero.Sdk
{
    /// <summary>
    /// Extensions to add the ChurnZero SDK easily.
    /// </summary>
    public static class DependencyExtensions
    {
        private const string ClientName = "ChurnZeroHttpApiClient";

        /// <summary>
        /// Adds an <see cref="IChurnZeroHttpApiClient"/> with the options that you specify. Note that an <see cref="IHttpClientFactory"/> is used.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddChurnZeroSdk(this IServiceCollection services, Action<ChurnZeroClientOptions> configureOptions)
        {
            var options = new ChurnZeroClientOptions();
            configureOptions(options);
            services.AddHttpClient(ClientName, client =>
            {
                client.BaseAddress = new Uri(options.Url);
            });
            services.AddSingleton<IChurnZeroHttpApiClient, ChurnZeroHttpApiClient>(sp =>
            {
                var httpClient = sp.GetRequiredService<IHttpClientFactory>()
                    .CreateClient(ClientName);
                return new ChurnZeroHttpApiClient(httpClient, options.AppKey);
            });
            return services;
        }
    }
}
