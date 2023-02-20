using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace ChurnZero.Sdk.Requests
{
    internal class TrackEventRequest : IChurnZeroHttpRequest
    {
        public TrackEventRequest(ChurnZeroEvent e, string appKey)
        {
            Validator.ValidateObject(e, new ValidationContext(e));
            AppKey = appKey;
            AccountExternalId = e.AccountExternalId;
            ContactExternalId = e.ContactExternalId;
            Description = e.Description;
            EventDate = e.EventDate;
            EventName = e.EventName;
            Quantity = e.Quantity;
            AllowDupes = e.AllowDupes;
        }
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.TrackEvent;
        [Required]
        public string AccountExternalId { get; set; }
        [Required]
        public string ContactExternalId { get; set; }

        [Required]
        public string EventName { get; set; }
        public DateTime? EventDate { get; set; }

        public string Description { get; set; }
        public int? Quantity { get; set; }
        public bool AllowDupes { get; set; }
        //[JsonIgnore]
        //public override Dictionary<string, string> CustomFields { get; set; }
    }
}
