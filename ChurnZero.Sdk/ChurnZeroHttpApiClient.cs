using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ChurnZero.Sdk.Models;
using ChurnZero.Sdk.Models.Requests;

namespace ChurnZero.Sdk
{
    public class ChurnZeroHttpApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _appKey;
        public ChurnZeroHttpApiClient(HttpClient httpClient, string appKey)
        {
            _httpClient = httpClient;
            _appKey = appKey;
        }

        public HttpResponseMessage SetAttribute(ChurnZeroAttributeModel attributeModel)
        {
            return SetAttributeAsync(attributeModel).GetAwaiter().GetResult();
        }
        public async Task<HttpResponseMessage> SetAttributeAsync(ChurnZeroAttributeModel attributeModel, bool ensureSuccessStatusCode = true)
        {
            Validator.ValidateObject(attributeModel, new ValidationContext(attributeModel));
            var request = new SetAttributeRequest()
            {
                ContactExternalId = attributeModel.ContactExternalId,
                AccountExternalId = attributeModel.AccountExternalId,
                Name = attributeModel.Name,
                AppKey = _appKey,
                Entity = attributeModel.Entity,
                Value = attributeModel.Value
            };
            var serialized = JsonSerializer.Serialize(request, new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var requestContent =
                new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            if (ensureSuccessStatusCode)
                response.EnsureSuccessStatusCode();
            return response;
        }

        //public async Task<HttpResponseMessage> TrackEvent(ChurnZeroEventModel eventModel)
        //{
        //    Validator.ValidateObject(eventModel, new ValidationContext(eventModel));
        //    var response = await _httpClient.PostAsync("i", new StringContent(JsonSerializer.Serialize(new SetAttributeRequest()
        //    {

        //    })))
        //}

    }
}
