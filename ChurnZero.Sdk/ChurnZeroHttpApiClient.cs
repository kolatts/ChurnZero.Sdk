using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ChurnZero.Sdk.Models;
using ChurnZero.Sdk.Requests;

namespace ChurnZero.Sdk
{
    public class ChurnZeroHttpApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _appKey;

        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public ChurnZeroHttpApiClient(HttpClient httpClient, string appKey)
        {
            if (string.IsNullOrWhiteSpace(appKey))
                throw new ArgumentNullException(nameof(appKey), "An app key is required.");
            _httpClient = httpClient;
            _appKey = appKey;
        }

        public HttpResponseMessage SetAttribute(ChurnZeroAttributeModel attributeModel, bool ensureSuccessStatusCode = true)
        {
            return SetAttributeAsync(attributeModel, ensureSuccessStatusCode).GetAwaiter().GetResult();
        }
        public async Task<HttpResponseMessage> SetAttributeAsync(ChurnZeroAttributeModel attributeModel, bool ensureSuccessStatusCode = true)
        {
            Validator.ValidateObject(attributeModel, new ValidationContext(attributeModel));
            var request = new SetAttributeRequest()
            {
                AppKey = _appKey,
                AccountExternalId = attributeModel.AccountExternalId,
                ContactExternalId = attributeModel.ContactExternalId,
                Name = attributeModel.Name,
                EntityType = attributeModel.EntityType,
                Value = attributeModel.Value
            };
            var serialized = JsonSerializer.Serialize(request, _serializerOptions);
            var requestContent =
                new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            if (ensureSuccessStatusCode) response.EnsureSuccessStatusCode();
            return response;
        }
        public HttpResponseMessage TrackEvent(ChurnZeroEventModel eventModel, bool ensureSuccessStatusCode = true)
        {
            return TrackEventAsync(eventModel, ensureSuccessStatusCode).GetAwaiter().GetResult();
        }

        public async Task<HttpResponseMessage> TrackEventAsync(ChurnZeroEventModel eventModel, bool ensureSuccessStatusCode = true)
        {
            Validator.ValidateObject(eventModel, new ValidationContext(eventModel));
            var request = new TrackEventRequest()
            {
                AppKey = _appKey,
                AccountExternalId = eventModel.AccountExternalId,
                ContactExternalId = eventModel.ContactExternalId,
                Description = eventModel.Description,
                EventDate = eventModel.EventDate,
                EventName = eventModel.EventName,
                Quantity = eventModel.Quantity,
                AllowDupes = eventModel.AllowDupes,
                //CustomFields = eventModel.CustomFields, //todo these need to be mapped to properties on the request itself
            };
            var serialized = JsonSerializer.Serialize(request, _serializerOptions);
            var requestContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            if (ensureSuccessStatusCode) response.EnsureSuccessStatusCode();
            return response;
        }





    }
}
