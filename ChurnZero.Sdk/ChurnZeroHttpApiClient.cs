using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChurnZero.Sdk.Models;
using ChurnZero.Sdk.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChurnZero.Sdk
{
    public class ChurnZeroHttpApiClient
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
                //DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
        }

        public HttpResponseMessage SetAttribute(ChurnZeroAttributeModel attributeModel)
        {
            return SetAttributeAsync(attributeModel).GetAwaiter().GetResult();
        }
        public async Task<HttpResponseMessage> SetAttributeAsync(ChurnZeroAttributeModel attributeModel)
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
            var serialized = JsonConvert.SerializeObject(request, Formatting.Indented, _jsonSerializerSettings);
            var requestContent =
                new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }
        public HttpResponseMessage TrackEvent(ChurnZeroEventModel eventModel)
        {
            return TrackEventAsync(eventModel).GetAwaiter().GetResult();
        }

        public async Task<HttpResponseMessage> TrackEventAsync(ChurnZeroEventModel eventModel)
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
                //CustomFields = eventModel.CustomFields,
            };
            string serialized;
            //Getting 422's when submitting custom fields. Disabling support for now.

            //if (request.CustomFields.Any())
            //{
            //    var jObject = JObject.FromObject(request);
            //    foreach (var customField in request.CustomFields)
            //    {
            //        jObject.Add(CustomField.FormatDisplayNameToCustomFieldName(customField.Key), customField.Value);
            //    }
            //    serialized = JsonConvert.SerializeObject(jObject, Formatting.Indented, _jsonSerializerSettings);
            //    var finalObj = JObject.Parse(serialized);
            //    var queryString = HttpUtility.ParseQueryString(string.Empty);
            //    foreach (var prop in finalObj.Properties())
            //    {
            //        queryString[prop.Name] = prop.Value.ToString();
            //    }

            //    var url = $"i?{queryString}";
            //    return await _httpClient.GetAsync(url);

            //}
            //else
            serialized = JsonConvert.SerializeObject(request, Formatting.Indented, _jsonSerializerSettings);
            var requestContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }



    }
}
