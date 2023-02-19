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
        HttpResponseMessage SetAttributes(params ChurnZeroAttributeModel[] attributes);
        /// <summary>
        /// Sets the attributes on an account or contact asynchronously.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> SetAttributesAsync(params ChurnZeroAttributeModel[] attributes);
        /// <summary>
        /// Sends events in a synchronous call. Events require a contact and account external ID.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        HttpResponseMessage TrackEvents(ChurnZeroEventModel events);
        /// <summary>
        /// Sends events asynchronously. Events require a contact and account external ID.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> TrackEventsAsync(params ChurnZeroEventModel[] events);
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
        public HttpResponseMessage SetAttributes(params ChurnZeroAttributeModel[] attributes)
        {
            return SetAttributesAsync(attributes).GetAwaiter().GetResult();
        }
        /// <inheritdoc/>
        public async Task<HttpResponseMessage> SetAttributesAsync(params ChurnZeroAttributeModel[] attributes)
        {
            Validator.ValidateObject(attributes, new ValidationContext(attributes));
            var requests = attributes.Select(x=> new SetAttributeRequest()
            {
                AppKey = _appKey,
                AccountExternalId = x.AccountExternalId,
                ContactExternalId = x.ContactExternalId,
                Name = x.Name,
                EntityType = x.EntityType,
                Value = x.Value
            }).ToList();
            var serialized = JsonConvert.SerializeObject(requests, Formatting.Indented, _jsonSerializerSettings);
            var requestContent =
                new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }
        /// <inheritdoc/>
        public HttpResponseMessage TrackEvents(ChurnZeroEventModel events)
        {
            return TrackEventsAsync(events).GetAwaiter().GetResult();
        }
        /// <inheritdoc/>
        public async Task<HttpResponseMessage> TrackEventsAsync(params ChurnZeroEventModel[] events)
        {
            Validator.ValidateObject(events, new ValidationContext(events));
            var requests = events.Select(x=> new TrackEventRequest()
            {
                AppKey = _appKey,
                AccountExternalId = x.AccountExternalId,
                ContactExternalId = x.ContactExternalId,
                Description = x.Description,
                EventDate = x.EventDate,
                EventName = x.EventName,
                Quantity = x.Quantity,
                AllowDupes = x.AllowDupes,
            });
            var serialized = JsonConvert.SerializeObject(requests, Formatting.Indented, _jsonSerializerSettings);
            var requestContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }



    }
}
