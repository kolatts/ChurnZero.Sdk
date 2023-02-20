using System.ComponentModel.DataAnnotations;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;

namespace ChurnZero.Sdk.Requests
{
    internal class IncrementAttributeRequest : IChurnZeroHttpRequest
    {
        public IncrementAttributeRequest(ChurnZeroAttribute a, string appKey)
        {
            Validator.ValidateObject(a, new ValidationContext(a));
            AppKey = appKey;
            AccountExternalId = a.AccountExternalId;
            ContactExternalId = a.ContactExternalId;
            Name = a.Name;
            Entity = a.EntityType.ToString().ToLower();
            Value = a.Value;
        }
        public string AccountExternalId { get; set; }
        public string ContactExternalId { get; set; }
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.IncrementAttribute;
        public string Name { get; set; }
        public string Value { get; set; }

        public string Entity { get; set; }

    }
}