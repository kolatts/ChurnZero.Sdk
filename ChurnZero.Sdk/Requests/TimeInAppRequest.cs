using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;

namespace ChurnZero.Sdk.Requests
{
    internal class TimeInAppRequest : IChurnZeroHttpRequest
    {
        public TimeInAppRequest(ChurnZeroTimeInApp t, string appKey)
        {
            Validator.ValidateObject(t, new ValidationContext(t));
            AccountExternalId = t.AccountExternalId;
            ContactExternalId = t.ContactExternalId;
            StartDate = t.StartDate.GetValueOrDefault();
            EndDate = t.EndDate.GetValueOrDefault();
            Module = t.Module;
            AppKey = appKey;
        }
        public string AccountExternalId { get; set; }
        public string ContactExternalId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Module { get; set; }
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.TimeInApp;
    }
}
