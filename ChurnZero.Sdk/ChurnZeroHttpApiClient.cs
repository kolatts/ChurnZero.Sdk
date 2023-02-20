using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChurnZero.Sdk.Models;
using ChurnZero.Sdk.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChurnZero.Sdk
{
    /// <summary>
    /// A client that makes HTTP calls to Churn Zero's HTTP API (distinct from REST API) to <i>push data into Churn Zero</i>
    /// </summary>
    public interface IChurnZeroHttpApiClient
    {
        /// <summary>
        /// Sets the attributes on an account or contact in a synchronous call.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        HttpResponseMessage SetAttributes(params ChurnZeroAttribute[] attributes);
        /// <summary>
        /// Sets the attributes on an account or contact asynchronously.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> SetAttributesAsync(params ChurnZeroAttribute[] attributes);
        /// <summary>
        /// Supports numeric attributes only. Value of attribute is what the increment is.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        HttpResponseMessage IncrementAttributes(params ChurnZeroAttribute[] attributes);

        /// <summary>
        /// Supports numeric attributes only. Value of attribute is what the increment is.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> IncrementAttributesAsync(params ChurnZeroAttribute[] attributes);
        /// <summary>
        /// Sends events in a synchronous call. Events require a contact and account external ID.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        HttpResponseMessage TrackEvents(params ChurnZeroEvent[] events);
        /// <summary>
        /// Sends events asynchronously. Events require a contact and account external ID.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> TrackEventsAsync(params ChurnZeroEvent[] events);
        /// <summary>
        /// Tracks time in apps synchronously. Start and End Dates are required.
        /// </summary>
        /// <param name="timeInApps"></param>
        /// <returns></returns>
        HttpResponseMessage TrackTimeInApps(params ChurnZeroTimeInApp[] timeInApps);
        /// <summary>
        /// Tracks time in apps asynchronously. Start and End Dates are required.
        /// </summary>
        /// <param name="timeInApps"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> TrackTimeInAppsAsync(params ChurnZeroTimeInApp[] timeInApps);
    }
    /// <inheritdoc cref="IChurnZeroHttpApiClient"/>
    public class ChurnZeroHttpApiClient : IChurnZeroHttpApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _appKey;

        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public ChurnZeroHttpApiClient(HttpClient httpClient, string appKey)
        {
            if (string.IsNullOrWhiteSpace(appKey))
                throw new ArgumentNullException(nameof(appKey), "An app key is required.");
            _httpClient = httpClient;
            _appKey = appKey;
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
            };
        }
        /// <inheritdoc/>
        public HttpResponseMessage SetAttributes(params ChurnZeroAttribute[] attributes) => SetAttributesAsync(attributes).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> SetAttributesAsync(params ChurnZeroAttribute[] attributes)
        {
            var requests = attributes.Select(x=> new SetAttributeRequest(x, _appKey)).ToList();
            var serialized = JsonConvert.SerializeObject(requests, Formatting.Indented, _jsonSerializerSettings);
            var requestContent =
                new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }
        /// <inheritdoc/>
        public HttpResponseMessage IncrementAttributes(params ChurnZeroAttribute[] attributes) => IncrementAttributesAsync(attributes).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> IncrementAttributesAsync(params ChurnZeroAttribute[] attributes)
        {
            var requests = attributes.Select(x => new IncrementAttributeRequest(x, _appKey)).ToList();
            var serialized = JsonConvert.SerializeObject(requests, Formatting.Indented, _jsonSerializerSettings);
            var requestContent =
                new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }
        /// <inheritdoc/>
        public HttpResponseMessage TrackEvents(params ChurnZeroEvent[] events) => TrackEventsAsync(events).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> TrackEventsAsync(params ChurnZeroEvent[] events)
        {
            var requests = events.Select(x=> new TrackEventRequest(x, _appKey)).ToList();
            var serialized = JsonConvert.SerializeObject(requests, Formatting.Indented, _jsonSerializerSettings);
            var requestContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }
        /// <inheritdoc/>
        public HttpResponseMessage TrackTimeInApps(params ChurnZeroTimeInApp[] timeInApps) => TrackTimeInAppsAsync(timeInApps).GetAwaiter().GetResult();
        /// <inheritdoc/>
        public async Task<HttpResponseMessage> TrackTimeInAppsAsync(params ChurnZeroTimeInApp[] timeInApps)
        {
            var requests = timeInApps.Select(x => new TimeInAppRequest(x, _appKey)).ToList();
            var serialized = JsonConvert.SerializeObject(requests, Formatting.Indented, _jsonSerializerSettings);
            var requestContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }
    }
}
